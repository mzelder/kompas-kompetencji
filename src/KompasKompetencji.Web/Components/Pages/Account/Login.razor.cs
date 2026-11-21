using KompasKompetencji.Web.Auth;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace KompasKompetencji.Web.Components.Pages.Account;

public partial class Login
{
    [Inject] private UserService Users { get; set; } = default!;
    [Inject] private LoginTicketStore Tickets { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IWebHostEnvironment Environment { get; set; } = default!;

    [SupplyParameterFromQuery] public string? ReturnUrl { get; set; }

    [SupplyParameterFromQuery(Name = "error")] public string? ErrorCode { get; set; }

    private readonly LoginModel loginModel = new();
    private readonly RegisterModel registerModel = new();
    private readonly List<RoleOption> roleOptions =
        AppRoles.All.Select(r => new RoleOption(r, AppRoles.DisplayName(r))).ToList();

    private int selectedTab;
    private bool busy;
    private string? errorMessage;

    protected override void OnInitialized()
    {
        if (ErrorCode == "ticket")
        {
            errorMessage = "Logowanie wygasło. Spróbuj ponownie.";
        }
    }

    private async Task SignInAsync(LoginModel model)
    {
        errorMessage = null;
        busy = true;
        var user = await Users.ValidateCredentialsAsync(model.Login, model.Password);
        busy = false;

        if (user is null)
        {
            errorMessage = "Nieprawidłowy login lub hasło.";
            return;
        }
        CompleteSignIn(user.Id, ReturnUrl);
    }

    private async Task RegisterAsync(RegisterModel model)
    {
        errorMessage = null;
        busy = true;
        var result = await Users.RegisterAsync(model.Login, model.Password, model.Role);
        busy = false;

        if (!result.Succeeded)
        {
            errorMessage = result.Error;
            return;
        }
        CompleteSignIn(result.User!.Id, returnUrl: null);
    }

    private async Task SignInAsTestAccountAsync(TestAccount account)
    {
        if (!Environment.IsDevelopment())
        {
            return;
        }

        errorMessage = null;
        busy = true;
        var user = await Users.FindByLoginAsync(account.Login);
        busy = false;

        if (user is null)
        {
            errorMessage = $"Konto testowe „{account.Login}” nie istnieje w bazie. Uruchom aplikację ponownie, aby je utworzyć.";
            return;
        }
        CompleteSignIn(user.Id, returnUrl: null);
    }

    private void CompleteSignIn(int userId, string? returnUrl)
    {
        var ticket = Tickets.Issue(userId);
        Navigation.NavigateTo(AuthEndpoints.BuildSignInUrl(ticket, returnUrl), forceLoad: true);
    }

    private sealed class LoginModel
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    private sealed class RegisterModel
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = AppRoles.Supervisor;
    }

    private sealed record RoleOption(string Value, string Text);
}
