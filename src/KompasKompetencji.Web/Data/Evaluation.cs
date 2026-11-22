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

public class Evaluation
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    public int EvaluatorId { get; set; }

    public Employee Evaluator { get; set; } = null!;

    public EvaluationSheet Sheet { get; set; }

    public DateTime? SheetConfirmedAt { get; set; }

    public DateTime? WrittenEvaluationAt { get; set; }

    public EvaluationStatus Status { get; set; }

    public DateOnly PeriodStart { get; set; }

    public DateOnly? PeriodEnd { get; set; }

    public DateOnly DueDate { get; set; }

    public string? Expectations { get; set; }

    public string? Goals { get; set; }

    public DateTime? PartIConfirmedAt { get; set; }

    public DateOnly? EmployeeAcquaintedOn { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateOnly? PreparedOn { get; set; }

    public DateOnly? PartIIAcquaintedOn { get; set; }

    public List<Conversation> Conversations { get; set; } = [];

    public List<EvaluationAdditionalCriterion> AdditionalCriteria { get; set; } = [];

    public Conversation? FindConversation(ConversationType type) =>
        Conversations.FirstOrDefault(c => c.Type == type);
}
