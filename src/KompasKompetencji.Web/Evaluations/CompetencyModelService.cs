using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Text;
using Microsoft.EntityFrameworkCore;

namespace KompasKompetencji.Web.Evaluations;

public sealed record CompetencyView(Criterion Criterion, IReadOnlyList<CompetencyIndicator> Indicators)
{
    public string Name => EvaluationCriteria.Name(Criterion);

    public bool IsMandatoryIn(EvaluationSheet sheet) => EvaluationCriteria.Mandatory(sheet).Contains(Criterion);

    public bool Matches(string normalizedTerm) =>
        TextNormalizer.Normalize(Name).Contains(normalizedTerm)
        || Indicators.Any(i => TextNormalizer.Normalize(i.Text).Contains(normalizedTerm));
}

public sealed class CompetencyModelService(IDbContextFactory<AppDbContext> dbFactory)
{
    public async Task<IReadOnlyList<CompetencyView>> GetAsync(CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var indicators = await db.CompetencyIndicators.AsNoTracking()
            .Include(i => i.Behaviors)
            .OrderBy(i => i.Id)
            .ToListAsync(ct);

        return indicators
            .GroupBy(i => i.Criterion)
            .OrderBy(g => g.Min(i => i.Id))
            .Select(g => new CompetencyView(g.Key, g.OrderBy(i => i.Number).ToList()))
            .ToList();
    }
}
