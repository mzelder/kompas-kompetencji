using Microsoft.AspNetCore.Components;

namespace KompasKompetencji.Web.Components.Shared;

public partial class AiAssistBadge
{
    [Parameter, EditorRequired] public string Description { get; set; } = string.Empty;
}
