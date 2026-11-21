namespace KompasKompetencji.Web.Data;

public class EvaluationNote
{
    public const int MaxTextLength = 2000;

    public int Id { get; set; }

    public int EvaluationId { get; set; }

    public Evaluation Evaluation { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public required string Text { get; set; }
}
