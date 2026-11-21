using KompasKompetencji.Web.Data;

namespace KompasKompetencji.Web.Evaluations;

public enum EvaluationStage
{
    JobDescriptionReview,
    StartingConversation,
    Assessment,
    Completion,
}

public static class EvaluationStages
{
    public static IReadOnlyList<EvaluationStage> All { get; } = Enum.GetValues<EvaluationStage>();

    public static EvaluationStage Current(Evaluation? openEvaluation) =>
        openEvaluation?.SheetConfirmedAt is null ? EvaluationStage.JobDescriptionReview
        : openEvaluation.PartIConfirmedAt is null ? EvaluationStage.StartingConversation
        : EvaluationStage.Assessment;
}
