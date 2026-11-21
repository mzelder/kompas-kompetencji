using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using KompasKompetencji.Web.Organization;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace KompasKompetencji.Web.Components.Shared;

public partial class EmployeeStageList
{
    private IReadOnlyList<EmployeeEvaluationRow> filteredRows = [];
    private string? search;

    [Parameter, EditorRequired] public IReadOnlyList<EmployeeEvaluationRow> Rows { get; set; } = [];

    [Parameter, EditorRequired] public DateOnly Today { get; set; }

    [Parameter] public bool Loading { get; set; }

    [Parameter] public string CountLabel { get; set; } = "Pracownicy";

    [Parameter] public string EmptyText { get; set; } = "Brak pracowników.";

    [Parameter] public EventCallback<Employee> OnEmployeeClick { get; set; }

    [Parameter] public RenderFragment<EmployeeEvaluationRow>? NextStep { get; set; }

    private string CountCaption => filteredRows.Count == Rows.Count
        ? $"{CountLabel}: {Rows.Count}"
        : $"Wyniki wyszukiwania: {filteredRows.Count} z {Rows.Count}";

    protected override void OnParametersSet() => ApplyFilter();

    private async Task OnRowClick(DataGridRowMouseEventArgs<EmployeeEvaluationRow> args)
    {
        if (args.Data is EmployeeEvaluationRow row)
        {
            await OnEmployeeClick.InvokeAsync(row.Employee);
        }
    }

    private void OnSearchChanged(string value)
    {
        search = value;
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        var matching = EmployeeSearch.Filter(Rows.Select(r => r.Employee).ToList(), search).ToHashSet();
        filteredRows = Rows.Where(r => matching.Contains(r.Employee)).ToList();
    }
}
