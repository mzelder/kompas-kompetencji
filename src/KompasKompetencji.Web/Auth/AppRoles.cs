namespace KompasKompetencji.Web.Auth;

public static class AppRoles
{
    public const string Supervisor = "Przelozony";
    public const string Hr = "HR";
    public const string Admin = "Admin";

    public static readonly IReadOnlyList<string> All = [Supervisor, Hr, Admin];

    public static bool IsValid(string? role) => role is not null && All.Contains(role);

    public static string DisplayName(string? role) => role switch
    {
        Supervisor => "Przełożony",
        Hr => "HR",
        Admin => "Administrator",
        _ => "Brak roli",
    };

    public static string Icon(string? role) => role switch
    {
        Supervisor => "supervisor_account",
        Hr => "badge",
        Admin => "admin_panel_settings",
        _ => "person",
    };

    public static string HomePath(string? role) => role switch
    {
        Supervisor => "/przelozony",
        Hr => "/hr/etapy-ocen",
        Admin => "/admin",
        _ => "/brak-dostepu",
    };
}
