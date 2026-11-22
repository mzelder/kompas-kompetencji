using KompasKompetencji.Web.Evaluations;
using Microsoft.AspNetCore.Components;

namespace KompasKompetencji.Web.Components.Shared;

public partial class EvaluationResult
{
    [Parameter] public WrittenEvaluationSummary? Summary { get; set; }

    [Parameter] public string EmptyText { get; set; } = "–";
}
