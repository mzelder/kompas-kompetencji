using KompasKompetencji.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace KompasKompetencji.Web.Organization;

public sealed record NewEmployee(string FirstName, string LastName, int JobPositionId, DateOnly EmployedSince);

public sealed record JobPositionOption(JobPosition Position, IReadOnlyList<string> Occupants);

public sealed record OperationResult(string? Error)
{
    public bool Succeeded => Error is null;

    public static OperationResult Success { get; } = new((string?)null);

    public static OperationResult Fail(string error) => new(error);
}

public sealed class OrganizationService(IDbContextFactory<AppDbContext> dbFactory)
{
    public async Task<OrganizationChart> GetChartAsync(CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var employees = await db.Employees.AsNoTracking().ToListAsync(ct);
        var links = await db.EmployeeSupervisors.AsNoTracking().ToListAsync(ct);
        var accountLogins = await db.Users.AsNoTracking()
            .Where(u => u.EmployeeId != null)
            .ToDictionaryAsync(u => u.EmployeeId!.Value, u => u.Login, ct);
        return new OrganizationChart(employees, links, accountLogins);
    }

    public async Task<IReadOnlyList<JobPositionOption>> GetJobPositionsAsync(CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var positions = await db.JobPositions.AsNoTracking()
            .Include(p => p.Employees)
            .OrderBy(p => p.Department).ThenBy(p => p.Unit).ThenBy(p => p.Symbol)
            .ToListAsync(ct);
        return positions
            .Select(p => new JobPositionOption(p, p.Employees.OrderBy(e => e.LastName).Select(e => e.FullName).ToList()))
            .ToList();
    }

    public async Task<Employee> AddEmployeeAsync(NewEmployee input, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var position = await db.JobPositions.SingleAsync(p => p.Id == input.JobPositionId, ct);
        var employee = new Employee
        {
            FirstName = input.FirstName.Trim(),
            LastName = input.LastName.Trim(),
            Position = position.Title,
            Department = position.OrganizationalUnit,
            IsSupervisor = position.ManagesTeam,
            EmployedSince = input.EmployedSince,
            JobPositionId = position.Id,
        };

        db.Employees.Add(employee);
        await db.SaveChangesAsync(ct);
        return employee;
    }

    public async Task<OperationResult> AssignSupervisorsAsync(int employeeId, IReadOnlyCollection<int> supervisorIds, CancellationToken ct = default)
    {
        if (supervisorIds.Count == 0)
        {
            return OperationResult.Fail("Wybierz co najmniej jednego przełożonego.");
        }

        var chart = await GetChartAsync(ct);
        if (chart.Find(employeeId) is null)
        {
            return OperationResult.Fail("Nie znaleziono pracownika.");
        }

        var assignable = chart.AssignableSupervisorsFor(employeeId).Select(s => s.Id).ToHashSet();
        if (!supervisorIds.All(assignable.Contains))
        {
            return OperationResult.Fail("Nie można przypisać wybranego przełożonego. Dane mogły się zmienić, odśwież widok.");
        }

        await using var db = await dbFactory.CreateDbContextAsync(ct);
        db.EmployeeSupervisors.AddRange(supervisorIds.Select(id => new EmployeeSupervisor { EmployeeId = employeeId, SupervisorId = id }));
        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            return OperationResult.Fail("Nie udało się zapisać przypisania. Odśwież widok i spróbuj ponownie.");
        }

        return OperationResult.Success;
    }

    public async Task RemoveSupervisorAsync(int employeeId, int supervisorId, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        await db.EmployeeSupervisors
            .Where(l => l.EmployeeId == employeeId && l.SupervisorId == supervisorId)
            .ExecuteDeleteAsync(ct);
    }
}
