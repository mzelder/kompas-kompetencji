using System.Text.RegularExpressions;
using KompasKompetencji.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KompasKompetencji.Web.Auth;

public sealed record RegisterResult(AppUser? User, string? Error)
{
    public bool Succeeded => User is not null;

    public static RegisterResult Success(AppUser user) => new(user, null);

    public static RegisterResult Fail(string error) => new(null, error);
}

public sealed partial class UserService(
    IDbContextFactory<AppDbContext> dbFactory,
    IPasswordHasher<AppUser> passwordHasher)
{
    public const int MinLoginLength = 2;
    public const int MaxLoginLength = 50;
    public const int MinPasswordLength = 4;
    public const string LoginPattern = "^[a-zA-Z0-9._-]+$";

    [GeneratedRegex(LoginPattern)]
    private static partial Regex LoginRegex();

    public static string NormalizeLogin(string login) => login.Trim().ToLowerInvariant();

    public async Task<AppUser?> ValidateCredentialsAsync(string login, string password, CancellationToken ct = default)
    {
        var user = await FindByLoginAsync(login, ct);
        if (user is null)
        {
            return null;
        }

        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Failed ? null : user;
    }

    public async Task<RegisterResult> RegisterAsync(string login, string password, string role, CancellationToken ct = default)
    {
        var normalized = NormalizeLogin(login);

        if (normalized.Length is < MinLoginLength or > MaxLoginLength)
        {
            return RegisterResult.Fail($"Login musi mieć od {MinLoginLength} do {MaxLoginLength} znaków.");
        }
        if (!LoginRegex().IsMatch(normalized))
        {
            return RegisterResult.Fail("Login może zawierać tylko litery, cyfry oraz znaki . _ -");
        }
        if (password.Length < MinPasswordLength)
        {
            return RegisterResult.Fail($"Hasło musi mieć co najmniej {MinPasswordLength} znaki.");
        }
        if (!AppRoles.IsValid(role))
        {
            return RegisterResult.Fail("Nieznana rola.");
        }

        await using var db = await dbFactory.CreateDbContextAsync(ct);
        if (await db.Users.AnyAsync(u => u.Login == normalized, ct))
        {
            return RegisterResult.Fail("Ten login jest już zajęty.");
        }

        var user = new AppUser
        {
            Login = normalized,
            Role = role,
            PasswordHash = string.Empty,
            CreatedAt = DateTime.UtcNow,
        };
        user.PasswordHash = passwordHasher.HashPassword(user, password);
        db.Users.Add(user);

        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            return RegisterResult.Fail("Ten login jest już zajęty.");
        }

        return RegisterResult.Success(user);
    }

    public async Task<AppUser?> FindByLoginAsync(string login, CancellationToken ct = default)
    {
        var normalized = NormalizeLogin(login);
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        return await db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Login == normalized, ct);
    }

    public async Task<AppUser?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        return await db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<List<AppUser>> GetAllAsync(CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        return await db.Users.AsNoTracking().OrderBy(u => u.Role).ThenBy(u => u.Login).ToListAsync(ct);
    }

    public async Task EnsureTestAccountsAsync(CancellationToken ct = default)
    {
        foreach (var account in TestAccounts.All)
        {
            if (await FindByLoginAsync(account.Login, ct) is null)
            {
                await RegisterAsync(account.Login, account.Password, account.Role, ct);
            }
        }
    }
}
