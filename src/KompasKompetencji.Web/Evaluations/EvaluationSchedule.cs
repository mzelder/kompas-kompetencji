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
    bool IsSheetConfirmed,
    DateOnly CycleStart,
    DateTime? StartingConversationAt,
    ConversationStatus? StartingConversationStatus,
    bool IsPartIConfirmed)
{
    public DateOnly StartingConversationDeadline => StartingConversationRules.Deadline(CycleStart);

    public DateTime? ConversationAtFor(ConversationType type) =>
        type == ConversationType.Starting ? StartingConversationAt : ConversationAt;

    public ConversationStatus? ConversationStatusFor(ConversationType type) =>
        type == ConversationType.Starting ? StartingConversationStatus : ConversationStatus;

    public ConversationType? PastDueConversation(DateTime now) =>
        ConversationRules.IsPastDue(StartingConversationAt, StartingConversationStatus, now) ? ConversationType.Starting
        : ConversationRules.IsPastDue(ConversationAt, ConversationStatus, now) ? ConversationType.Assessment
        : null;

    public bool IsInConversationWindow => Urgency != EvaluationUrgency.Planned;

    public DateOnly ConversationWindowOpensOn => DueDate.AddDays(-EvaluationSchedule.DueSoonDays);

    public bool CanScheduleConversation => IsInConversationWindow && IsSheetConfirmed && IsPartIConfirmed;

    public bool NeedsStart => !IsPartIConfirmed && StartingConversationStatus != Data.ConversationStatus.Scheduled;

    public bool NeedsConversation => CanScheduleConversation && ConversationAt is null;

    public bool NeedsAction => NeedsStart || NeedsConversation;
}

public static class EvaluationSchedule
{
    public const int DueSoonDays = 60;

    public static DateOnly NextDueDate(DateOnly employedSince, DateOnly? lastCompletedDueDate) =>
        lastCompletedDueDate?.AddYears(1) ?? employedSince.AddYears(1);

    public static EvaluationSheet SheetFor(Employee employee) =>
        employee.IsSupervisor ? EvaluationSheet.B : EvaluationSheet.C;

    public static Evaluation? LastCompleted(IEnumerable<Evaluation> evaluations) =>
        evaluations.Where(v => v.Status == EvaluationStatus.Completed).MaxBy(v => v.DueDate);

    public static DateOnly CycleStart(Employee employee, IEnumerable<Evaluation> evaluations) =>
        LastCompleted(evaluations)?.CompletedAt is DateTime completedAt
            ? DateOnly.FromDateTime(completedAt)
            : employee.EmployedSince;

    public static Evaluation? OpenEvaluation(IEnumerable<Evaluation> evaluations) =>
        evaluations.Where(v => v.Status != EvaluationStatus.Completed).MaxBy(v => v.DueDate);

    public static EvaluationTask Describe(Employee employee, IEnumerable<Evaluation> evaluations, DateOnly today)
    {
        var list = evaluations.ToList();
        var open = OpenEvaluation(list);
        var lastCompleted = LastCompleted(list);
        var dueDate = open?.DueDate ?? NextDueDate(employee.EmployedSince, lastCompleted?.DueDate);
        var started = open is not null && open.Status >= EvaluationStatus.InProgress;
        var assessmentConversation = open?.FindConversation(ConversationType.Assessment);
        var startingConversation = open?.FindConversation(ConversationType.Starting);

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
            ConversationAt: assessmentConversation?.At,
            ConversationStatus: assessmentConversation?.Status,
            IsSheetConfirmed: open?.SheetConfirmedAt is not null,
            CycleStart: CycleStart(employee, list),
            StartingConversationAt: startingConversation?.At,
            StartingConversationStatus: startingConversation?.Status,
            IsPartIConfirmed: open?.PartIConfirmedAt is not null);
    }
}
