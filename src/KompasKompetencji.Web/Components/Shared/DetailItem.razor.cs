using Microsoft.AspNetCore.Components;

namespace KompasKompetencji.Web.Components.Shared;

public partial class DetailItem
{
    [Parameter, EditorRequired] public string Label { get; set; } = string.Empty;

    [Parameter, EditorRequired] public string Value { get; set; } = string.Empty;

    [Parameter] public bool Fill { get; set; }
}
