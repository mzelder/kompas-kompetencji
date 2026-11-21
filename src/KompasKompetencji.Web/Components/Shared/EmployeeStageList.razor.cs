using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using KompasKompetencji.Web.Organization;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace KompasKompetencji.Web.Components.Shared;

public partial class EmployeeStageList
{
    private IReadOnlyList<Employee> filteredEmployees = [];
    private string? search;

    [Parameter, EditorRequired] public IReadOnlyList<Employee> Employees { get; set; } = [];

    [Parameter, EditorRequired] public IReadOnlyDictionary<int, EvaluationStage> Stages { get; set; } = default!;

    [Parameter] public bool Loading { get; set; }

    [Parameter] public string CountLabel { get; set; } = "Pracownicy";

    [Parameter] public string EmptyText { get; set; } = "Brak pracowników.";

    [Parameter] public EventCallback<Employee> OnEmployeeClick { get; set; }

    [Parameter] public RenderFragment<Employee>? StageDetails { get; set; }

    private string CountCaption => filteredEmployees.Count == Employees.Count
        ? $"{CountLabel}: {Employees.Count}"
        : $"Wyniki wyszukiwania: {filteredEmployees.Count} z {Employees.Count}";

    protected override void OnParametersSet()
    {
        filteredEmployees = EmployeeSearch.Filter(Employees, search);
    }

    private async Task OnRowClick(DataGridRowMouseEventArgs<Employee> args)
    {
        if (args.Data is Employee employee)
        {
            await OnEmployeeClick.InvokeAsync(employee);
        }
    }

    private void OnSearchChanged(string value)
    {
        search = value;
        filteredEmployees = EmployeeSearch.Filter(Employees, search);
    }
}
