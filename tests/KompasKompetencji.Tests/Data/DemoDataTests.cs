using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KompasKompetencji.Tests.Data;

public class DemoDataTests
{
    [Fact]
    public void DemoCriterionLevelsMatchIndicatorRatings()
    {
        using var db = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=(localdb)\\unused;Database=unused")
            .Options);

        var model = db.GetService<IDesignTimeModel>().Model;
        var indicators = CompetencyModelSource.Indicators.ToDictionary(i => i.Id);
        var ratings = model.FindEntityType(typeof(IndicatorRating))!.GetSeedData()
            .Select(r => (EvaluationId: (int)r[nameof(IndicatorRating.EvaluationId)]!, IndicatorId: (int)r[nameof(IndicatorRating.IndicatorId)]!, Level: (int?)r[nameof(IndicatorRating.Level)]))
            .ToList();
        var decisions = model.FindEntityType(typeof(CriterionDecision))!.GetSeedData()
            .ToDictionary(
                d => ((int)d[nameof(CriterionDecision.EvaluationId)]!, (Criterion)d[nameof(CriterionDecision.Criterion)]!),
                d => (int)d[nameof(CriterionDecision.Level)]!);

        Assert.NotEmpty(ratings);
        foreach (var group in ratings.GroupBy(r => (r.EvaluationId, indicators[r.IndicatorId].Criterion)))
        {
            var expected = CriterionLevelRule.Suggest(group.Select(r => r.Level).ToList()).Level;
            Assert.True(decisions.TryGetValue(group.Key, out var level), $"Brak decyzji dla oceny {group.Key.EvaluationId}, kryterium {group.Key.Criterion}.");
            Assert.Equal(expected, level);
        }
    }
}
