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
    [Inject] private DialogService DialogService { get; set; } = default!;
    [Inject] private NotificationService NotificationService { get; set; } = default!;

    private IReadOnlyList<Employee> employees = [];
    private IReadOnlyDictionary<int, EvaluationStage> stages = new Dictionary<int, EvaluationStage>();
    private IReadOnlyDictionary<int, EvaluationTask> tasks = new Dictionary<int, EvaluationTask>();
    private readonly DateOnly today = DateOnly.FromDateTime(DateTime.Today);
    private int supervisorId;
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
            if (user?.EmployeeId is int employeeId)
            {
                supervisorId = employeeId;
                employees = (await Organization.GetChartAsync()).SubordinatesOf(supervisorId);
                await LoadEvaluationsAsync();
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

    private async Task LoadEvaluationsAsync()
    {
        stages = await Evaluations.GetCurrentStagesAsync(employees.Select(e => e.Id).ToList());
        tasks = (await Evaluations.GetSupervisorOverviewAsync(supervisorId, DateTime.Now)).Tasks
            .ToDictionary(t => t.Employee.Id);
    }

    private EvaluationTask? AssessmentTaskFor(Employee employee) =>
        stages.GetValueOrDefault(employee.Id) == EvaluationStage.Assessment ? tasks.GetValueOrDefault(employee.Id) : null;

    private async Task ScheduleConversationAsync(EvaluationTask task)
    {
        if (await ConversationDialog.OpenAsync(DialogService, task, supervisorId, today))
        {
            await LoadEvaluationsAsync();
            NotificationService.Notify(NotificationSeverity.Success, "Zapisano termin rozmowy", task.Employee.FullName);
        }
    }

    private void OpenEmployee(Employee employee) =>
        Navigation.NavigateTo(EmployeeEvaluation.PathFor(employee.Id));
}
