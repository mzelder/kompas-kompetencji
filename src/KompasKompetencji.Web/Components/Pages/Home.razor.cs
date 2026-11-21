using KompasKompetencji.Web.Auth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace KompasKompetencji.Web.Components.Pages;

public partial class Home
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    [CascadingParameter] private Task<AuthenticationState> AuthState { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        Navigation.NavigateTo(AppRoles.HomePath(user.GetRole()), replace: true);
    }
}
