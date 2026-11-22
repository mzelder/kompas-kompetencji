using KompasKompetencji.Web.Data;

namespace KompasKompetencji.Web.Evaluations;

public enum CompletionStep
{
    Preparation,
    AwaitingAcquaintance,
}

public sealed record AllowedDates(DateOnly Min, DateOnly Max)
{
    public bool Contains(DateOnly date) => date >= Min && date <= Max;
}

public static class CompletionRules
{
    public const string AssessmentConversationNotHeld = "Oznacz rozmowę oceniającą jako odbytą przed zamknięciem oceny.";

    public static CompletionStep Step(Evaluation evaluation) =>
        evaluation.PreparedOn is null ? CompletionStep.Preparation : CompletionStep.AwaitingAcquaintance;

    public static DateOnly? AssessmentConversationDay(Evaluation evaluation) =>
        evaluation.FindConversation(ConversationType.Assessment) is { Status: ConversationStatus.Held } conversation
            ? DateOnly.FromDateTime(conversation.At)
            : null;

    public static AllowedDates PreparedOnRange(Evaluation evaluation, DateOnly today)
    {
        var min = AssessmentConversationDay(evaluation) ?? evaluation.PeriodStart;
        return new AllowedDates(min > today ? today : min, today);
    }

    public static AllowedDates AcquaintedOnRange(Evaluation evaluation, DateOnly today)
    {
        var min = evaluation.PreparedOn ?? today;
        return new AllowedDates(min > today ? today : min, today);
    }

    public static string? ValidatePreparedOn(Evaluation evaluation, DateOnly preparedOn, DateOnly today)
    {
        if (AssessmentConversationDay(evaluation) is null)
        {
            return AssessmentConversationNotHeld;
        }

        var range = PreparedOnRange(evaluation, today);
        return range.Contains(preparedOn)
            ? null
            : $"Data sporządzenia oceny musi przypadać między dniem rozmowy oceniającej ({EvaluationText.Date(range.Min)}) a dzisiejszym dniem.";
    }

    public static string? ValidateAcquaintedOn(Evaluation evaluation, DateOnly acquaintedOn, DateOnly today)
    {
        if (evaluation.PreparedOn is null)
        {
            return "Najpierw zapisz datę sporządzenia oceny.";
        }

        var range = AcquaintedOnRange(evaluation, today);
        return range.Contains(acquaintedOn)
            ? null
            : $"Data zapoznania pracownika z oceną musi przypadać między datą sporządzenia oceny ({EvaluationText.Date(range.Min)}) a dzisiejszym dniem.";
    }

    public static string? CloseBlocker(Evaluation evaluation) =>
        AssessmentConversationDay(evaluation) is null ? AssessmentConversationNotHeld
        : evaluation.WrittenEvaluationAt is null ? "Najpierw sporządź ocenę na piśmie."
        : evaluation.PreparedOn is null ? "Najpierw zapisz datę sporządzenia oceny."
        : null;
}
