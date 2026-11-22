using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace KompasKompetencji.Web.Components.Pages.Przelozony.Stages;

public partial class JobDescriptionStage
{
    [Inject] private EvaluationService EvaluationService { get; set; } = default!;
    [Inject] private NotificationService NotificationService { get; set; } = default!;

    [Parameter, EditorRequired] public EmployeeEvaluationView View { get; set; } = default!;

    [Parameter, EditorRequired] public int SupervisorId { get; set; }

    [Parameter] public bool ReadOnly { get; set; }

    [Parameter] public EventCallback OnConfirmed { get; set; }

    private EvaluationSheet sheet;
    private bool busy;
    private string? errorMessage;

    private string PositionCaption => View.JobPosition is JobPosition position
        ? $"{position.Title} · {position.Symbol} · {position.OrganizationalUnit}"
        : $"{View.Employee.Position} · {View.Employee.Department}";

    private EvaluationSheet ConfirmedSheet => View.OpenEvaluation?.Sheet ?? View.Task.Sheet;

    protected override void OnInitialized()
    {
        sheet = View.SuggestedSheet;
    }

    private string OptionLabel(EvaluationSheet option) =>
        option == View.SuggestedSheet ? $"{EvaluationText.Sheet(option)} (podpowiedź)" : EvaluationText.Sheet(option);

    private async Task ConfirmAsync()
    {
        errorMessage = null;
        busy = true;
        var result = await EvaluationService.ConfirmSheetAsync(SupervisorId, View.Employee.Id, sheet, DateTime.Now);
        busy = false;

        if (!result.Succeeded)
        {
            errorMessage = result.Error;
            return;
        }

        NotificationService.Notify(NotificationSeverity.Success, "Arkusz zatwierdzony", $"{View.Employee.FullName}: {EvaluationText.Sheet(sheet)}");
        await OnConfirmed.InvokeAsync();
    }
}
