using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using Microsoft.AspNetCore.Components;

namespace KompasKompetencji.Web.Components.Pages.Przelozony;

public partial class EvaluationTaskItem
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    [Parameter, EditorRequired] public EvaluationTask Task { get; set; } = default!;

    [Parameter, EditorRequired] public DateOnly Today { get; set; }

    [Parameter] public ConversationType? PastDueConversation { get; set; }

    [Parameter] public EventCallback<ConversationType> OnConversation { get; set; }

    [Parameter] public EventCallback<ConversationType> OnMarkHeld { get; set; }

    private bool ShowsStartingDeadline => PastDueConversation == ConversationType.Starting
        || (PastDueConversation is null && Task.NeedsStart);

    private DateOnly ShownDate => ShowsStartingDeadline ? Task.StartingConversationDeadline : Task.DueDate;

    private bool IsStartingDeadlineOverdue => Task.StartingConversationDeadline < Today;

    private string StartIcon => Task.IsSheetConfirmed ? "event_busy" : "assignment_late";

    private string StartText
    {
        get
        {
            var deadline = IsStartingDeadlineOverdue
                ? "jak najszybciej"
                : $"najpóźniej {EvaluationText.Date(Task.StartingConversationDeadline)}";
            if (!Task.IsSheetConfirmed)
            {
                return $"Zatwierdź arkusz i wyznacz termin rozmowy rozpoczynającej, {deadline}";
            }

            return Task.StartingConversationAt is null
                ? $"Wyznacz termin rozmowy rozpoczynającej, {deadline}"
                : "Rozmowa rozpoczynająca odbyta, zatwierdź część I arkusza";
        }
    }

    private int OverdueDays => Today.DayNumber - ShownDate.DayNumber;
}
