using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Organization;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace KompasKompetencji.Web.Components.Pages.Hr;

public partial class AssignSupervisorDialog
{
    [Inject] private DialogService DialogService { get; set; } = default!;
    [Inject] private OrganizationService Organization { get; set; } = default!;

    [Parameter, EditorRequired] public Data.Employee Employee { get; set; } = default!;

    [Parameter, EditorRequired] public IReadOnlyList<Data.Employee> Candidates { get; set; } = [];

    private IEnumerable<int> selectedIds = [];
    private bool busy;
    private string? errorMessage;

    private async Task AssignAsync()
    {
        errorMessage = null;
        busy = true;
        var result = await Organization.AssignSupervisorsAsync(Employee.Id, selectedIds.ToList());
        busy = false;

        if (!result.Succeeded)
        {
            errorMessage = result.Error;
            return;
        }
        DialogService.Close(true);
    }
}
