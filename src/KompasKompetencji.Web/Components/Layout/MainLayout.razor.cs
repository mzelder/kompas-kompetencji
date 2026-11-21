using KompasKompetencji.Web.Auth;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace KompasKompetencji.Web.Components.Layout;

public partial class MainLayout
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private const string LogoutValue = "logout";

    private bool sidebarExpanded = true;

    private void OnProfileMenuClick(RadzenProfileMenuItem item)
    {
        if (item.Value == LogoutValue)
        {
            Navigation.NavigateTo(AuthEndpoints.LogoutPath, forceLoad: true);
        }
    }
}
