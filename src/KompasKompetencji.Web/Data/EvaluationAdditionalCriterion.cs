using KompasKompetencji.Web.Evaluations;

namespace KompasKompetencji.Web.Data;

public class EvaluationAdditionalCriterion
{
    public int EvaluationId { get; set; }

    public Evaluation Evaluation { get; set; } = null!;

    public Criterion Criterion { get; set; }
}
