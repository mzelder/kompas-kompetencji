using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Organization;
using Microsoft.EntityFrameworkCore;

namespace KompasKompetencji.Web.Evaluations;

public sealed record EvaluationConversation(Employee Employee, DateTime Start, DateTime End, ConversationStatus Status, bool IsEditable)
{
    public string Title => $"{Employee.FullName} · rozmowa okresowa";
}

public sealed record SupervisorOverview(IReadOnlyList<EvaluationTask> Tasks, IReadOnlyList<EvaluationConversation> Conversations, DateTime Now)
{
    public bool IsConversationPastDue(EvaluationTask task) =>
        ConversationRules.IsPastDue(task.ConversationAt, task.ConversationStatus, Now);

    public IReadOnlyList<EvaluationTask> WithUrgency(params EvaluationUrgency[] urgencies) =>
        Tasks.Where(t => urgencies.Contains(t.Urgency)).ToList();

    public EvaluationTask? TaskFor(int employeeId) => Tasks.FirstOrDefault(t => t.Employee.Id == employeeId);

    public IReadOnlyList<EvaluationTask> ToDo => Tasks
        .Where(t => IsConversationPastDue(t) || t.NeedsAction)
        .OrderByDescending(IsConversationPastDue)
        .ToList();
}

public sealed record EmployeeEvaluationView(
    Employee Employee,
    JobDescription? JobDescription,
    EvaluationTask Task,
    Evaluation? OpenEvaluation,
    EvaluationStage CurrentStage,
    EvaluationSheet SuggestedSheet,
    string SuggestionReason,
    IReadOnlyList<EvaluationNote> Notes);

public sealed class EvaluationService(IDbContextFactory<AppDbContext> dbFactory)
{
    public static readonly TimeSpan ConversationLength = TimeSpan.FromHours(1);

    public async Task<SupervisorOverview> GetSupervisorOverviewAsync(int supervisorId, DateTime now, CancellationToken ct = default)
    {
        var today = DateOnly.FromDateTime(now);
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
            .OrderByDescending(t => t.NeedsAction)
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

        return new SupervisorOverview(tasks, conversations, now);
    }

    public async Task<OperationResult> SaveConversationAsync(
        int supervisorId, int employeeId, DateTime conversationAt, ConversationStatus requestedStatus, DateTime now,
        CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var employee = await FindSubordinateAsync(db, supervisorId, employeeId, ct);
        if (employee is null)
        {
            return OperationResult.Fail("Nie znaleziono pracownika w Twoim zespole.");
        }

        var evaluations = await db.Evaluations.Where(v => v.EmployeeId == employeeId).ToListAsync(ct);
        var task = EvaluationSchedule.Describe(employee, evaluations, DateOnly.FromDateTime(now));
        var open = EvaluationSchedule.OpenEvaluation(evaluations);
        if (open?.SheetConfirmedAt is null)
        {
            return OperationResult.Fail("Najpierw zatwierdź arkusz oceny na stronie pracownika.");
        }

        if (!task.IsInConversationWindow)
        {
            return OperationResult.Fail("Rozmowę wyznacza się, gdy do terminu oceny zostają mniej niż 2 miesiące.");
        }

        var error = ConversationRules.Validate(conversationAt, open.ConversationAt, requestedStatus, task.DueDate, now);
        if (error is not null)
        {
            return OperationResult.Fail(error);
        }

        open.ConversationAt = conversationAt;
        open.ConversationStatus = requestedStatus;
        await db.SaveChangesAsync(ct);
        return OperationResult.Success;
    }

    public async Task<IReadOnlyDictionary<int, EvaluationStage>> GetCurrentStagesAsync(
        IReadOnlyCollection<int> employeeIds, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var evaluations = await db.Evaluations.AsNoTracking()
            .Where(v => employeeIds.Contains(v.EmployeeId))
            .ToListAsync(ct);
        var evaluationsByEmployee = evaluations.ToLookup(v => v.EmployeeId);

        return employeeIds.ToDictionary(
            id => id,
            id => EvaluationStages.Current(EvaluationSchedule.OpenEvaluation(evaluationsByEmployee[id])));
    }

    public async Task<EmployeeEvaluationView?> GetEmployeeEvaluationAsync(
        int supervisorId, int employeeId, DateOnly today, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var employee = await FindSubordinateAsync(db, supervisorId, employeeId, ct);
        if (employee is null)
        {
            return null;
        }

        var jobDescription = await db.JobDescriptions.AsNoTracking().SingleOrDefaultAsync(j => j.EmployeeId == employeeId, ct);
        var evaluations = await db.Evaluations.AsNoTracking().Where(v => v.EmployeeId == employeeId).ToListAsync(ct);
        var open = EvaluationSchedule.OpenEvaluation(evaluations);
        var notes = open is null
            ? []
            : await db.EvaluationNotes.AsNoTracking()
                .Where(n => n.EvaluationId == open.Id)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync(ct);

        return new EmployeeEvaluationView(
            employee,
            jobDescription,
            EvaluationSchedule.Describe(employee, evaluations, today),
            open,
            EvaluationStages.Current(open),
            EvaluationSchedule.SheetFor(employee),
            EvaluationText.SheetSuggestionReason(employee),
            notes);
    }

    public async Task<OperationResult> AddNoteAsync(
        int supervisorId, int employeeId, string text, DateTime now, CancellationToken ct = default)
    {
        var trimmed = text.Trim();
        if (trimmed.Length == 0)
        {
            return OperationResult.Fail("Wpisz treść notatki.");
        }

        if (trimmed.Length > EvaluationNote.MaxTextLength)
        {
            return OperationResult.Fail($"Notatka może mieć najwyżej {EvaluationNote.MaxTextLength} znaków.");
        }

        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var open = await FindAssessmentEvaluationAsync(db, supervisorId, employeeId, ct);
        if (open is null)
        {
            return OperationResult.Fail("Notatki dodaje się na etapie oceniania pracownika z Twojego zespołu.");
        }

        db.EvaluationNotes.Add(new EvaluationNote { EvaluationId = open.Id, CreatedAt = now, Text = trimmed });
        await db.SaveChangesAsync(ct);
        return OperationResult.Success;
    }

    public async Task<OperationResult> DeleteNoteAsync(
        int supervisorId, int employeeId, int noteId, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var open = await FindAssessmentEvaluationAsync(db, supervisorId, employeeId, ct);
        var note = open is null
            ? null
            : await db.EvaluationNotes.SingleOrDefaultAsync(n => n.Id == noteId && n.EvaluationId == open.Id, ct);
        if (note is null)
        {
            return OperationResult.Fail("Nie znaleziono notatki.");
        }

        db.EvaluationNotes.Remove(note);
        await db.SaveChangesAsync(ct);
        return OperationResult.Success;
    }

    public async Task<OperationResult> ConfirmSheetAsync(
        int supervisorId, int employeeId, EvaluationSheet sheet, DateTime now, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var employee = await FindSubordinateAsync(db, supervisorId, employeeId, ct);
        if (employee is null)
        {
            return OperationResult.Fail("Nie znaleziono pracownika w Twoim zespole.");
        }

        var evaluations = await db.Evaluations.Where(v => v.EmployeeId == employeeId).ToListAsync(ct);
        var open = EvaluationSchedule.OpenEvaluation(evaluations);
        if (open?.SheetConfirmedAt is not null)
        {
            return OperationResult.Fail("Arkusz oceny jest już zatwierdzony.");
        }

        if (open is null)
        {
            var task = EvaluationSchedule.Describe(employee, evaluations, DateOnly.FromDateTime(now));
            open = CreateOpenEvaluation(employeeId, supervisorId, task);
            db.Evaluations.Add(open);
        }

        open.Sheet = sheet;
        open.SheetConfirmedAt = now;
        await db.SaveChangesAsync(ct);
        return OperationResult.Success;
    }

    private static async Task<Evaluation?> FindAssessmentEvaluationAsync(
        AppDbContext db, int supervisorId, int employeeId, CancellationToken ct)
    {
        if (await FindSubordinateAsync(db, supervisorId, employeeId, ct) is null)
        {
            return null;
        }

        var evaluations = await db.Evaluations.AsNoTracking().Where(v => v.EmployeeId == employeeId).ToListAsync(ct);
        var open = EvaluationSchedule.OpenEvaluation(evaluations);
        return EvaluationStages.Current(open) == EvaluationStage.Assessment ? open : null;
    }

    private static async Task<Employee?> FindSubordinateAsync(AppDbContext db, int supervisorId, int employeeId, CancellationToken ct)
    {
        var isSubordinate = await db.EmployeeSupervisors
            .AnyAsync(l => l.SupervisorId == supervisorId && l.EmployeeId == employeeId, ct);
        return isSubordinate ? await db.Employees.SingleOrDefaultAsync(e => e.Id == employeeId, ct) : null;
    }

    private static Evaluation CreateOpenEvaluation(int employeeId, int supervisorId, EvaluationTask task) => new()
    {
        EmployeeId = employeeId,
        EvaluatorId = supervisorId,
        Sheet = task.Sheet,
        Status = EvaluationStatus.NotStarted,
        PeriodStart = task.DueDate.AddYears(-1),
        PeriodEnd = task.DueDate.AddDays(-1),
        DueDate = task.DueDate,
    };
}
