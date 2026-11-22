using KompasKompetencji.Web.Evaluations;

namespace KompasKompetencji.Web.Data;

public class IndicatorRating
{
    public int EvaluationId { get; set; }

    public Evaluation Evaluation { get; set; } = null!;

    public int IndicatorId { get; set; }

    public CompetencyIndicator Indicator { get; set; } = null!;

    public int? Level { get; set; }
}

public class CriterionDecision
{
    public const int MaxJustificationLength = 2000;

    public int EvaluationId { get; set; }

    public Evaluation Evaluation { get; set; } = null!;

    public Criterion Criterion { get; set; }

    public int? SuggestedLevel { get; set; }

    public int Level { get; set; }

    public string? Justification { get; set; }

    public DateTime DecidedAt { get; set; }
}
