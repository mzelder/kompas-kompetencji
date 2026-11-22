using KompasKompetencji.Web.Assistant;
using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace KompasKompetencji.Web.Components.Pages.Przelozony.Stages;

public partial class CriterionAssessmentEditor
{
    [Inject] private WrittenEvaluationService WrittenEvaluations { get; set; } = default!;
    [Inject] private NotificationService NotificationService { get; set; } = default!;
    [Inject] private DialogService DialogService { get; set; } = default!;

    [Parameter, EditorRequired] public CriterionAssessment Assessment { get; set; } = default!;

    [Parameter, EditorRequired] public IReadOnlyList<EvaluationNote> Notes { get; set; } = [];

    [Parameter] public bool ReadOnly { get; set; }

    [Parameter, EditorRequired] public int SupervisorId { get; set; }

    [Parameter, EditorRequired] public int EmployeeId { get; set; }

    [Parameter] public EventCallback OnChanged { get; set; }

    [Parameter] public bool HasNext { get; set; }

    [Parameter] public EventCallback OnNext { get; set; }

    private sealed record Option(string Value, string Text);

    private const string NoBasisValue = "none";
    private const string NoBasisText = "Brak podstaw do oceny";

    private static readonly IReadOnlyList<Option> RatingOptions =
    [
        .. CompetencyLevels.All.Select(l => new Option(l.ToString(), CompetencyLevels.Name(l))),
        new Option(NoBasisValue, NoBasisText),
    ];

    private readonly HashSet<int> expandedIndicators = [];
    private IndicatorSuggestions? suggestions;
    private bool suggesting;

    private string PendingLevelText => Assessment switch
    {
        { AllAnswered: false } => $"Oceń wszystkie wskaźniki ({Assessment.AnsweredCount} z {Assessment.Indicators.Count})",
        _ => "Nie można wyliczyć: przy wszystkich wskaźnikach jest brak podstaw do oceny",
    };

    private static string? RatingValue(IndicatorAssessment indicator) =>
        indicator.IsAnswered ? indicator.Level?.ToString() ?? NoBasisValue : null;

    private string BehaviorClass(bool isSelected) =>
        $"behavior-option{(isSelected ? " selected" : string.Empty)}{(ReadOnly ? " readonly" : string.Empty)}";

    private void ToggleExpanded(int indicatorId)
    {
        if (!expandedIndicators.Remove(indicatorId))
        {
            expandedIndicators.Add(indicatorId);
        }
    }

    private Task SelectBehaviorAsync(IndicatorAssessment indicator, int level) =>
        ReadOnly ? Task.CompletedTask : RateAsync(indicator, level.ToString());

    private async Task RateAsync(IndicatorAssessment indicator, string? value)
    {
        if (ReadOnly || value is null)
        {
            return;
        }

        int? level = value == NoBasisValue ? null : int.Parse(value);
        var result = await WrittenEvaluations.RateIndicatorAsync(SupervisorId, EmployeeId, indicator.Indicator.Id, level, DateTime.Now);
        if (!result.Succeeded)
        {
            NotificationService.Notify(NotificationSeverity.Error, "Nie udało się zapisać wskaźnika", result.Error ?? string.Empty);
            return;
        }
        await OnChanged.InvokeAsync();
    }

    private async Task SuggestAsync()
    {
        suggesting = true;
        suggestions = await WrittenEvaluations.SuggestFromNotesAsync(SupervisorId, EmployeeId, Assessment.Criterion);
        suggesting = false;
        if (!suggestions.IsAvailable)
        {
            NotificationService.Notify(NotificationSeverity.Info, "Podpowiedź AI jest niedostępna",
                "Asystent AI nie odpowiedział. Sprawdź klucz API asystenta albo spróbuj ponownie.");
        }
    }

    private Task ShowNotesAsync() =>
        DialogService.OpenAsync<CriterionNotesDialog>(
            $"Notatki: {EvaluationCriteria.Name(Assessment.Criterion)}",
            new Dictionary<string, object?> { [nameof(CriterionNotesDialog.Notes)] = Notes },
            new DialogOptions { Width = "640px" });
}
