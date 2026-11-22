using KompasKompetencji.Web.Assistant;
using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;

namespace KompasKompetencji.Tests.Assistant;

public class WrittenEvaluationTests
{
    [Fact]
    public void CompetencyModel_HasAllCompetenciesAndIndicators()
    {
        Assert.Equal(49, CompetencyModelSource.Indicators.Count);
        Assert.Equal(49 * 5, CompetencyModelSource.Behaviors.Count);
        Assert.Equal(EvaluationCriteria.All.Count, CompetencyModelSource.Indicators.Select(i => i.Criterion).Distinct().Count());
    }

    [Fact]
    public void Suggest_WithoutRatingsGivesNoLevel()
    {
        var suggestion = CriterionLevelRule.Suggest([null, null]);
        Assert.Null(suggestion.Level);
        Assert.Equal(0, suggestion.Rated);
    }

    [Fact]
    public void Suggest_RoundsAverageAndSkipsNoBasis()
    {
        var suggestion = CriterionLevelRule.Suggest([3, 4, 4, null]);
        Assert.Equal(4, suggestion.Level);
        Assert.Equal(3, suggestion.Rated);
    }

    [Theory]
    [InlineData(new[] { 3, 4 }, 4)]
    [InlineData(new[] { 2, 3 }, 3)]
    [InlineData(new[] { 1, 2, 2, 2 }, 2)]
    public void Suggest_RoundsHalfUp(int[] levels, int expected)
    {
        Assert.Equal(expected, CriterionLevelRule.Suggest(levels.Select(l => (int?)l).ToList()).Level);
    }

    [Fact]
    public void Suggest_WarnsWhenLessThanHalfRated()
    {
        Assert.True(CriterionLevelRule.Suggest([3, null, null]).HasTooFewBasis);
        Assert.False(CriterionLevelRule.Suggest([3, 4, null]).HasTooFewBasis);
    }

    [Theory]
    [InlineData(new[] { 3, 3, 3, 3, 3 }, 5.0, "Na poziomie oczekiwań", true)]
    [InlineData(new[] { 4, 4, 3, 3, 1 }, 5.0, "Na poziomie oczekiwań", false)]
    [InlineData(new[] { 2, 2, 3, 2, 2 }, 3.4, "Poniżej oczekiwań", false)]
    public void Summary_UsesSheetThresholds(int[] levels, double average, string overall, bool positive)
    {
        var summary = WrittenEvaluationSummary.From(levels);
        Assert.Equal(average, summary.AveragePoints, 3);
        Assert.Equal(overall, summary.OverallLevel);
        Assert.Equal(positive, summary.IsPositive);
    }

    [Fact]
    public void ParseSuggestions_ReadsLevelsAndNoBasis()
    {
        var indicators = new[]
        {
            new CompetencyIndicator { Id = 10, Number = 1, Text = "A" },
            new CompetencyIndicator { Id = 11, Number = 2, Text = "B" },
        };
        var items = IndicatorSuggester.Parse("W1: 2 | Błędy w zestawieniu.\nW2: BRAK | Brak notatek.\nW9: 4 | spoza listy", indicators);

        Assert.Equal(2, items.Count);
        Assert.Equal(new IndicatorSuggestion(10, 2, "Błędy w zestawieniu."), items[0]);
        Assert.Equal(new IndicatorSuggestion(11, null, "Brak notatek."), items[1]);
    }
}
