using Microsoft.AspNetCore.Components;
using Radzen;

namespace KompasKompetencji.Web.Components.Shared;

public partial class StatusMessage
{
    [Parameter, EditorRequired] public string Icon { get; set; } = string.Empty;

    [Parameter] public string IconColor { get; set; } = Colors.Primary;

    [Parameter, EditorRequired] public string Title { get; set; } = string.Empty;

    [Parameter] public string? Text { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }
}
