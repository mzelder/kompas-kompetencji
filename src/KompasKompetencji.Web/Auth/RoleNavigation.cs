namespace KompasKompetencji.Web.Auth;

public sealed record NavItem(string Text, string Path, string Icon);

public static class RoleNavigation
{
    public static IReadOnlyList<NavItem> For(string? role) => role switch
    {
        AppRoles.Supervisor =>
        [
            new("Moi pracownicy", "/przelozony", "groups"),
        ],
        AppRoles.Hr =>
        [
            new("Procesy ocen", "/hr", "fact_check"),
        ],
        AppRoles.Admin =>
        [
            new("Użytkownicy", "/admin", "manage_accounts"),
        ],
        _ => [],
    };
}
