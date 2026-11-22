namespace KompasKompetencji.Web.Data;

public class JobPosition
{
    public int Id { get; set; }

    public required string Symbol { get; set; }

    public required string Title { get; set; }

    public required string Department { get; set; }

    public string? Unit { get; set; }

    public string? DirectSupervisorTitle { get; set; }

    public string? IndirectSupervisorTitle { get; set; }

    public required string Purpose { get; set; }

    public required string Tasks { get; set; }

    public required string RequiredCompetencies { get; set; }

    public SubordinatesRange Subordinates { get; set; }

    public ICollection<Employee> Employees { get; set; } = [];

    public string OrganizationalUnit => Unit ?? Department;

    public bool ManagesTeam => Subordinates != SubordinatesRange.None;

    public IReadOnlyList<string> TaskList => SplitLines(Tasks);

    public IReadOnlyList<string> RequiredCompetencyList => SplitLines(RequiredCompetencies);

    private static string[] SplitLines(string text) =>
        text.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}

public enum SubordinatesRange
{
    None,
    UpTo4,
    From5To9,
    Over9,
    Managers,
}
