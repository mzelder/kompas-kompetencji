namespace KompasKompetencji.Web.Data;

public class JobDescription
{
    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    public required string Purpose { get; set; }

    public required string Tasks { get; set; }

    public required string RequiredCompetencies { get; set; }

    public int? SubordinatesCount { get; set; }

    public IReadOnlyList<string> TaskList => SplitLines(Tasks);

    public IReadOnlyList<string> RequiredCompetencyList => SplitLines(RequiredCompetencies);

    private static string[] SplitLines(string text) =>
        text.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
