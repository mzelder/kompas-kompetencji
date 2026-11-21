using KompasKompetencji.Web.Data;

namespace KompasKompetencji.Web.Evaluations;

public enum Criterion
{
    Communication,
    Creativity,
    Negotiation,
    WorkOrganization,
    CustomerOrientation,
    DecisionMaking,
    CrisisManagement,
    Reliability,
    AnalyticalThinking,
    KnowledgeAndDevelopment,
    Cooperation,
    PublicSpeaking,
    PeopleManagement,
    StrategicManagement,
}

public static class EvaluationCriteria
{
    public const int MaxAdditional = 3;

    public static IReadOnlyList<Criterion> All { get; } = Enum.GetValues<Criterion>();

    private static readonly IReadOnlyList<Criterion> MandatoryForSheetB =
    [
        Criterion.PeopleManagement,
        Criterion.DecisionMaking,
        Criterion.WorkOrganization,
        Criterion.Cooperation,
        Criterion.Communication,
    ];

    private static readonly IReadOnlyList<Criterion> MandatoryForSheetC =
    [
        Criterion.KnowledgeAndDevelopment,
        Criterion.Reliability,
        Criterion.WorkOrganization,
        Criterion.Cooperation,
        Criterion.Communication,
    ];

    public static string Name(Criterion criterion) => criterion switch
    {
        Criterion.Communication => "Komunikacja",
        Criterion.Creativity => "Kreatywność",
        Criterion.Negotiation => "Negocjowanie",
        Criterion.WorkOrganization => "Organizacja pracy i orientacja na osiąganie celów",
        Criterion.CustomerOrientation => "Orientacja na klienta/interesanta",
        Criterion.DecisionMaking => "Podejmowanie decyzji i odpowiedzialność",
        Criterion.CrisisManagement => "Radzenie sobie w sytuacjach kryzysowych",
        Criterion.Reliability => "Rzetelność",
        Criterion.AnalyticalThinking => "Myślenie analityczne",
        Criterion.KnowledgeAndDevelopment => "Wykorzystywanie wiedzy i doskonalenie zawodowe",
        Criterion.Cooperation => "Współpraca",
        Criterion.PublicSpeaking => "Wystąpienia publiczne",
        Criterion.PeopleManagement => "Zarządzanie ludźmi",
        Criterion.StrategicManagement => "Zarządzanie strategiczne",
        _ => criterion.ToString(),
    };

    public static IReadOnlyList<Criterion> Mandatory(EvaluationSheet sheet) =>
        sheet == EvaluationSheet.B ? MandatoryForSheetB : MandatoryForSheetC;

    public static IReadOnlyList<Criterion> AvailableAdditional(EvaluationSheet sheet) =>
        All.Except(Mandatory(sheet)).ToList();

    public static string? ValidateAdditional(EvaluationSheet sheet, IReadOnlyCollection<Criterion> additional)
    {
        if (additional.Distinct().Count() != additional.Count)
        {
            return "Każde kryterium dodatkowe można wybrać tylko raz.";
        }
        if (additional.Count > MaxAdditional)
        {
            return $"Możesz wybrać najwyżej {MaxAdditional} kryteria dodatkowe.";
        }
        if (additional.Any(Mandatory(sheet).Contains))
        {
            return "Kryterium obowiązkowe nie może być kryterium dodatkowym.";
        }
        return null;
    }
}
