using KompasKompetencji.Web.Auth;
using KompasKompetencji.Web.Evaluations;
using KompasKompetencji.Web.Evaluations.Pdf;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace KompasKompetencji.Web.Components.Pages.Przelozony;

public partial class EmployeeEvaluation
{
    [Inject] private UserService Users { get; set; } = default!;
    [Inject] private EvaluationService EvaluationService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    public const string ListPath = "/przelozony/pracownicy";

    public static string PathFor(int employeeId) => $"{ListPath}/{employeeId}";

    [CascadingParameter] private Task<AuthenticationState> AuthState { get; set; } = default!;

    [Parameter] public int EmployeeId { get; set; }

    private readonly DateOnly today = DateOnly.FromDateTime(DateTime.Today);
    private EmployeeEvaluationView? view;
    private int supervisorId;
    private int selectedIndex;
    private bool loading = true;
    private string? loadError;

    protected override async Task OnParametersSetAsync()
    {
        loading = true;
        try
        {
            var userId = (await AuthState).User.GetUserId();
            var user = userId is null ? null : await Users.GetByIdAsync(userId.Value);
            view = null;
            if (user?.EmployeeId is int employeeId)
            {
                supervisorId = employeeId;
                await LoadAsync();
            }
        }
        catch (Exception ex)
        {
            loadError = ex.Message;
        }
        loading = false;
    }

    private async Task LoadAsync()
    {
        view = await EvaluationService.GetEmployeeEvaluationAsync(supervisorId, EmployeeId, today);
        selectedIndex = view is null ? 0 : (int)view.CurrentStage;
    }

    private void BackToList() => Navigation.NavigateTo(ListPath);

    private void DownloadSheet(int evaluationId) =>
        Navigation.NavigateTo(EvaluationPdfEndpoints.SheetUrl(evaluationId), forceLoad: true);
}
