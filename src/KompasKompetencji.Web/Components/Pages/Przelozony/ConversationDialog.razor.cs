using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace KompasKompetencji.Web.Components.Pages.Przelozony;

public partial class ConversationDialog
{
    [Inject] private DialogService DialogService { get; set; } = default!;
    [Inject] private EvaluationService EvaluationService { get; set; } = default!;

    [Parameter, EditorRequired] public EvaluationTask Task { get; set; } = default!;

    [Parameter, EditorRequired] public ConversationType Type { get; set; }

    [Parameter, EditorRequired] public int SupervisorId { get; set; }

    [Parameter, EditorRequired] public DateOnly Today { get; set; }

    private DateTime conversationAt;
    private ConversationStatus status;
    private DateTime? currentAt;
    private ConversationStatus? currentStatus;
    private DateOnly deadline;
    private DateTime? latestAllowed;
    private bool overdue;
    private bool busy;
    private string? errorMessage;

    public static async Task<bool> OpenAsync(
        DialogService dialogService, EvaluationTask task, ConversationType type, int supervisorId, DateOnly today)
    {
        var saved = await dialogService.OpenAsync<ConversationDialog>(
            EvaluationText.ConversationDateTitle(type),
            new Dictionary<string, object?>
            {
                [nameof(Task)] = task,
                [nameof(Type)] = type,
                [nameof(SupervisorId)] = supervisorId,
                [nameof(Today)] = today,
            },
            new DialogOptions { Width = "520px" });
        return saved is true;
    }

    private bool IsStarting => Type == ConversationType.Starting;

    private bool dateChanged => currentAt != conversationAt;

    private string DeadlineLabel => IsStarting ? "Rozmowa rozpoczynająca najpóźniej" : "Termin oceny";

    private string OverdueMessage => IsStarting
        ? $"Termin na rozmowę rozpoczynającą minął {EvaluationText.Date(deadline)}. Wyznacz ją jak najszybciej."
        : $"Termin oceny minął {EvaluationText.Date(deadline)}. Wyznacz rozmowę jak najszybciej.";

    private string DateHint =>
        overdue ? "Wybierz najbliższy możliwy termin albo datę rozmowy, która już się odbyła."
        : IsStarting ? $"Rozmowa rozpoczynająca odbywa się w ciągu {StartingConversationRules.DaysFromCycleStart} dni od początku cyklu, czyli do {EvaluationText.Date(deadline)}."
        : $"Najpóźniej w dniu oceny: {EvaluationText.Date(deadline)}.";

    private string? StatusHint =>
        !ConversationRules.CanBeHeld(conversationAt, DateTime.Now) ? "Rozmowę można oznaczyć jako odbytą po jej terminie."
        : !ConversationRules.CanBeScheduled(conversationAt, currentAt, DateTime.Now) ? "Termin w przeszłości: rozmowa zostanie zapisana jako odbyta."
        : null;

    protected override void OnInitialized()
    {
        var now = DateTime.Now;
        currentAt = Task.ConversationAtFor(Type);
        currentStatus = Task.ConversationStatusFor(Type);
        if (IsStarting)
        {
            deadline = Task.StartingConversationDeadline;
            overdue = StartingConversationRules.IsOverdue(Task.CycleStart, now);
            latestAllowed = StartingConversationRules.LatestAllowed(Task.CycleStart, now);
            conversationAt = currentAt ?? StartingConversationRules.Suggest(Task.CycleStart, now);
        }
        else
        {
            deadline = Task.DueDate;
            overdue = ConversationRules.IsOverdue(Task.DueDate, now);
            latestAllowed = ConversationRules.LatestAllowed(Task.DueDate, now);
            conversationAt = currentAt ?? ConversationRules.Suggest(Task.DueDate, now);
        }
        status = currentStatus ?? ConversationStatus.Scheduled;
    }

    private void OnDateChanged(DateTime value)
    {
        conversationAt = value;
        errorMessage = null;
        status = dateChanged
            ? ConversationRules.StatusForNewDate(value, DateTime.Now)
            : currentStatus ?? ConversationStatus.Scheduled;
    }

    private async Task SaveAsync()
    {
        errorMessage = null;
        busy = true;
        var result = await EvaluationService.SaveConversationAsync(SupervisorId, Task.Employee.Id, Type, conversationAt, status, DateTime.Now);
        busy = false;

        if (!result.Succeeded)
        {
            errorMessage = result.Error;
            return;
        }
        DialogService.Close(true);
    }
}
