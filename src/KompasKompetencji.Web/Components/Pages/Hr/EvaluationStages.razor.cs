using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using KompasKompetencji.Web.Organization;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace KompasKompetencji.Web.Components.Pages.Hr;

public partial class EvaluationStages
{
    [Inject] private OrganizationService Organization { get; set; } = default!;
    [Inject] private EvaluationService Evaluations { get; set; } = default!;

    private IReadOnlyList<Employee> employees = [];
    private IReadOnlyDictionary<int, EvaluationStage> stages = new Dictionary<int, EvaluationStage>();
    private bool loading = true;
    private string? loadError;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            employees = (await Organization.GetChartAsync()).Employees;
            stages = await Evaluations.GetCurrentStagesAsync(employees.Select(e => e.Id).ToList());
        }
        catch (Exception ex)
        {
            loadError = ex.Message;
        }
        loading = false;
    }
}
