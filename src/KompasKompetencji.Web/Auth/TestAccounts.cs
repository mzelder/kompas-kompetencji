namespace KompasKompetencji.Web.Auth;

public sealed record TestAccount(string Login, string Password, string Role);

public static class TestAccounts
{
    public static readonly IReadOnlyList<TestAccount> All =
    [
        new("przelozony", "haslo123", AppRoles.Supervisor),
        new("hr", "haslo123", AppRoles.Hr),
        new("admin", "haslo123", AppRoles.Admin),
    ];
}
