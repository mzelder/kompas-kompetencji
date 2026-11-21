using KompasKompetencji.Web.Organization;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace KompasKompetencji.Web.Components.Pages.Hr;

public partial class AddEmployeeDialog
{
    [Inject] private DialogService DialogService { get; set; } = default!;
    [Inject] private OrganizationService Organization { get; set; } = default!;

    private readonly EmployeeForm form = new();
    private bool busy;

    private async Task SaveAsync(EmployeeForm model)
    {
        busy = true;
        var employee = await Organization.AddEmployeeAsync(
            new NewEmployee(model.FirstName, model.LastName, model.Position, model.Department, model.IsSupervisor,
                DateOnly.FromDateTime(model.EmployedSince)));
        busy = false;
        DialogService.Close(employee);
    }

    private sealed class EmployeeForm
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public bool IsSupervisor { get; set; }
        public DateTime EmployedSince { get; set; } = DateTime.Today;
    }
}
