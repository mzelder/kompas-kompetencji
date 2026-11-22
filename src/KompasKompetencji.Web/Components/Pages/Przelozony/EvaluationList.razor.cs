using KompasKompetencji.Web.Auth;
using KompasKompetencji.Web.Evaluations;
using KompasKompetencji.Web.Organization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace KompasKompetencji.Web.Components.Pages.Przelozony;

public partial class EvaluationList
{
    [Inject] private UserService Users { get; set; } = default!;
    [Inject] private EvaluationListService EvaluationListService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    [CascadingParameter] private Task<AuthenticationState> AuthState { get; set; } = default!;

    private readonly DateOnly today = DateOnly.FromDateTime(DateTime.Today);
    private SupervisorEvaluations? list;
    private IReadOnlyList<EvaluationListRow> inProgress = [];
    private IReadOnlyList<EvaluationListRow> completed = [];
    private string? search;
    private bool loading = true;
    private bool accountNotLinked;
    private string? loadError;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var userId = (await AuthState).User.GetUserId();
            var user = userId is null ? null : await Users.GetByIdAsync(userId.Value);
            if (user?.EmployeeId is int supervisorId)
            {
                list = await EvaluationListService.GetAsync(supervisorId);
                ApplyFilter();
            }
            else
            {
                accountNotLinked = true;
            }
        }
        catch (Exception ex)
        {
            loadError = ex.Message;
        }
        loading = false;
    }

    private void OnSearchChanged(string value)
    {
        search = value;
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        if (list is null)
        {
            return;
        }

        inProgress = Filter(list.InProgress);
        completed = Filter(list.Completed);
    }

    private IReadOnlyList<EvaluationListRow> Filter(IReadOnlyList<EvaluationListRow> rows)
    {
        var matching = EmployeeSearch.Filter(rows.Select(r => r.Employee).Distinct().ToList(), search).ToHashSet();
        return rows.Where(r => matching.Contains(r.Employee)).ToList();
    }

    private void OnRowClick(DataGridRowMouseEventArgs<EvaluationListRow> args)
    {
        if (args.Data is EvaluationListRow row)
        {
            Navigation.NavigateTo(EmployeeEvaluation.PathFor(row.Employee.Id));
        }
    }
}
