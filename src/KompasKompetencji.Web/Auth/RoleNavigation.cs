namespace KompasKompetencji.Web.Auth;

public sealed record NavItem(string Text, string Path, string Icon);

public sealed record NavGroup(string Title, IReadOnlyList<NavItem> Items);

public static class RoleNavigation
{
    public static IReadOnlyList<NavGroup> For(string? role) => role switch
    {
        AppRoles.Supervisor =>
        [
            new("Oceny", [new("Moi pracownicy", "/przelozony", "groups")]),
        ],
        AppRoles.Hr =>
        [
            new("Procesy", [new("Procesy ocen", "/hr", "fact_check")]),
            new("Organizacja", [new("Pracownicy i przełożeni", "/hr/pracownicy", "account_tree")]),
        ],
        AppRoles.Admin =>
        [
            new("Administracja", [new("Użytkownicy", "/admin", "manage_accounts")]),
        ],
        _ => [],
    };
}
