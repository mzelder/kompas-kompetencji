using KompasKompetencji.Web.Evaluations;

namespace KompasKompetencji.Web.Components.Pages.Przelozony;

public partial class Guidelines
{
    private sealed record ScaleRow(string Level, int Points);

    private static readonly IReadOnlyList<ScaleRow> ScaleRows =
        CompetencyLevels.All
            .OrderByDescending(level => level)
            .Select(level => new ScaleRow(CompetencyLevels.Name(level), CompetencyLevels.Points(level)))
            .ToList();
}
