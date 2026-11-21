using KompasKompetencji.Web.Evaluations;
using Microsoft.AspNetCore.Components;

namespace KompasKompetencji.Web.Components.Shared;

public partial class StageMarker
{
    [Parameter, EditorRequired] public EvaluationStage Stage { get; set; }
}
