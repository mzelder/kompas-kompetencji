using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace KompasKompetencji.Web.Components.Pages.Przelozony.Stages;

public partial class AssessmentStage
{
    [Inject] private EvaluationService EvaluationService { get; set; } = default!;
    [Inject] private NotificationService NotificationService { get; set; } = default!;
    [Inject] private DialogService DialogService { get; set; } = default!;

    [Parameter, EditorRequired] public EmployeeEvaluationView View { get; set; } = default!;

    [Parameter, EditorRequired] public int SupervisorId { get; set; }

    [Parameter] public bool ReadOnly { get; set; }

    [Parameter] public EventCallback OnChanged { get; set; }

    private string text = string.Empty;
    private bool busy;
    private string? errorMessage;

    private string ConversationHint => View.Task switch
    {
        { ConversationAt: DateTime conversationAt } => $"Wyznaczona na {EvaluationText.Date(conversationAt)}.",
        { CanScheduleConversation: false } task =>
            $"Termin wyznaczysz od {EvaluationText.Date(task.ConversationWindowOpensOn)}, czyli 2 miesiące przed terminem oceny ({EvaluationText.Date(task.DueDate)}).",
        { Urgency: EvaluationUrgency.Overdue } => "Termin oceny minął. Wyznacz rozmowę jak najszybciej.",
        var task => $"Wyznacz termin rozmowy, najpóźniej {EvaluationText.Date(task.DueDate)}.",
    };

    private async Task ScheduleConversationAsync()
    {
        if (await ConversationDialog.OpenAsync(DialogService, View.Task, ConversationType.Assessment, SupervisorId, DateOnly.FromDateTime(DateTime.Today)))
        {
            NotificationService.Notify(NotificationSeverity.Success, "Zapisano termin rozmowy", View.Employee.FullName);
            await OnChanged.InvokeAsync();
        }
    }

    private async Task AddAsync()
    {
        errorMessage = null;
        busy = true;
        var result = await EvaluationService.AddNoteAsync(SupervisorId, View.Employee.Id, text, DateTime.Now);
        busy = false;

        if (!result.Succeeded)
        {
            errorMessage = result.Error;
            return;
        }

        text = string.Empty;
        NotificationService.Notify(NotificationSeverity.Success, "Dodano notatkę", View.Employee.FullName);
        await OnChanged.InvokeAsync();
    }

    private async Task DeleteAsync(EvaluationNote note)
    {
        var confirmed = await DialogService.Confirm(
            "Notatki nie będzie można przywrócić.",
            "Usunąć notatkę?",
            new ConfirmOptions { OkButtonText = "Usuń", CancelButtonText = "Anuluj" });
        if (confirmed is not true)
        {
            return;
        }

        var result = await EvaluationService.DeleteNoteAsync(SupervisorId, View.Employee.Id, note.Id);
        if (!result.Succeeded)
        {
            NotificationService.Notify(NotificationSeverity.Error, "Nie udało się usunąć notatki", result.Error ?? string.Empty);
            return;
        }

        NotificationService.Notify(NotificationSeverity.Success, "Usunięto notatkę", View.Employee.FullName);
        await OnChanged.InvokeAsync();
    }
}
