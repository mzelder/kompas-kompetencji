using KompasKompetencji.Web.Data;

namespace KompasKompetencji.Web.Evaluations;

public enum EvaluationUrgency
{
    Overdue,
    InProgress,
    DueSoon,
    Planned,
}

public sealed record EvaluationTask(
    Employee Employee,
    DateOnly DueDate,
    bool IsFirstEvaluation,
    EvaluationStatus? OpenStatus,
    EvaluationSheet Sheet,
    EvaluationUrgency Urgency,
    DateTime? ConversationAt,
    ConversationStatus? ConversationStatus,
    bool IsSheetConfirmed)
{
    public bool IsInConversationWindow => Urgency != EvaluationUrgency.Planned;

    public bool CanScheduleConversation => IsInConversationWindow && IsSheetConfirmed;

    public bool NeedsSheet => IsInConversationWindow && !IsSheetConfirmed;

    public bool NeedsConversation => CanScheduleConversation && ConversationAt is null;

    public bool NeedsAction => NeedsSheet || NeedsConversation;
}

public static class EvaluationSchedule
{
    public const int DueSoonDays = 60;

    public static DateOnly NextDueDate(DateOnly employedSince, DateOnly? lastCompletedDueDate) =>
        lastCompletedDueDate?.AddYears(1) ?? employedSince.AddYears(1);

    public static EvaluationSheet SheetFor(Employee employee) =>
        employee.IsSupervisor ? EvaluationSheet.B : EvaluationSheet.C;

    public static Evaluation? OpenEvaluation(IEnumerable<Evaluation> evaluations) =>
        evaluations.Where(v => v.Status != EvaluationStatus.Completed).MaxBy(v => v.DueDate);

    public static EvaluationTask Describe(Employee employee, IEnumerable<Evaluation> evaluations, DateOnly today)
    {
        var list = evaluations.ToList();
        var open = OpenEvaluation(list);
        var lastCompleted = list.Where(v => v.Status == EvaluationStatus.Completed).MaxBy(v => v.DueDate);
        var dueDate = open?.DueDate ?? NextDueDate(employee.EmployedSince, lastCompleted?.DueDate);
        var started = open is not null && open.Status != EvaluationStatus.NotStarted;

        var urgency =
            dueDate < today ? EvaluationUrgency.Overdue
            : started ? EvaluationUrgency.InProgress
            : dueDate <= today.AddDays(DueSoonDays) ? EvaluationUrgency.DueSoon
            : EvaluationUrgency.Planned;

        return new EvaluationTask(
            employee,
            dueDate,
            IsFirstEvaluation: lastCompleted is null,
            OpenStatus: open?.Status,
            Sheet: open?.Sheet ?? SheetFor(employee),
            urgency,
            ConversationAt: open?.ConversationAt,
            ConversationStatus: open?.ConversationStatus,
            IsSheetConfirmed: open?.SheetConfirmedAt is not null);
    }
}
