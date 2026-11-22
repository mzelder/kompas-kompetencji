using KompasKompetencji.Web.Evaluations;

namespace KompasKompetencji.Web.Data;

public enum NoteTone
{
    Positive,
    Neutral,
    Negative,
}

public class EvaluationNote
{
    public const int MaxTextLength = 2000;

    public int Id { get; set; }

    public int EvaluationId { get; set; }

    public Evaluation Evaluation { get; set; } = null!;

    public Criterion? Criterion { get; set; }

    public NoteTone Tone { get; set; } = NoteTone.Neutral;

    public DateTime CreatedAt { get; set; }

    public DateTime? TextEditedAt { get; set; }

    public DateTime? ClassificationEditedAt { get; set; }

    public bool IsEdited => TextEditedAt is not null || ClassificationEditedAt is not null;

    public required string Text { get; set; }
}
