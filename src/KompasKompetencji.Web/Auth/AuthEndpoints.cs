using System.Security.Claims;
using KompasKompetencji.Web.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace KompasKompetencji.Web.Auth;

public static class AuthEndpoints
{
    public const string SignInPath = "/auth/signin";
    public const string LogoutPath = "/auth/logout";
    public const string LoginPagePath = "/login";

    public static string BuildSignInUrl(string ticket, string? returnUrl)
    {
        var url = $"{SignInPath}?ticket={Uri.EscapeDataString(ticket)}";
        return IsLocalUrl(returnUrl) ? $"{url}&returnUrl={Uri.EscapeDataString(returnUrl!)}" : url;
    }

    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(SignInPath, async (string? ticket, string? returnUrl, HttpContext http,
            LoginTicketStore tickets, UserService users) =>
        {
            var userId = string.IsNullOrEmpty(ticket) ? null : tickets.Redeem(ticket);
            var user = userId is null ? null : await users.GetByIdAsync(userId.Value);
            if (user is null)
            {
                return Results.LocalRedirect($"{LoginPagePath}?error=ticket");
            }

            await http.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, CreatePrincipal(user));
            return Results.LocalRedirect(IsLocalUrl(returnUrl) ? returnUrl! : AppRoles.HomePath(user.Role));
        }).AllowAnonymous();

        app.MapGet(LogoutPath, async (HttpContext http) =>
        {
            await http.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.LocalRedirect(LoginPagePath);
        });

        return app;
    }

    private static ClaimsPrincipal CreatePrincipal(AppUser user)
    {
        Claim[] claims =
        [
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Login),
            new(ClaimTypes.Role, user.Role),
        ];
        return new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
    }

    private static bool IsLocalUrl(string? url) =>
        !string.IsNullOrEmpty(url)
        && url[0] == '/'
        && (url.Length == 1 || (url[1] != '/' && url[1] != '\\'));
}
