using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace KompasKompetencji.Web.Components.Pages.Przelozony.Stages;

public partial class WrittenEvaluationCard
{
    [Inject] private WrittenEvaluationService WrittenEvaluations { get; set; } = default!;
    [Inject] private NotificationService NotificationService { get; set; } = default!;
    [Inject] private DialogService DialogService { get; set; } = default!;

    [Parameter, EditorRequired] public EmployeeEvaluationView View { get; set; } = default!;

    [Parameter, EditorRequired] public int SupervisorId { get; set; }

    [Parameter] public EventCallback OnChanged { get; set; }

    private WrittenEvaluationView? form;
    private Criterion? selected;
    private bool loading = true;
    private bool collapsed;
    private bool finishing;
    private string? loadError;

    private CriterionAssessment? Current =>
        form?.Criteria.FirstOrDefault(c => c.Criterion == selected) ?? form?.Criteria.FirstOrDefault();

    private string Title => form is { IsUnlocked: true } f
        ? $"Ocena kryteriów ({f.DecidedCount}/{f.Criteria.Count})"
        : "Ocena kryteriów";

    private void ToggleCollapsed() => collapsed = !collapsed;

    private void OnHeadingKeyDown(KeyboardEventArgs args)
    {
        if (args.Key is "Enter" or " ")
        {
            ToggleCollapsed();
        }
    }

    private string Subtitle => form switch
    {
        { IsUnlocked: true } f => $"Część II arkusza: oceń wskaźniki każdego kryterium, a jego poziom wyliczy się sam. Ocenione kryteria: {f.DecidedCount} z {f.Criteria.Count}.",
        _ => "Część II arkusza: wskaźniki behawioralne z modelu kompetencji i poziom każdego kryterium.",
    };

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
        selected = form?.Criteria.FirstOrDefault(c => !c.IsDecided)?.Criterion ?? form?.Criteria.FirstOrDefault()?.Criterion;
        loading = false;
    }

    private async Task LoadAsync()
    {
        try
        {
            form = await WrittenEvaluations.GetAsync(SupervisorId, View.Employee.Id);
            loadError = null;
        }
        catch (Exception ex)
        {
            loadError = ex.Message;
        }
    }

    private IReadOnlyList<EvaluationNote> NotesFor(Criterion criterion) =>
        View.Notes.Where(n => n.Criterion == criterion).ToList();

    private void Select(Criterion criterion) => selected = criterion;

    private Task OnCriterionChangedAsync() => LoadAsync();

    private Criterion? NextUndecided()
    {
        if (form is null || Current is not CriterionAssessment current)
        {
            return null;
        }

        var order = form.Criteria.Select(c => c.Criterion).ToList();
        var start = order.IndexOf(current.Criterion);
        return order.Skip(start + 1).Concat(order.Take(start))
            .Select(c => (Criterion?)c)
            .FirstOrDefault(c => form.Criteria.First(a => a.Criterion == c) is { IsDecided: false });
    }

    private void SelectNext()
    {
        if (NextUndecided() is Criterion next)
        {
            selected = next;
        }
    }

    private async Task FinishAsync()
    {
        var confirmed = await DialogService.Confirm(
            "Po przejściu do zakończenia oceny kryteriów nie da się już zmienić.",
            "Zakończyć ocenę na piśmie?",
            new ConfirmOptions { OkButtonText = "Przejdź do zakończenia", CancelButtonText = "Anuluj" });
        if (confirmed is not true)
        {
            return;
        }

        finishing = true;
        var result = await WrittenEvaluations.FinishAsync(SupervisorId, View.Employee.Id, DateTime.Now);
        finishing = false;
        if (!result.Succeeded)
        {
            NotificationService.Notify(NotificationSeverity.Error, "Nie udało się zakończyć oceny", result.Error ?? string.Empty);
            return;
        }

        NotificationService.Notify(NotificationSeverity.Success, "Ocena na piśmie zapisana", View.Employee.FullName);
        await OnChanged.InvokeAsync();
    }

    private static string StatusIcon(CriterionAssessment assessment) =>
        assessment.IsDecided ? "check_circle" : assessment.AnsweredCount > 0 ? "pending" : "radio_button_unchecked";

    private static string StatusColor(CriterionAssessment assessment) =>
        assessment.IsDecided ? Colors.Success : assessment.AnsweredCount > 0 ? Colors.Warning : Colors.Base400;

    private static string StatusText(CriterionAssessment assessment) =>
        assessment.IsDecided ? $"Ocenione: {CompetencyLevels.Name(assessment.Decision!.Level)}"
        : assessment.AnsweredCount > 0 ? $"W trakcie: {assessment.AnsweredCount} z {assessment.Indicators.Count} wskaźników"
        : "Nieocenione";
}
