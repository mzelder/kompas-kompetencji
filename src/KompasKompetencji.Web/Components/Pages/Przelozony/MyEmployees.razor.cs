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

    private sealed record NextStepAction(string Text, string Icon, ButtonStyle Style, Variant Variant, bool Enabled);

    private sealed record NextStepView(string Title, string Detail, bool IsWarning, NextStepAction? Action);

    private IReadOnlyList<Employee> employees = [];
    private IReadOnlyList<EmployeeEvaluationRow> rows = [];
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
                await LoadRowsAsync();
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

    private async Task LoadRowsAsync()
    {
        rows = await Evaluations.GetEvaluationRowsAsync(employees, today);
    }

    private static NextStepView? NextStepFor(EmployeeEvaluationRow row) => row.Stage switch
    {
        EvaluationStage.JobDescriptionReview => new(
            "Opis stanowiska i arkusz", "Przejrzyj opis stanowiska i zatwierdź arkusz oceny", false,
            new("Zatwierdź arkusz", "assignment", ButtonStyle.Primary, Variant.Filled, true)),
        EvaluationStage.StartingConversation => new(
            "Rozmowa rozpoczynająca", "Ustal z pracownikiem kryteria oceny", false, null),
        EvaluationStage.Assessment => AssessmentConversationStep(row.Task),
        EvaluationStage.Completion => row.Task.PreparedOn is DateOnly preparedOn
            ? new("Zakończenie", $"Zapoznaj pracownika z oceną, sporządzona {EvaluationText.Date(preparedOn)}", false, null)
            : new("Zakończenie", "Wpisz datę sporządzenia oceny", false, null),
        _ => null,
    };

    private static NextStepView AssessmentConversationStep(EvaluationTask task)
    {
        const string title = "Rozmowa oceniająca";
        var changeDate = new NextStepAction("Zmień termin", "edit_calendar", ButtonStyle.Base, Variant.Text, true);
        var schedule = new NextStepAction("Wyznacz termin", "edit_calendar", ButtonStyle.Primary, Variant.Filled, task.CanScheduleConversation);

        return task switch
        {
            { ConversationAt: DateTime at, ConversationStatus: ConversationStatus.Held } =>
                new(title, $"Odbyta {EvaluationText.Date(at)}", false, null),
            { ConversationAt: DateTime at } when ConversationRules.IsPastDue(at, task.ConversationStatus, DateTime.Now) =>
                new(title, $"{EvaluationText.Date(at)}: termin minął", true, changeDate),
            { ConversationAt: DateTime at } =>
                new(title, EvaluationText.Date(at), false, changeDate),
            { CanScheduleConversation: false } =>
                new(title, $"Możliwa od {EvaluationText.Date(task.ConversationWindowOpensOn)}", false, schedule),
            { Urgency: EvaluationUrgency.Overdue } =>
                new(title, "Termin oceny minął, wyznacz jak najszybciej", true, schedule),
            _ =>
                new(title, $"Najpóźniej {EvaluationText.Date(task.DueDate)}", false, schedule),
        };
    }

    private async Task RunActionAsync(EmployeeEvaluationRow row)
    {
        if (row.Stage != EvaluationStage.Assessment)
        {
            OpenEmployee(row.Employee);
            return;
        }

        if (await ConversationDialog.OpenAsync(DialogService, row.Task, ConversationType.Assessment, supervisorId, today))
        {
            await LoadRowsAsync();
            NotificationService.Notify(NotificationSeverity.Success, "Zapisano termin rozmowy", row.Employee.FullName);
        }
    }

    private void OpenEmployee(Employee employee) =>
        Navigation.NavigateTo(EmployeeEvaluation.PathFor(employee.Id));
}
