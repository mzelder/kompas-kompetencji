using KompasKompetencji.Web.Assistant;
using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;

namespace KompasKompetencji.Tests.Assistant;

public class NoteCriterionClassifierTests
{
    private static readonly IReadOnlyList<Criterion> SheetC =
    [
        Criterion.KnowledgeAndDevelopment,
        Criterion.Reliability,
        Criterion.WorkOrganization,
        Criterion.Cooperation,
        Criterion.Communication,
        Criterion.AnalyticalThinking,
    ];

    [Theory]
    [InlineData("KRYTERIUM: Reliability\nWYDZWIEK: Positive", Criterion.Reliability, NoteTone.Positive)]
    [InlineData("kryterium: cooperation\r\nwydźwięk: negative\n", Criterion.Cooperation, NoteTone.Negative)]
    [InlineData("KRYTERIUM: WorkOrganization.\nWYDZWIEK: Neutral.", Criterion.WorkOrganization, NoteTone.Neutral)]
    public void ParseAnswer_ReadsCriterionAndTone(string answer, Criterion expectedCriterion, NoteTone expectedTone)
    {
        Assert.Equal(new NoteClassification(expectedCriterion, expectedTone), NoteCriterionClassifier.ParseAnswer(answer, SheetC));
    }

    [Fact]
    public void ParseAnswer_NoCriterionGivesEmptyCriterion()
    {
        Assert.Equal(new NoteClassification(null, NoteTone.Positive),
            NoteCriterionClassifier.ParseAnswer("KRYTERIUM: BRAK\nWYDZWIEK: Positive", SheetC));
    }

    [Theory]
    [InlineData("KRYTERIUM: PublicSpeaking\nWYDZWIEK: Positive")]
    [InlineData("KRYTERIUM: Reliability")]
    [InlineData("KRYTERIUM: Reliability\nWYDZWIEK: dobry")]
    [InlineData("")]
    [InlineData("Nie wiem")]
    public void ParseAnswer_RejectsUnknownAnswer(string answer)
    {
        Assert.Null(NoteCriterionClassifier.ParseAnswer(answer, SheetC));
    }

    [Theory]
    [InlineData("Samodzielnie wykrył błąd w obliczeniach raportu i zaproponował dodatkowy krok kontroli.", Criterion.Reliability)]
    [InlineData("Poprowadziła dla wydziału warsztat z weryfikacji źródeł i podzieliła się wiedzą.", Criterion.KnowledgeAndDevelopment)]
    [InlineData("Oddał zestawienie tydzień przed terminem, zgodnie z planem.", Criterion.WorkOrganization)]
    [InlineData("Sprawnie prowadził spotkania robocze z innymi komórkami urzędu.", Criterion.Cooperation)]
    [InlineData("Jasno przekazuje informacje kierownictwu.", Criterion.Communication)]
    public void MatchByKeywords_PicksMostMatchingCriterion(string note, Criterion expected)
    {
        Assert.Equal(expected, NoteCriterionClassifier.MatchByKeywords(note, SheetC));
    }

    [Fact]
    public void MatchByKeywords_WithoutMatchGivesEmptyCriterion()
    {
        Assert.Null(NoteCriterionClassifier.MatchByKeywords("Bez szczegółów.", SheetC));
    }

    [Theory]
    [InlineData("Oddała zestawienie przed terminem, bez poprawek.", NoteTone.Positive)]
    [InlineData("Dwukrotnie przekazał zestawienie po terminie, bez uprzedzenia.", NoteTone.Negative)]
    [InlineData("Uczestniczył w spotkaniu wydziału.", NoteTone.Neutral)]
    public void ToneByKeywords_ReadsTone(string note, NoteTone expected)
    {
        Assert.Equal(expected, NoteCriterionClassifier.ToneByKeywords(note));
    }
}
