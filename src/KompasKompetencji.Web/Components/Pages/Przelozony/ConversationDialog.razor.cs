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

    [Parameter, EditorRequired] public int SupervisorId { get; set; }

    [Parameter, EditorRequired] public DateOnly Today { get; set; }

    private DateTime conversationAt;
    private ConversationStatus status;
    private DateTime? latestAllowed;
    private bool overdue;
    private bool busy;
    private string? errorMessage;

    private bool dateChanged => Task.ConversationAt != conversationAt;

    private string? StatusHint =>
        !ConversationRules.CanBeHeld(conversationAt, DateTime.Now) ? "Rozmowę można oznaczyć jako odbytą po jej terminie."
        : !ConversationRules.CanBeScheduled(conversationAt, Task.ConversationAt, DateTime.Now) ? "Termin w przeszłości: rozmowa zostanie zapisana jako odbyta."
        : null;

    protected override void OnInitialized()
    {
        var now = DateTime.Now;
        overdue = ConversationRules.IsOverdue(Task.DueDate, now);
        latestAllowed = ConversationRules.LatestAllowed(Task.DueDate, now);
        conversationAt = Task.ConversationAt ?? ConversationRules.Suggest(Task.DueDate, now);
        status = Task.ConversationStatus ?? ConversationStatus.Scheduled;
    }

    private void OnDateChanged(DateTime value)
    {
        conversationAt = value;
        errorMessage = null;
        status = dateChanged
            ? ConversationRules.StatusForNewDate(value, DateTime.Now)
            : Task.ConversationStatus ?? ConversationStatus.Scheduled;
    }

    private async Task SaveAsync()
    {
        errorMessage = null;
        busy = true;
        var result = await EvaluationService.SaveConversationAsync(SupervisorId, Task.Employee.Id, conversationAt, status, DateTime.Now);
        busy = false;

        if (!result.Succeeded)
        {
            errorMessage = result.Error;
            return;
        }
        DialogService.Close(true);
    }
}
