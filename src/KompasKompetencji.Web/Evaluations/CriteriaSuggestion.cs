using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Text;

namespace KompasKompetencji.Web.Evaluations;

public sealed record SuggestedCriterion(Criterion Criterion, string Competency);

public static class CriteriaSuggestion
{
    private static readonly IReadOnlyDictionary<string, Criterion> CompetencyToCriterion = BuildMapping();

    public static IReadOnlyList<SuggestedCriterion> Suggest(EvaluationSheet sheet, JobPosition? jobPosition)
    {
        if (jobPosition is null)
        {
            return [];
        }

        var mandatory = EvaluationCriteria.Mandatory(sheet);
        var suggestions = new List<SuggestedCriterion>();
        foreach (var competency in jobPosition.RequiredCompetencyList)
        {
            if (!CompetencyToCriterion.TryGetValue(TextNormalizer.Normalize(competency), out var criterion)
                || mandatory.Contains(criterion)
                || suggestions.Any(s => s.Criterion == criterion))
            {
                continue;
            }

            suggestions.Add(new SuggestedCriterion(criterion, competency));
            if (suggestions.Count == EvaluationCriteria.MaxAdditional)
            {
                break;
            }
        }
        return suggestions;
    }

    public static string? CompetencyFor(IEnumerable<SuggestedCriterion> suggestions, Criterion criterion) =>
        suggestions.FirstOrDefault(s => s.Criterion == criterion)?.Competency;

    private static Dictionary<string, Criterion> BuildMapping()
    {
        var aliases = new (string Competency, Criterion Criterion)[]
        {
            ("Orientacja na cele", Criterion.WorkOrganization),
            ("Organizacja pracy własnej", Criterion.WorkOrganization),
            ("Organizacja pracy", Criterion.WorkOrganization),
            ("Komunikacja pisemna", Criterion.Communication),
            ("Komunikacja ustna", Criterion.Communication),
            ("Zarządzanie zespołem", Criterion.PeopleManagement),
            ("Zarządzanie rozwojem pracowników", Criterion.PeopleManagement),
            ("Podejmowanie decyzji", Criterion.DecisionMaking),
            ("Myślenie strategiczne", Criterion.StrategicManagement),
            ("Orientacja na klienta", Criterion.CustomerOrientation),
            ("Doskonalenie zawodowe", Criterion.KnowledgeAndDevelopment),
        };

        var mapping = EvaluationCriteria.All.ToDictionary(c => TextNormalizer.Normalize(EvaluationCriteria.Name(c)));
        foreach (var (competency, criterion) in aliases)
        {
            mapping[TextNormalizer.Normalize(competency)] = criterion;
        }
        return mapping;
    }
}
