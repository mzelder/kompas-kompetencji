using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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

    private bool markingHeld;

    private bool IsConversationHeld => View.Task.ConversationStatus == ConversationStatus.Held;

    private bool IsConversationPastDue => ConversationRules.IsPastDue(View.Task.ConversationAt, View.Task.ConversationStatus, DateTime.Now);

    private string ConversationHint => View.Task switch
    {
        { ConversationStatus: ConversationStatus.Held, ConversationAt: DateTime heldAt } =>
            $"Rozmowa odbyła się {EvaluationText.Date(heldAt)}. Możesz uzupełnić ocenę kryteriów poniżej.",
        { ConversationAt: DateTime conversationAt } when IsConversationPastDue =>
            $"Termin rozmowy minął {EvaluationText.Date(conversationAt)}. Oznacz ją jako odbytą albo zmień termin.",
        { ConversationAt: DateTime conversationAt } => $"Wyznaczona na {EvaluationText.Date(conversationAt)}.",
        { CanScheduleConversation: false } task =>
            $"Termin wyznaczysz od {EvaluationText.Date(task.ConversationWindowOpensOn)}, czyli 2 miesiące przed terminem oceny ({EvaluationText.Date(task.DueDate)}).",
        { Urgency: EvaluationUrgency.Overdue } => "Termin oceny minął. Wyznacz rozmowę jak najszybciej.",
        var task => $"Wyznacz termin rozmowy, najpóźniej {EvaluationText.Date(task.DueDate)}.",
    };

    private async Task MarkConversationHeldAsync()
    {
        if (View.Task.ConversationAt is not DateTime conversationAt)
        {
            return;
        }

        markingHeld = true;
        var result = await EvaluationService.SaveConversationAsync(
            SupervisorId, View.Employee.Id, ConversationType.Assessment, conversationAt, ConversationStatus.Held, DateTime.Now);
        markingHeld = false;
        if (!result.Succeeded)
        {
            NotificationService.Notify(NotificationSeverity.Error, "Nie udało się zapisać", result.Error ?? string.Empty);
            return;
        }

        NotificationService.Notify(NotificationSeverity.Success, "Rozmowa oznaczona jako odbyta", View.Employee.FullName);
        await OnChanged.InvokeAsync();
    }

    private async Task ScheduleConversationAsync()
    {
        if (await ConversationDialog.OpenAsync(DialogService, View.Task, ConversationType.Assessment, SupervisorId, DateOnly.FromDateTime(DateTime.Today)))
        {
            NotificationService.Notify(NotificationSeverity.Success, "Zapisano termin rozmowy", View.Employee.FullName);
            await OnChanged.InvokeAsync();
        }
    }

    private sealed record FilterOption(string Value, string Text);

    private const string WithoutCriterion = "none";

    private string? filter;
    private bool collapsed;

    private void ToggleCollapsed() => collapsed = !collapsed;

    private void OnHeadingKeyDown(KeyboardEventArgs args)
    {
        if (args.Key is "Enter" or " ")
        {
            ToggleCollapsed();
        }
    }

    private static string ClassificationSource(EvaluationNote note) =>
        note.ClassificationEditedAt is null ? "ocena asystenta AI" : "ustawione ręcznie";

    private static string EditedDescription(EvaluationNote note) => string.Join(". ", new[]
    {
        note.TextEditedAt is DateTime textEditedAt ? $"Treść zmieniona {EvaluationText.Date(textEditedAt)}" : null,
        note.ClassificationEditedAt is DateTime classificationEditedAt
            ? $"Kryterium lub wydźwięk zmienione ręcznie {EvaluationText.Date(classificationEditedAt)}"
            : null,
    }.OfType<string>());

    private static string ToneIcon(NoteTone tone) => tone switch
    {
        NoteTone.Positive => "thumb_up",
        NoteTone.Negative => "thumb_down",
        _ => "remove",
    };

    private static string ToneColor(NoteTone tone) => tone switch
    {
        NoteTone.Positive => Colors.Success,
        NoteTone.Negative => Colors.Danger,
        _ => Colors.Base400,
    };

    private static string ToneClass(NoteTone tone) => $"note-tone-{tone.ToString().ToLowerInvariant()}";

    private IReadOnlyList<FilterOption> FilterOptions =>
    [
        .. View.Criteria.Select(c => new FilterOption(c.ToString(), $"{EvaluationCriteria.Name(c)} ({View.Notes.Count(n => n.Criterion == c)})")),
        new FilterOption(WithoutCriterion, $"Bez kryterium ({View.Notes.Count(n => n.Criterion is null)})"),
    ];

    private IReadOnlyList<EvaluationNote> VisibleNotes => filter switch
    {
        null => View.Notes,
        WithoutCriterion => View.Notes.Where(n => n.Criterion is null).ToList(),
        _ => View.Notes.Where(n => n.Criterion?.ToString() == filter).ToList(),
    };

    private async Task AddAsync()
    {
        errorMessage = null;
        busy = true;
        var (result, classification) = await EvaluationService.AddNoteAsync(SupervisorId, View.Employee.Id, text, DateTime.Now);
        busy = false;

        if (!result.Succeeded)
        {
            errorMessage = result.Error;
            return;
        }

        text = string.Empty;
        var criterionText = classification?.Criterion is Criterion criterion ? EvaluationCriteria.Name(criterion) : "bez kryterium";
        var toneText = EvaluationText.NoteToneLabel(classification?.Tone ?? NoteTone.Neutral).ToLowerInvariant();
        NotificationService.Notify(NotificationSeverity.Success, "Dodano notatkę", $"{View.Employee.FullName}: {criterionText}, {toneText}");
        await OnChanged.InvokeAsync();
    }

    private async Task EditAsync(EvaluationNote note)
    {
        if (await EditNoteDialog.OpenAsync(DialogService, note, View.Criteria, SupervisorId, View.Employee.Id) is not { } edit)
        {
            return;
        }

        var detail = edit.Reclassified is { } classification
            ? $"Asystent AI ocenił notatkę od nowa: {(classification.Criterion is Criterion criterion ? EvaluationCriteria.Name(criterion) : "bez kryterium")}, {EvaluationText.NoteToneLabel(classification.Tone).ToLowerInvariant()}."
            : View.Employee.FullName;
        NotificationService.Notify(NotificationSeverity.Success, "Zapisano zmiany w notatce", detail);
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
