using Microsoft.AspNetCore.Components.Routing;

namespace KompasKompetencji.Web.Auth;

public sealed record NavItem(string Text, string Path, string Icon, NavLinkMatch Match = NavLinkMatch.Prefix);

public sealed record NavGroup(string Title, IReadOnlyList<NavItem> Items);

public static class RoleNavigation
{
    public static IReadOnlyList<NavGroup> For(string? role) => role switch
    {
        AppRoles.Supervisor =>
        [
            new("Oceny",
            [
                new("Pulpit", "/przelozony", "dashboard", NavLinkMatch.All),
                new("Moi pracownicy", "/przelozony/pracownicy", "groups"),
                new("Oceny", "/przelozony/oceny", "assignment"),
            ]),
            new("Wsparcie",
            [
                new("Przebieg oceny", "/przelozony/przebieg-oceny", "route"),
                new("Model kompetencji", "/przelozony/model-kompetencji", "psychology"),
                new("Zasady i materiały", "/przelozony/zasady", "menu_book"),
            ]),
        ],
        AppRoles.Hr =>
        [
            new("Procesy",
            [
                new("Procesy ocen", "/hr", "fact_check", NavLinkMatch.All),
                new("Etapy ocen", "/hr/etapy-ocen", "timeline"),
            ]),
            new("Organizacja", [new("Pracownicy i przełożeni", "/hr/pracownicy", "account_tree")]),
        ],
        AppRoles.Admin =>
        [
            new("Administracja", [new("Użytkownicy", "/admin", "manage_accounts", NavLinkMatch.All)]),
        ],
        _ => [],
    };
}
