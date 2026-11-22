using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Organization;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace KompasKompetencji.Web.Components.Pages.Hr;

public partial class AddEmployeeDialog
{
    [Inject] private DialogService DialogService { get; set; } = default!;
    [Inject] private OrganizationService Organization { get; set; } = default!;

    private readonly EmployeeForm form = new();
    private IReadOnlyList<PositionItem> positions = [];
    private bool busy;

    private PositionItem? SelectedPosition => positions.FirstOrDefault(p => p.Id == form.JobPositionId);

    protected override async Task OnInitializedAsync()
    {
        var options = await Organization.GetJobPositionsAsync();
        positions = options.Select(o => new PositionItem(o.Position, o.Occupants)).ToList();
    }

    private async Task SaveAsync(EmployeeForm model)
    {
        if (model.JobPositionId is not int jobPositionId)
        {
            return;
        }

        busy = true;
        var employee = await Organization.AddEmployeeAsync(
            new NewEmployee(model.FirstName, model.LastName, jobPositionId, DateOnly.FromDateTime(model.EmployedSince)));
        busy = false;
        DialogService.Close(employee);
    }

    private sealed class EmployeeForm
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int? JobPositionId { get; set; }
        public DateTime EmployedSince { get; set; } = DateTime.Today;
    }

    private sealed record PositionItem(JobPosition Position, IReadOnlyList<string> Occupants)
    {
        public int Id => Position.Id;

        public string Label => $"{Position.Title} · {Position.Symbol}";

        public string Details => Occupants.Count == 0
            ? $"{Position.OrganizationalUnit} · stanowisko wolne"
            : $"{Position.OrganizationalUnit} · obecnie: {string.Join(", ", Occupants)}";
    }
}
