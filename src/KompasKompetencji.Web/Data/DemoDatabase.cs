using KompasKompetencji.Web.Auth;
using Microsoft.EntityFrameworkCore;

namespace KompasKompetencji.Web.Data;

public sealed class DemoDatabase(
    IDbContextFactory<AppDbContext> dbFactory,
    UserService users,
    IWebHostEnvironment environment,
    ILogger<DemoDatabase> logger)
{
    public async Task EnsureReadyAsync(CancellationToken ct = default)
    {
        await using (var db = await dbFactory.CreateDbContextAsync(ct))
        {
            var known = db.Database.GetMigrations().ToHashSet();
            var applied = await db.Database.GetAppliedMigrationsAsync(ct);
            var unknown = applied.Where(m => !known.Contains(m)).ToList();
            if (unknown.Count > 0)
            {
                logger.LogWarning("Baza ma migracje nieznane w kodzie ({Migrations}). Tworzę ją od nowa z danymi testowymi.", string.Join(", ", unknown));
                await db.Database.EnsureDeletedAsync(ct);
            }
            await db.Database.MigrateAsync(ct);
        }
        await users.EnsureTestAccountsAsync(ct);
    }

    public async Task ResetAsync(CancellationToken ct = default)
    {
        if (!environment.IsDevelopment())
        {
            throw new InvalidOperationException("Reset bazy jest dostępny tylko w środowisku deweloperskim.");
        }

        await using (var db = await dbFactory.CreateDbContextAsync(ct))
        {
            await db.Database.EnsureDeletedAsync(ct);
            await db.Database.MigrateAsync(ct);
        }
        await users.EnsureTestAccountsAsync(ct);
        logger.LogInformation("Baza została zresetowana do danych testowych.");
    }
}
