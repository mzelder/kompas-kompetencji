using KompasKompetencji.Web.Evaluations;
using Microsoft.AspNetCore.Components;

namespace KompasKompetencji.Web.Components.Pages.Przelozony;

public partial class EvaluationTaskItem
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    [Parameter, EditorRequired] public EvaluationTask Task { get; set; } = default!;

    [Parameter, EditorRequired] public DateOnly Today { get; set; }

    [Parameter] public bool ConversationPastDue { get; set; }

    [Parameter] public EventCallback<EvaluationTask> OnConversation { get; set; }

    [Parameter] public EventCallback<EvaluationTask> OnMarkHeld { get; set; }

    private int OverdueDays => Today.DayNumber - Task.DueDate.DayNumber;
}
