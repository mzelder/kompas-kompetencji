using KompasKompetencji.Web.Auth;
using KompasKompetencji.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace KompasKompetencji.Web.Demo;

public sealed class DemoDatabase(
    IDbContextFactory<AppDbContext> dbFactory,
    UserService users,
    ILogger<DemoDatabase> logger)
{
    private const string DropAllTablesSql = """
        DECLARE @sql nvarchar(max) = N'';
        SELECT @sql += N'ALTER TABLE ' + QUOTENAME(s.name) + N'.' + QUOTENAME(t.name) + N' DROP CONSTRAINT ' + QUOTENAME(f.name) + N';'
        FROM sys.foreign_keys f
        JOIN sys.tables t ON f.parent_object_id = t.object_id
        JOIN sys.schemas s ON t.schema_id = s.schema_id;
        SELECT @sql += N'DROP TABLE ' + QUOTENAME(s.name) + N'.' + QUOTENAME(t.name) + N';'
        FROM sys.tables t
        JOIN sys.schemas s ON t.schema_id = s.schema_id;
        EXEC sp_executesql @sql;
        """;

    public async Task EnsureReadyAsync(CancellationToken ct = default)
    {
        await using (var db = await dbFactory.CreateDbContextAsync(ct))
        {
            if (await db.Database.CanConnectAsync(ct))
            {
                var known = db.Database.GetMigrations().ToHashSet();
                var unknown = (await db.Database.GetAppliedMigrationsAsync(ct)).Where(m => !known.Contains(m)).ToList();
                if (unknown.Count > 0)
                {
                    logger.LogWarning("Baza ma migracje nieznane w kodzie ({Migrations}). Tworzę tabele od nowa z danymi testowymi.", string.Join(", ", unknown));
                    await db.Database.ExecuteSqlRawAsync(DropAllTablesSql, ct);
                }
            }
            await db.Database.MigrateAsync(ct);
        }
        await users.EnsureTestAccountsAsync(ct);
    }

    public async Task ResetAsync(CancellationToken ct = default)
    {
        await using (var db = await dbFactory.CreateDbContextAsync(ct))
        {
            if (await db.Database.CanConnectAsync(ct))
            {
                await db.Database.ExecuteSqlRawAsync(DropAllTablesSql, ct);
            }
            await db.Database.MigrateAsync(ct);
        }
        await users.EnsureTestAccountsAsync(ct);
        logger.LogInformation("Baza została zresetowana do danych testowych.");
    }
}
