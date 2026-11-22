using KompasKompetencji.Web.Evaluations;

namespace KompasKompetencji.Web.Data;

public class CompetencyIndicator
{
    public int Id { get; set; }

    public Criterion Criterion { get; set; }

    public int Number { get; set; }

    public required string Text { get; set; }

    public List<IndicatorBehavior> Behaviors { get; set; } = [];
}

public class IndicatorBehavior
{
    public int IndicatorId { get; set; }

    public CompetencyIndicator Indicator { get; set; } = null!;

    public int Level { get; set; }

    public required string Description { get; set; }
}
