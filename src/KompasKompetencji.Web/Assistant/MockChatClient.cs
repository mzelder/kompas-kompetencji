using System.Runtime.CompilerServices;
using KompasKompetencji.Web.Assistant.KnowledgeBase;
using Microsoft.Extensions.AI;

namespace KompasKompetencji.Web.Assistant;

public sealed class MockChatClient : IChatClient
{
    private const int QuoteLength = 600;

    private const string NotFoundAnswer =
        "**Nie znalazłem tego w bazie wiedzy.** Nie chcę zgadywać w sprawie zasad oceny, " +
        "dlatego proponuję sprawdzić to w przepisach albo zapytać komórkę kadrową.";

    public async Task<ChatResponse> GetResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default) =>
        await GetStreamingResponseAsync(messages, options, cancellationToken).ToChatResponseAsync(cancellationToken);

    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await Task.Delay(400, cancellationToken);

        foreach (var token in Tokenize(BuildAnswer(KnowledgePrompt.Parse(options?.Instructions))))
        {
            await Task.Delay(15, cancellationToken);
            yield return new ChatResponseUpdate(ChatRole.Assistant, token);
        }
    }

    public object? GetService(Type serviceType, object? serviceKey = null) =>
        serviceKey is null && serviceType.IsInstanceOfType(this) ? this : null;

    public void Dispose()
    {
    }

    private static string BuildAnswer(IReadOnlyList<(string Title, string Text)> fragments)
    {
        if (fragments.Count == 0)
            return NotFoundAnswer;

        var (title, text) = fragments[0];
        var quote = text.Length <= QuoteLength ? text : $"{text[..QuoteLength].TrimEnd()}…";
        var answer = $"Najlepiej pasujący fragment bazy wiedzy to **{title}** [1]:\n\n> {quote.Replace("\n", "\n> ")}";
        if (fragments.Count > 1)
            answer += $"\n\nWięcej na ten temat: {fragments[1].Title} [2].";
        return answer;
    }

    private static IEnumerable<string> Tokenize(string text)
    {
        var start = 0;
        for (var i = 1; i <= text.Length; i++)
        {
            if (i == text.Length || (char.IsWhiteSpace(text[i]) && !char.IsWhiteSpace(text[i - 1])))
            {
                yield return text[start..i];
                start = i;
            }
        }
    }
}
