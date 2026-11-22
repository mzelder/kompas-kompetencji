using KompasKompetencji.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace KompasKompetencji.Web.Demo;

public static class DemoDatabase
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

    public static async Task ResetAsync(IDbContextFactory<AppDbContext> factory)
    {
        await using var db = await factory.CreateDbContextAsync();
        if (await db.Database.CanConnectAsync())
        {
            await db.Database.ExecuteSqlRawAsync(DropAllTablesSql);
        }
        await db.Database.MigrateAsync();
    }
}
