using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Organization;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace KompasKompetencji.Web.Components.Pages.Hr;

public partial class Employees
{
    [Inject] private OrganizationService Organization { get; set; } = default!;
    [Inject] private DialogService DialogService { get; set; } = default!;
    [Inject] private NotificationService NotificationService { get; set; } = default!;

    private OrganizationChart chart = OrganizationChart.Empty;
    private bool loading = true;
    private string? loadError;

    private Employee? selectedEmployee;
    private Employee? selectedSupervisor;
    private bool employeeClickedWhileSelected;
    private bool supervisorClickedWhileSelected;
    private IList<Employee> employeeSelection = [];
    private IList<Employee> supervisorSelection = [];

    private string? employeeSearch;
    private string? supervisorSearch;
    private IReadOnlyList<Employee> filteredEmployees = [];
    private IReadOnlyList<Employee> visibleSupervisors = [];
    private IReadOnlyList<Employee> filteredSupervisors = [];

    private Employee? FocusedPerson => selectedSupervisor ?? selectedEmployee;

    private string SupervisorsEmptyText =>
        !string.IsNullOrWhiteSpace(supervisorSearch) && visibleSupervisors.Count > 0 ? "Nikt nie pasuje do wyszukiwania."
        : selectedEmployee is null ? "Brak przełożonych."
        : "Ten pracownik nie ma jeszcze przypisanego przełożonego.";

    private static string CountCaption(int shown, int total, string allLabel) =>
        shown == total ? $"{allLabel}: {total}" : $"Wyniki wyszukiwania: {shown} z {total}";

    protected override async Task OnInitializedAsync()
    {
        await ReloadAsync();
    }

    private async Task ReloadAsync()
    {
        loading = true;
        try
        {
            chart = await Organization.GetChartAsync();
            loadError = null;
        }
        catch (Exception ex)
        {
            chart = OrganizationChart.Empty;
            loadError = ex.Message;
        }
        loading = false;

        selectedEmployee = selectedEmployee is null ? null : chart.Find(selectedEmployee.Id);
        selectedSupervisor = selectedSupervisor is null ? null : chart.Find(selectedSupervisor.Id);
        filteredEmployees = EmployeeSearch.Filter(chart.Employees, employeeSearch);
        UpdateSelection();
    }

    private void OnEmployeeSearchChanged(string value)
    {
        employeeSearch = value;
        filteredEmployees = EmployeeSearch.Filter(chart.Employees, employeeSearch);
    }

    private void OnSupervisorSearchChanged(string value)
    {
        supervisorSearch = value;
        filteredSupervisors = EmployeeSearch.Filter(visibleSupervisors, supervisorSearch);
    }

    private void OnEmployeeRowClick(DataGridRowMouseEventArgs<Employee> args)
    {
        employeeClickedWhileSelected = args.Data is not null && args.Data.Id == selectedEmployee?.Id;
    }

    private void OnEmployeeSelect(Employee employee)
    {
        selectedEmployee = employeeClickedWhileSelected ? null : employee;
        employeeClickedWhileSelected = false;
        selectedSupervisor = null;
        UpdateSelection();
    }

    private void OnEmployeeDeselect(Employee employee)
    {
        selectedEmployee = null;
        selectedSupervisor = null;
        UpdateSelection();
    }

    private void OnSupervisorRowClick(DataGridRowMouseEventArgs<Employee> args)
    {
        supervisorClickedWhileSelected = args.Data is not null && args.Data.Id == selectedSupervisor?.Id;
    }

    private void OnSupervisorSelect(Employee supervisor)
    {
        selectedSupervisor = supervisorClickedWhileSelected ? null : supervisor;
        supervisorClickedWhileSelected = false;
        UpdateSelection();
    }

    private void OnSupervisorDeselect(Employee supervisor)
    {
        selectedSupervisor = null;
        UpdateSelection();
    }

    private void UpdateSelection()
    {
        visibleSupervisors = selectedEmployee is null ? chart.Supervisors : chart.SupervisorsOf(selectedEmployee.Id);
        filteredSupervisors = EmployeeSearch.Filter(visibleSupervisors, supervisorSearch);
        if (selectedSupervisor is not null && !visibleSupervisors.Any(s => s.Id == selectedSupervisor.Id))
        {
            selectedSupervisor = null;
        }

        employeeSelection = selectedEmployee is null ? [] : [selectedEmployee];
        supervisorSelection = selectedSupervisor is null ? [] : [selectedSupervisor];
    }

    private async Task AddEmployeeAsync()
    {
        var result = await DialogService.OpenAsync<AddEmployeeDialog>("Nowy pracownik",
            options: new DialogOptions { Width = "560px" });
        if (result is not Employee created)
        {
            return;
        }

        selectedEmployee = created;
        selectedSupervisor = null;
        employeeSearch = null;
        await ReloadAsync();
        NotificationService.Notify(NotificationSeverity.Success, "Dodano pracownika", created.FullName);
    }

    private async Task AssignSupervisorAsync()
    {
        if (selectedEmployee is null)
        {
            return;
        }

        var employee = selectedEmployee;
        var result = await DialogService.OpenAsync<AssignSupervisorDialog>("Dodaj przełożonego",
            new Dictionary<string, object?>
            {
                [nameof(AssignSupervisorDialog.Employee)] = employee,
                [nameof(AssignSupervisorDialog.Candidates)] = chart.AssignableSupervisorsFor(employee.Id),
            },
            new DialogOptions { Width = "520px" });
        if (result is not true)
        {
            return;
        }

        await ReloadAsync();
        NotificationService.Notify(NotificationSeverity.Success, "Przypisano przełożonego", employee.FullName);
    }

    private async Task RemoveSupervisorAsync(Employee supervisor)
    {
        if (selectedEmployee is null)
        {
            return;
        }

        var employee = selectedEmployee;
        var confirmed = await DialogService.Confirm(
            $"Czy usunąć {supervisor.FullName} z przełożonych pracownika {employee.FullName}?",
            "Usuń przypisanie",
            new ConfirmOptions { OkButtonText = "Usuń", CancelButtonText = "Anuluj" });
        if (confirmed != true)
        {
            return;
        }

        await Organization.RemoveSupervisorAsync(employee.Id, supervisor.Id);
        await ReloadAsync();
        NotificationService.Notify(NotificationSeverity.Info, "Usunięto przypisanie", $"{supervisor.FullName} – {employee.FullName}");
    }
}
