using Microsoft.AspNetCore.Components;

namespace KompasKompetencji.Web.Components.Shared;

public partial class PageHeader
{
    [Parameter, EditorRequired] public string Title { get; set; } = string.Empty;

    [Parameter] public string? Subtitle { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public bool Compact { get; set; }
}
