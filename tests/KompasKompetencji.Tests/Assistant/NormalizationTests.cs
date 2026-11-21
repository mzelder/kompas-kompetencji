using KompasKompetencji.Web.Assistant.KnowledgeBase;
using KompasKompetencji.Web.Text;

namespace KompasKompetencji.Tests.Assistant;

public class NormalizationTests
{
    [Theory]
    [InlineData("Sprzeciw")]
    [InlineData("SPRZECIW")]
    [InlineData("sprzeciw")]
    public void LetterCaseDoesNotChangeResults(string word)
    {
        var expected = Ids("sprzeciw od oceny");

        Assert.Equal(expected, Ids($"{word} od oceny"));
    }

    [Fact]
    public void PolishLettersDoNotChangeResults()
    {
        Assert.Equal(Ids("Ocena okresowa – rozmowa oceniająca, służba cywilna"), Ids("Ocena okresowa - rozmowa oceniajaca, sluzba cywilna"));
    }

    [Fact]
    public void NormalizerRemovesPolishLettersAndCase()
    {
        Assert.Equal("zazolc gesla jazn", TextNormalizer.Normalize("ZAŻÓŁĆ gęślą jaźń"));
    }

    [Fact]
    public void TokenizerSkipsStopWords()
    {
        Assert.Equal(["dni", "sprzeci"], SearchTokenizer.Tokenize("Ile dni na sprzeciw?"));
    }

    [Theory]
    [InlineData("sprzeciw", "sprzeciwu")]
    [InlineData("oceniająca", "oceniającej")]
    [InlineData("okresowa", "okresowej")]
    public void InflectedFormsShareStem(string word, string inflected)
    {
        Assert.Equal(SearchTokenizer.Tokenize(word), SearchTokenizer.Tokenize(inflected));
    }

    private static List<string> Ids(string query) =>
        TestKnowledgeBase.Retriever.Search(query).Select(r => r.Chunk.Id).ToList();
}
