namespace KompasKompetencji.Web.Data;

public enum ConversationType
{
    Starting,
    Assessment,
}

public enum ConversationStatus
{
    Scheduled,
    Held,
}

public class Conversation
{
    public int Id { get; set; }

    public int EvaluationId { get; set; }

    public Evaluation Evaluation { get; set; } = null!;

    public ConversationType Type { get; set; }

    public DateTime At { get; set; }

    public ConversationStatus Status { get; set; }
}
