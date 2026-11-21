namespace KompasKompetencji.Web.Data;

public enum EvaluationStatus
{
    NotStarted,
    CriteriaSet,
    InProgress,
    ReadyForConversation,
    Completed,
}

public enum EvaluationSheet
{
    B,
    C,
}

public enum ConversationStatus
{
    Scheduled,
    Held,
}

public class Evaluation
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    public int EvaluatorId { get; set; }

    public Employee Evaluator { get; set; } = null!;

    public EvaluationSheet Sheet { get; set; }

    public EvaluationStatus Status { get; set; }

    public DateOnly PeriodStart { get; set; }

    public DateOnly PeriodEnd { get; set; }

    public DateOnly DueDate { get; set; }

    public DateTime? ConversationAt { get; set; }

    public ConversationStatus? ConversationStatus { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }
}
