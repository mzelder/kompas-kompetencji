using KompasKompetencji.Web.Auth;
using Microsoft.AspNetCore.Components;

namespace KompasKompetencji.Web.Components.Shared;

public partial class RedirectToLogin
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    protected override void OnInitialized()
    {
        var returnUrl = "/" + Navigation.ToBaseRelativePath(Navigation.Uri);
        var target = returnUrl == "/"
            ? AuthEndpoints.LoginPagePath
            : $"{AuthEndpoints.LoginPagePath}?returnUrl={Uri.EscapeDataString(returnUrl)}";
        Navigation.NavigateTo(target, forceLoad: true);
    }
}
