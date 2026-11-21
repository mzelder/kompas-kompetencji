using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using KompasKompetencji.Web.Assistant.KnowledgeBase;
using Microsoft.Extensions.AI;

namespace KompasKompetencji.Web.Assistant;

public sealed class KnowledgeBaseChatClient(IChatClient innerClient, Bm25Retriever retriever) : DelegatingChatClient(innerClient)
{
    public const int FragmentCount = 5;
    public const int SnippetLength = 300;

    public override async Task<ChatResponse> GetResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default) =>
        await GetStreamingResponseAsync(messages, options, cancellationToken).ToChatResponseAsync(cancellationToken);

    public override async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var conversation = messages.ToList();
        var question = conversation.LastOrDefault(m => m.Role == ChatRole.User)?.Text;

        if (EvaluationLevelGuard.IsLevelRequest(question))
        {
            yield return new ChatResponseUpdate(ChatRole.Assistant, EvaluationLevelGuard.Refusal);
            yield break;
        }

        var fragments = retriever.Search(SearchQuery.FromConversation(conversation), FragmentCount);
        var requestOptions = options?.Clone() ?? new ChatOptions();
        var knowledge = KnowledgePrompt.Build(fragments);
        requestOptions.Instructions = string.IsNullOrWhiteSpace(requestOptions.Instructions)
            ? knowledge
            : $"{requestOptions.Instructions}\n\n{knowledge}";

        var answer = new StringBuilder();
        await foreach (var update in base.GetStreamingResponseAsync(WithoutReasoning(conversation), requestOptions, cancellationToken))
        {
            answer.Append(update.Text);
            yield return update;
        }

        var cited = KnowledgePrompt.CitedNumbers(answer.ToString(), fragments.Count);
        if (cited.Count == 0)
            yield break;

        var citations = new TextContent(string.Empty)
        {
            Annotations = [.. cited.Select(n => ToCitation(n, fragments[n - 1].Chunk))],
        };
        yield return new ChatResponseUpdate(ChatRole.Assistant, [citations]);
    }

    private static List<ChatMessage> WithoutReasoning(IEnumerable<ChatMessage> conversation) =>
        conversation
            .Select(m => m.Contents.Any(c => c is TextReasoningContent)
                ? new ChatMessage(m.Role, [.. m.Contents.Where(c => c is not TextReasoningContent)]) { AuthorName = m.AuthorName, MessageId = m.MessageId }
                : m)
            .Where(m => m.Contents.Count > 0)
            .ToList();

    private static CitationAnnotation ToCitation(int number, KnowledgeChunk chunk) => new()
    {
        FileId = number.ToString(CultureInfo.InvariantCulture),
        Title = chunk.Title,
        Snippet = chunk.Text.Length <= SnippetLength ? chunk.Text : $"{chunk.Text[..SnippetLength].TrimEnd()}…",
    };
}
