namespace KompasKompetencji.Web.Evaluations;

public static class CompetencyLevels
{
    public const int Min = 1;
    public const int Max = 5;

    public static IReadOnlyList<int> All { get; } = [1, 2, 3, 4, 5];

    public static string Name(int level) => level switch
    {
        1 => "Znacznie poniżej oczekiwań",
        2 => "Poniżej oczekiwań",
        3 => "Na poziomie oczekiwań",
        4 => "Powyżej oczekiwań",
        5 => "Znacznie powyżej oczekiwań",
        _ => throw new ArgumentOutOfRangeException(nameof(level)),
    };

    public static int Points(int level) => level * 2 - 1;

    public static string NameWithPoints(int level) => $"{Name(level)} ({Points(level)} pkt)";

    public static bool IsValid(int level) => level is >= Min and <= Max;
}

public sealed record LevelSuggestion(int? Level, double? Average, int Rated, int Total)
{
    public bool HasTooFewBasis => Total > 0 && Rated * 2 < Total;
}

public static class CriterionLevelRule
{
    public static LevelSuggestion Suggest(IReadOnlyCollection<int?> answers)
    {
        var rated = answers.OfType<int>().ToList();
        if (rated.Count == 0)
        {
            return new LevelSuggestion(null, null, 0, answers.Count);
        }

        var average = rated.Average();
        return new LevelSuggestion((int)Math.Round(average, MidpointRounding.AwayFromZero), average, rated.Count, answers.Count);
    }
}

public sealed record WrittenEvaluationSummary(double AveragePoints, string OverallLevel, bool IsPositive)
{
    public static WrittenEvaluationSummary From(IReadOnlyCollection<int> levels)
    {
        var points = levels.Select(CompetencyLevels.Points).ToList();
        var average = points.Average();
        var overall = average switch
        {
            <= 2 => CompetencyLevels.Name(1),
            <= 4 => CompetencyLevels.Name(2),
            <= 6 => CompetencyLevels.Name(3),
            <= 8 => CompetencyLevels.Name(4),
            _ => CompetencyLevels.Name(5),
        };
        return new WrittenEvaluationSummary(average, overall, average > 4 && !points.Contains(1));
    }
}
