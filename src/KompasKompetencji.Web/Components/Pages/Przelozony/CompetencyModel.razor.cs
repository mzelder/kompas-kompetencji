using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using KompasKompetencji.Web.Text;
using Microsoft.AspNetCore.Components;

namespace KompasKompetencji.Web.Components.Pages.Przelozony;

public partial class CompetencyModel
{
    [Inject] private CompetencyModelService CompetencyModelService { get; set; } = default!;

    private IReadOnlyList<CompetencyView> competencies = [];
    private IReadOnlyList<CompetencyView> filtered = [];
    private Criterion? selected;
    private string? search;
    private bool loading = true;
    private string? loadError;

    private CompetencyView? Current =>
        filtered.FirstOrDefault(c => c.Criterion == selected) ?? filtered.FirstOrDefault();

    private string Subtitle => competencies.Count == 0
        ? "Kompetencje, wskaźniki behawioralne i opisy poziomów spełniania."
        : $"{competencies.Count} kompetencji · {competencies.Sum(c => c.Indicators.Count)} wskaźników behawioralnych · {CompetencyLevels.All.Count} poziomów oceny";

    private string CountCaption => filtered.Count == competencies.Count
        ? $"Kompetencje: {competencies.Count}"
        : $"Wyniki wyszukiwania: {filtered.Count} z {competencies.Count}";

    protected override async Task OnInitializedAsync()
    {
        try
        {
            competencies = await CompetencyModelService.GetAsync();
            filtered = competencies;
        }
        catch (Exception ex)
        {
            loadError = ex.Message;
        }
        loading = false;
    }

    private void Select(Criterion criterion) => selected = criterion;

    private void OnSearchChanged(string value)
    {
        search = value;
        var term = TextNormalizer.Normalize(value?.Trim() ?? string.Empty);
        filtered = term.Length == 0 ? competencies : competencies.Where(c => c.Matches(term)).ToList();
    }

    private static string SheetRole(CompetencyView competency) =>
        (competency.IsMandatoryIn(EvaluationSheet.B), competency.IsMandatoryIn(EvaluationSheet.C)) switch
        {
            (true, true) => "obowiązkowe w B i C",
            (true, false) => "obowiązkowe w B",
            (false, true) => "obowiązkowe w C",
            _ => "dodatkowe",
        };
}
