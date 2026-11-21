using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Organization;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace KompasKompetencji.Web.Components.Pages.Hr;

public partial class PersonDetails
{
    [Parameter, EditorRequired] public Employee Person { get; set; } = default!;

    [Parameter, EditorRequired] public OrganizationChart Chart { get; set; } = default!;

    private static string NamesOrNone(IReadOnlyList<Employee> people) =>
        people.Count == 0 ? "Brak" : string.Join(", ", people.Select(p => p.FullName));
}
