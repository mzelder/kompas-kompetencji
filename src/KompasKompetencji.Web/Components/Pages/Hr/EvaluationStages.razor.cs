using KompasKompetencji.Web.Evaluations;
using KompasKompetencji.Web.Organization;
using Microsoft.AspNetCore.Components;

namespace KompasKompetencji.Web.Components.Pages.Hr;

public partial class EvaluationStages
{
    [Inject] private OrganizationService Organization { get; set; } = default!;
    [Inject] private EvaluationService Evaluations { get; set; } = default!;

    private readonly DateOnly today = DateOnly.FromDateTime(DateTime.Today);
    private IReadOnlyList<EmployeeEvaluationRow> rows = [];
    private bool loading = true;
    private string? loadError;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var employees = (await Organization.GetChartAsync()).Employees;
            rows = await Evaluations.GetEvaluationRowsAsync(employees, today);
        }
        catch (Exception ex)
        {
            loadError = ex.Message;
        }
        loading = false;
    }
}
