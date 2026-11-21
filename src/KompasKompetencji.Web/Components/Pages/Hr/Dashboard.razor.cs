namespace KompasKompetencji.Web.Components.Pages.Hr;

public partial class Dashboard
{
    private readonly List<EvaluationProcessRow> processes = [];

    private sealed record EvaluationProcessRow(string Employee, string Supervisor, string Sheet, string Status, DateTime Deadline);
}
