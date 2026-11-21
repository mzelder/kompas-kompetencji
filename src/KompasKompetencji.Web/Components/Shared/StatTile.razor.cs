using Microsoft.AspNetCore.Components;

namespace KompasKompetencji.Web.Components.Shared;

public partial class StatTile
{
    [Parameter, EditorRequired] public string Icon { get; set; } = string.Empty;

    [Parameter] public string? IconColor { get; set; }

    [Parameter, EditorRequired] public string Label { get; set; } = string.Empty;

    [Parameter, EditorRequired] public string Value { get; set; } = string.Empty;

    [Parameter] public bool Compact { get; set; }
}
