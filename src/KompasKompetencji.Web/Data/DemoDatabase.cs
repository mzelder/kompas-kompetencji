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
            logger.LogWarning("Usuwam bazę i tworzę ją od nowa z danymi testowymi.");
            await db.Database.EnsureDeletedAsync(ct);
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
