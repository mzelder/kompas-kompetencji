using System.Text.Json;
using KompasKompetencji.Web.Assistant.KnowledgeBase;
using Microsoft.Extensions.AI;
using Xunit.Abstractions;

namespace KompasKompetencji.Tests.Assistant;

public class RetrievalTests(ITestOutputHelper output)
{
    private const int ResultCount = 5;
    private const double MinimumRecall = 0.9;

    public static TheoryData<string, string[]> Questions()
    {
        var data = new TheoryData<string, string[]>();
        foreach (var item in LoadQuestions())
            data.Add(item.Question, item.ExpectedChunkIds);
        return data;
    }

    [Theory]
    [MemberData(nameof(Questions))]
    public void ExpectedChunkIsInTopResults(string question, string[] expectedChunkIds)
    {
        var found = TestKnowledgeBase.Retriever.Search(question, ResultCount).Select(r => r.Chunk.Id).ToList();

        Assert.True(
            IsHit(found, expectedChunkIds),
            $"Expected one of [{string.Join(", ", expectedChunkIds)}], got [{string.Join(", ", found)}]");
    }

    [Fact]
    public void RecallAtFiveIsHighEnough()
    {
        var questions = LoadQuestions();
        var hits = 0;
        foreach (var item in questions)
        {
            var found = TestKnowledgeBase.Retriever.Search(item.Question, ResultCount).Select(r => r.Chunk.Id).ToList();
            var hit = IsHit(found, item.ExpectedChunkIds);
            hits += hit ? 1 : 0;
            output.WriteLine($"{(hit ? "OK  " : "MISS")} {item.Question} -> {string.Join(", ", found)}");
        }

        var recall = (double)hits / questions.Count;
        output.WriteLine($"recall@{ResultCount} = {recall:P0} ({hits}/{questions.Count})");
        Assert.True(recall >= MinimumRecall, $"recall@{ResultCount} = {recall:P0}");
    }

    [Fact]
    public void FollowUpQuestionUsesPreviousQuestion()
    {
        List<ChatMessage> conversation =
        [
            new(ChatRole.User, "Komu przysługuje sprzeciw od oceny okresowej?"),
            new(ChatRole.Assistant, "Ocenianemu, do dyrektora generalnego urzędu."),
            new(ChatRole.User, "A ile ma na to dni?"),
        ];

        var query = SearchQuery.FromConversation(conversation);
        var found = TestKnowledgeBase.Retriever.Search(query, ResultCount).Select(r => r.Chunk.Id);

        Assert.Contains("ustawa-art-83", found);
    }

    [Fact]
    public void QueryUsesOnlyLastTwoUserMessages()
    {
        List<ChatMessage> conversation =
        [
            new(ChatRole.User, "pierwsze"),
            new(ChatRole.Assistant, "odpowiedź"),
            new(ChatRole.User, "drugie"),
            new(ChatRole.User, "trzecie"),
        ];

        Assert.Equal("drugie\ntrzecie", SearchQuery.FromConversation(conversation));
    }

    [Fact]
    public void QueryWithoutKnownWordsReturnsNothing()
    {
        Assert.Empty(TestKnowledgeBase.Retriever.Search("xyzzy qwerty"));
    }

    private static bool IsHit(IEnumerable<string> foundIds, IEnumerable<string> expectedIds) =>
        foundIds.Any(found => expectedIds.Any(expected => found == expected || found.StartsWith($"{expected}-cz", StringComparison.Ordinal)));

    private static List<RetrievalQuestion> LoadQuestions()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "retrieval-questions.json");
        return JsonSerializer.Deserialize<List<RetrievalQuestion>>(
            File.ReadAllText(path),
            new JsonSerializerOptions(JsonSerializerDefaults.Web)) ?? [];
    }

    private sealed record RetrievalQuestion(string Question, string[] ExpectedChunkIds);
}
