using KompasKompetencji.Web.Evaluations;
using Microsoft.AspNetCore.Components;

namespace KompasKompetencji.Web.Components.Shared;

public partial class StageStepLabel
{
    [Parameter, EditorRequired] public EvaluationStage Stage { get; set; }

    [Parameter] public bool ShowTitle { get; set; } = true;
}
