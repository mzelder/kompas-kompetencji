using KompasKompetencji.Web.Auth;
using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using KompasKompetencji.Web.Organization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace KompasKompetencji.Web.Components.Pages.Przelozony;

public partial class MyEmployees
{
    [Inject] private OrganizationService Organization { get; set; } = default!;
    [Inject] private UserService Users { get; set; } = default!;
    [Inject] private EvaluationService Evaluations { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private IReadOnlyList<Employee> employees = [];
    private IReadOnlyDictionary<int, EvaluationStage> stages = new Dictionary<int, EvaluationStage>();
    private bool loading = true;
    private bool accountNotLinked;
    private string? loadError;

    [CascadingParameter] private Task<AuthenticationState> AuthState { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var userId = (await AuthState).User.GetUserId();
            var user = userId is null ? null : await Users.GetByIdAsync(userId.Value);
            if (user?.EmployeeId is int supervisorId)
            {
                employees = (await Organization.GetChartAsync()).SubordinatesOf(supervisorId);
                stages = await Evaluations.GetCurrentStagesAsync(employees.Select(e => e.Id).ToList());
            }
            else
            {
                accountNotLinked = true;
            }
        }
        catch (Exception ex)
        {
            loadError = ex.Message;
        }
        loading = false;
    }

    private void OpenEmployee(Employee employee) =>
        Navigation.NavigateTo(EmployeeEvaluation.PathFor(employee.Id));
}
