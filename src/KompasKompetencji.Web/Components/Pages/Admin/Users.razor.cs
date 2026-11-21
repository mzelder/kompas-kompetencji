using KompasKompetencji.Web.Auth;
using KompasKompetencji.Web.Data;
using Microsoft.AspNetCore.Components;

namespace KompasKompetencji.Web.Components.Pages.Admin;

public partial class Users
{
    [Inject] private UserService UserService { get; set; } = default!;

    private List<AppUser>? users;

    protected override async Task OnInitializedAsync()
    {
        users = await UserService.GetAllAsync();
    }
}
