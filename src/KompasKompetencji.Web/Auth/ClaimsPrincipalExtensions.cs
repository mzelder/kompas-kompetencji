using System.Security.Claims;

namespace KompasKompetencji.Web.Auth;

public static class ClaimsPrincipalExtensions
{
    public static string? GetRole(this ClaimsPrincipal user) => user.FindFirstValue(ClaimTypes.Role);
}
