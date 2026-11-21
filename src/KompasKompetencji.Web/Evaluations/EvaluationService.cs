using KompasKompetencji.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace KompasKompetencji.Web.Evaluations;

public sealed record EvaluationConversation(Employee Employee, DateTime Start, DateTime End, EvaluationStatus Status)
{
    public string Title => $"{Employee.FullName} · rozmowa okresowa";
}

public sealed record SupervisorOverview(IReadOnlyList<EvaluationTask> Tasks, IReadOnlyList<EvaluationConversation> Conversations)
{
    public IReadOnlyList<EvaluationTask> WithUrgency(params EvaluationUrgency[] urgencies) =>
        Tasks.Where(t => urgencies.Contains(t.Urgency)).ToList();
}

public sealed class EvaluationService(IDbContextFactory<AppDbContext> dbFactory)
{
    public static readonly TimeSpan ConversationLength = TimeSpan.FromHours(1);

    public async Task<SupervisorOverview> GetSupervisorOverviewAsync(int supervisorId, DateOnly today, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var subordinates = await db.EmployeeSupervisors.AsNoTracking()
            .Where(l => l.SupervisorId == supervisorId)
            .Select(l => l.Employee)
            .ToListAsync(ct);
        var subordinatesById = subordinates.ToDictionary(e => e.Id);
        var subordinateIds = subordinatesById.Keys.ToList();

        var evaluations = await db.Evaluations.AsNoTracking()
            .Where(v => subordinateIds.Contains(v.EmployeeId))
            .ToListAsync(ct);
        var evaluationsByEmployee = evaluations.ToLookup(v => v.EmployeeId);

        var tasks = subordinates
            .Select(e => EvaluationSchedule.Describe(e, evaluationsByEmployee[e.Id], today))
            .OrderBy(t => t.Urgency)
            .ThenBy(t => t.DueDate)
            .ThenBy(t => t.Employee.LastName)
            .ToList();

        var conversations = evaluations
            .Where(v => v.ConversationAt is not null)
            .Select(v => new EvaluationConversation(
                subordinatesById[v.EmployeeId],
                v.ConversationAt!.Value,
                v.ConversationAt.Value + ConversationLength,
                v.Status))
            .OrderBy(c => c.Start)
            .ToList();

        return new SupervisorOverview(tasks, conversations);
    }
}
