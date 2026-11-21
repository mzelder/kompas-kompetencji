using KompasKompetencji.Web.Data;
using Microsoft.AspNetCore.Components;

namespace KompasKompetencji.Web.Components.Shared;

public partial class PersonName
{
    [Parameter, EditorRequired] public Employee Person { get; set; } = default!;

    [Parameter] public bool ShowSupervisorIcon { get; set; } = true;
}
