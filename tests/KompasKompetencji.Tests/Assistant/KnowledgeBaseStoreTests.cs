using KompasKompetencji.Web.Assistant.KnowledgeBase;

namespace KompasKompetencji.Tests.Assistant;

public class KnowledgeBaseStoreTests
{
    [Fact]
    public void EveryChunkHasIdTitleAndText()
    {
        Assert.NotEmpty(TestKnowledgeBase.Store.Chunks);
        Assert.All(TestKnowledgeBase.Store.Chunks, chunk =>
        {
            Assert.False(string.IsNullOrWhiteSpace(chunk.Id));
            Assert.False(string.IsNullOrWhiteSpace(chunk.Title));
            Assert.False(string.IsNullOrWhiteSpace(chunk.Text));
        });
    }

    [Fact]
    public void ChunkIdsAreUnique()
    {
        var duplicates = TestKnowledgeBase.Store.Chunks
            .GroupBy(c => c.Id)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        Assert.Empty(duplicates);
    }

    [Fact]
    public void MissingFileGivesReadableError()
    {
        var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.json");

        var error = Assert.Throws<FileNotFoundException>(() => KnowledgeBaseStore.Load(path));

        Assert.Contains("build_knowledge_base.py", error.Message);
    }
}
