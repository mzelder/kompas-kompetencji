using KompasKompetencji.Web.Assistant;
using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace KompasKompetencji.Web.Components.Pages.Przelozony.Stages;

public partial class EditNoteDialog
{
    [Inject] private DialogService DialogService { get; set; } = default!;
    [Inject] private EvaluationService EvaluationService { get; set; } = default!;

    [Parameter, EditorRequired] public EvaluationNote Note { get; set; } = default!;

    [Parameter, EditorRequired] public IReadOnlyList<Criterion> Criteria { get; set; } = [];

    [Parameter, EditorRequired] public int SupervisorId { get; set; }

    [Parameter, EditorRequired] public int EmployeeId { get; set; }

    public sealed record NoteEditResult(NoteClassification? Reclassified);

    private sealed record CriterionOption(string Value, string Text);

    private const string WithoutCriterion = "none";

    private string text = string.Empty;
    private string criterionKey = WithoutCriterion;
    private NoteTone tone;
    private bool manual;
    private bool busy;
    private string? errorMessage;

    public static async Task<NoteEditResult?> OpenAsync(
        DialogService dialogService, EvaluationNote note, IReadOnlyList<Criterion> criteria, int supervisorId, int employeeId)
    {
        var saved = await dialogService.OpenAsync<EditNoteDialog>(
            "Edytuj notatkę",
            new Dictionary<string, object?>
            {
                [nameof(Note)] = note,
                [nameof(Criteria)] = criteria,
                [nameof(SupervisorId)] = supervisorId,
                [nameof(EmployeeId)] = employeeId,
            },
            new DialogOptions { Width = "560px" });
        return saved as NoteEditResult;
    }

    private IReadOnlyList<CriterionOption> CriterionOptions =>
    [
        .. Criteria.Select(c => new CriterionOption(c.ToString(), EvaluationCriteria.Name(c))),
        new CriterionOption(WithoutCriterion, "Bez kryterium"),
    ];

    protected override void OnInitialized()
    {
        text = Note.Text;
        criterionKey = Note.Criterion?.ToString() ?? WithoutCriterion;
        tone = Note.Tone;
        manual = Note.ClassificationEditedAt is not null;
    }

    private void OnManualChanged(bool value)
    {
        manual = value;
        if (!manual)
        {
            criterionKey = Note.Criterion?.ToString() ?? WithoutCriterion;
            tone = Note.Tone;
        }
    }

    private async Task SaveAsync()
    {
        errorMessage = null;
        busy = true;
        Criterion? criterion = Enum.TryParse<Criterion>(criterionKey, out var parsed) ? parsed : null;
        var (result, reclassified) = await EvaluationService.UpdateNoteAsync(SupervisorId, EmployeeId, Note.Id, text, manual, criterion, tone, DateTime.Now);
        busy = false;

        if (!result.Succeeded)
        {
            errorMessage = result.Error;
            return;
        }
        DialogService.Close(new NoteEditResult(reclassified));
    }
}
