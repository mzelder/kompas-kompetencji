using KompasKompetencji.Web.Evaluations;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace KompasKompetencji.Web.Components.Shared;

public partial class EvaluationStageText
{
    [Parameter] public EvaluationStage? Stage { get; set; }

    private static void PreventChange(StepsCanChangeEventArgs args) => args.PreventDefault();
}
