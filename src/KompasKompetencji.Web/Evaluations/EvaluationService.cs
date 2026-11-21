using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Organization;
using Microsoft.EntityFrameworkCore;

namespace KompasKompetencji.Web.Evaluations;

public sealed record EvaluationConversation(Employee Employee, DateTime Start, DateTime End, ConversationStatus Status, bool IsEditable)
{
    public string Title => $"{Employee.FullName} · rozmowa okresowa";
}

public sealed record SupervisorOverview(IReadOnlyList<EvaluationTask> Tasks, IReadOnlyList<EvaluationConversation> Conversations)
{
    public IReadOnlyList<EvaluationTask> WithUrgency(params EvaluationUrgency[] urgencies) =>
        Tasks.Where(t => urgencies.Contains(t.Urgency)).ToList();

    public EvaluationTask? TaskFor(int employeeId) => Tasks.FirstOrDefault(t => t.Employee.Id == employeeId);
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
            .OrderByDescending(t => t.NeedsConversation)
            .ThenBy(t => t.Urgency)
            .ThenBy(t => t.DueDate)
            .ThenBy(t => t.Employee.LastName)
            .ToList();

        var conversations = evaluations
            .Where(v => v.ConversationAt is not null)
            .Select(v => new EvaluationConversation(
                subordinatesById[v.EmployeeId],
                v.ConversationAt!.Value,
                v.ConversationAt.Value + ConversationLength,
                v.ConversationStatus ?? ConversationStatus.Scheduled,
                IsEditable: v.Status != EvaluationStatus.Completed))
            .OrderBy(c => c.Start)
            .ToList();

        return new SupervisorOverview(tasks, conversations);
    }

    public async Task<OperationResult> SaveConversationAsync(
        int supervisorId, int employeeId, DateTime conversationAt, ConversationStatus requestedStatus, DateTime now,
        CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var isSubordinate = await db.EmployeeSupervisors
            .AnyAsync(l => l.SupervisorId == supervisorId && l.EmployeeId == employeeId, ct);
        var employee = isSubordinate ? await db.Employees.SingleOrDefaultAsync(e => e.Id == employeeId, ct) : null;
        if (employee is null)
        {
            return OperationResult.Fail("Nie znaleziono pracownika w Twoim zespole.");
        }

        var evaluations = await db.Evaluations.Where(v => v.EmployeeId == employeeId).ToListAsync(ct);
        var task = EvaluationSchedule.Describe(employee, evaluations, DateOnly.FromDateTime(now));
        if (!task.CanScheduleConversation)
        {
            return OperationResult.Fail("Rozmowę wyznacza się, gdy do terminu oceny zostają mniej niż 2 miesiące.");
        }

        var open = EvaluationSchedule.OpenEvaluation(evaluations);
        var status = ConversationRules.ResolveStatus(conversationAt, open?.ConversationAt, requestedStatus);
        var error = ConversationRules.Validate(conversationAt, open?.ConversationAt, status, task.DueDate, now);
        if (error is not null)
        {
            return OperationResult.Fail(error);
        }

        if (open is null)
        {
            open = new Evaluation
            {
                EmployeeId = employeeId,
                EvaluatorId = supervisorId,
                Sheet = task.Sheet,
                Status = EvaluationStatus.NotStarted,
                PeriodStart = task.DueDate.AddYears(-1),
                PeriodEnd = task.DueDate.AddDays(-1),
                DueDate = task.DueDate,
            };
            db.Evaluations.Add(open);
        }

        open.ConversationAt = conversationAt;
        open.ConversationStatus = status;
        await db.SaveChangesAsync(ct);
        return OperationResult.Success;
    }
}
