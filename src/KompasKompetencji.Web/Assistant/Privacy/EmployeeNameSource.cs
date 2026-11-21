using KompasKompetencji.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace KompasKompetencji.Web.Assistant.Privacy;

public sealed class EmployeeNameSource(IDbContextFactory<AppDbContext> dbFactory)
{
    public async Task<IReadOnlyList<PersonName>> GetAsync(CancellationToken cancellationToken)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        return await db.Employees
            .AsNoTracking()
            .Select(e => new PersonName(e.Id, e.FirstName, e.LastName))
            .ToListAsync(cancellationToken);
    }
}
