using System.Runtime.CompilerServices;
using KompasKompetencji.Web.Assistant;
using KompasKompetencji.Web.Assistant.KnowledgeBase;
using Microsoft.Extensions.AI;

namespace KompasKompetencji.Tests.Assistant;

public class KnowledgeBaseChatClientTests
{
    [Fact]
    public async Task SendsFragmentsInInstructionsAndKeepsOriginalInstructions()
    {
        var model = new RecordingChatClient("Termin wynosi 7 dni [1].");
        var client = new KnowledgeBaseChatClient(model, TestKnowledgeBase.Retriever);

        await Collect(client, "Ile dni ma pracownik na sprzeciw od oceny okresowej?", new ChatOptions { Instructions = "Prompt systemowy" });

        Assert.NotNull(model.LastOptions?.Instructions);
        Assert.StartsWith("Prompt systemowy", model.LastOptions.Instructions);
        Assert.Contains(KnowledgePrompt.Header, model.LastOptions.Instructions);
        Assert.Contains("Art. 83.", model.LastOptions.Instructions);
    }

    [Fact]
    public async Task DoesNotChangeCallerOptions()
    {
        var model = new RecordingChatClient("Odpowiedź.");
        var client = new KnowledgeBaseChatClient(model, TestKnowledgeBase.Retriever);
        var options = new ChatOptions { Instructions = "Prompt systemowy" };

        await Collect(client, "Ile dni na sprzeciw?", options);

        Assert.Equal("Prompt systemowy", options.Instructions);
    }

    [Fact]
    public async Task AddsCitationsOnlyForCitedFragments()
    {
        var model = new RecordingChatClient("Pierwsza informacja [2]. Druga informacja [2, 4].");
        var client = new KnowledgeBaseChatClient(model, TestKnowledgeBase.Retriever);

        var updates = await Collect(client, "Ile dni ma pracownik na sprzeciw od oceny okresowej?");

        var citations = Citations(updates);
        var fragments = TestKnowledgeBase.Retriever.Search(SearchQuery.FromConversation([new(ChatRole.User, "Ile dni ma pracownik na sprzeciw od oceny okresowej?")]), KnowledgeBaseChatClient.FragmentCount);
        Assert.Equal([fragments[1].Chunk.Title, fragments[3].Chunk.Title], citations.Select(c => c.Title));
        Assert.All(citations, c => Assert.True(c.Snippet!.Length <= KnowledgeBaseChatClient.SnippetLength + 1));
    }

    [Fact]
    public async Task AddsNoCitationsWhenAnswerCitesNothing()
    {
        var model = new RecordingChatClient("Nie znalazłem tego w bazie wiedzy.");
        var client = new KnowledgeBaseChatClient(model, TestKnowledgeBase.Retriever);

        var updates = await Collect(client, "Ile dni na sprzeciw?");

        Assert.Empty(Citations(updates));
    }

    [Fact]
    public async Task RefusesLevelRequestWithoutCallingModel()
    {
        var model = new RecordingChatClient("Nie powinno się pojawić.");
        var client = new KnowledgeBaseChatClient(model, TestKnowledgeBase.Retriever);

        var updates = await Collect(client, "Jaki poziom dać Annie za współpracę?");

        Assert.Equal(0, model.Calls);
        Assert.Equal(EvaluationLevelGuard.Refusal, string.Concat(updates.Select(u => u.Text)));
    }

    [Fact]
    public async Task MockAnswersFromFirstFragment()
    {
        var client = new KnowledgeBaseChatClient(new MockChatClient(), TestKnowledgeBase.Retriever);

        var updates = await Collect(client, "Ile dni ma pracownik na sprzeciw od oceny okresowej?");

        Assert.Contains("[1]", string.Concat(updates.Select(u => u.Text)));
        Assert.NotEmpty(Citations(updates));
    }

    [Fact]
    public async Task RemovesReasoningFromHistoryBeforeCallingModel()
    {
        var model = new RecordingChatClient("Odpowiedź [1].");
        var client = new KnowledgeBaseChatClient(model, TestKnowledgeBase.Retriever);
        List<ChatMessage> conversation =
        [
            new(ChatRole.User, "Komu przysługuje sprzeciw od oceny okresowej?"),
            new(ChatRole.Assistant, [new TextReasoningContent("rozumowanie"), new TextContent("Ocenianemu [1].")]),
            new(ChatRole.User, "A ile ma na to dni?"),
        ];

        await foreach (var _ in client.GetStreamingResponseAsync(conversation))
        {
        }

        Assert.NotNull(model.LastMessages);
        Assert.DoesNotContain(model.LastMessages.SelectMany(m => m.Contents), c => c is TextReasoningContent);
        Assert.Equal("Ocenianemu [1].", model.LastMessages[1].Text);
        Assert.Contains(conversation[1].Contents, c => c is TextReasoningContent);
    }

    [Fact]
    public void CitedNumbersIgnoresNumbersOutsideFragments()
    {
        Assert.Equal([1, 3], KnowledgePrompt.CitedNumbers("a [3] b [1, 3] c [7] d [0]", 5));
    }

    [Fact]
    public void CitedNumbersAcceptsProvisionAfterNumber()
    {
        Assert.Equal([1, 2, 4, 5], KnowledgePrompt.CitedNumbers("a [1, § 8 ust. 1] b [2, pkt 1] c [4; 5] d [2025 r.]", 5));
    }

    [Fact]
    public void CitedNumbersAcceptsProvisionInParenthesesAfterNumber()
    {
        Assert.Equal([1, 2, 3, 5], KnowledgePrompt.CitedNumbers("a [1, 2, 5 (art. 81 ust. 9)] b [3 (art. 71)]", 5));
    }

    private static async Task<List<ChatResponseUpdate>> Collect(IChatClient client, string question, ChatOptions? options = null)
    {
        var updates = new List<ChatResponseUpdate>();
        await foreach (var update in client.GetStreamingResponseAsync([new ChatMessage(ChatRole.User, question)], options))
            updates.Add(update);
        return updates;
    }

    private static List<CitationAnnotation> Citations(IEnumerable<ChatResponseUpdate> updates) =>
        updates.SelectMany(u => u.Contents)
            .SelectMany(c => c.Annotations ?? [])
            .OfType<CitationAnnotation>()
            .ToList();

    private sealed class RecordingChatClient(string answer) : IChatClient
    {
        public ChatOptions? LastOptions { get; private set; }

        public List<ChatMessage>? LastMessages { get; private set; }

        public int Calls { get; private set; }

        public Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default) =>
            GetStreamingResponseAsync(messages, options, cancellationToken).ToChatResponseAsync(cancellationToken);

        public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
            IEnumerable<ChatMessage> messages,
            ChatOptions? options = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            Calls++;
            LastOptions = options;
            LastMessages = messages.ToList();
            await Task.Yield();
            foreach (var word in answer.Split(' '))
                yield return new ChatResponseUpdate(ChatRole.Assistant, word + " ");
        }

        public object? GetService(Type serviceType, object? serviceKey = null) => null;

        public void Dispose()
        {
        }
    }
}
