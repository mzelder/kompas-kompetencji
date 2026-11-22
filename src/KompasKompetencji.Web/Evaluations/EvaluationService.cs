using KompasKompetencji.Web.Assistant;
using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations.Pdf;
using KompasKompetencji.Web.Organization;
using Microsoft.EntityFrameworkCore;

namespace KompasKompetencji.Web.Evaluations;

public sealed record EvaluationConversation(
    Employee Employee, ConversationType Type, DateTime Start, DateTime End, ConversationStatus Status, bool IsEditable)
{
    public string Title => $"{Employee.FullName} · {EvaluationText.ConversationTypeName(Type)}";
}

public sealed record SupervisorOverview(IReadOnlyList<EvaluationTask> Tasks, IReadOnlyList<EvaluationConversation> Conversations, DateTime Now)
{
    public ConversationType? PastDueConversation(EvaluationTask task) => task.PastDueConversation(Now);

    public bool IsConversationPastDue(EvaluationTask task) => PastDueConversation(task) is not null;

    public IReadOnlyList<EvaluationTask> WithUrgency(params EvaluationUrgency[] urgencies) =>
        Tasks.Where(t => urgencies.Contains(t.Urgency)).ToList();

    public EvaluationTask? TaskFor(int employeeId) => Tasks.FirstOrDefault(t => t.Employee.Id == employeeId);

    public IReadOnlyList<EvaluationTask> ToDo => Tasks
        .Where(t => IsConversationPastDue(t) || t.NeedsAction)
        .OrderByDescending(IsConversationPastDue)
        .ThenByDescending(t => t.NeedsStart)
        .ThenBy(t => t.NeedsStart ? t.StartingConversationDeadline : t.DueDate)
        .ToList();
}

public sealed record EmployeeEvaluationRow(Employee Employee, EvaluationStage Stage, EvaluationTask Task)
{
    public string FullName => Employee.FullName;

    public DateOnly DueDate => Task.DueDate;
}

public sealed record PreviousEvaluation(int Id, EvaluationSheet Sheet, DateOnly PreparedOn);

public sealed record CompletionView(
    CompletionStep Step,
    DateOnly? PreparedOn,
    AllowedDates PreparedOnRange,
    AllowedDates AcquaintedOnRange,
    DateOnly? AssessmentConversationDay,
    string? CloseBlocker);

public sealed record EmployeeEvaluationView(
    Employee Employee,
    JobDescription? JobDescription,
    EvaluationTask Task,
    Evaluation? OpenEvaluation,
    EvaluationStage CurrentStage,
    EvaluationSheet SuggestedSheet,
    string SuggestionReason,
    DateOnly CycleStart,
    DateOnly StartingConversationDeadline,
    Conversation? StartingConversation,
    StartingConversationStep StartingConversationStep,
    IReadOnlyList<Criterion> MandatoryCriteria,
    IReadOnlyList<Criterion> AdditionalCriteria,
    IReadOnlyList<SuggestedCriterion> SuggestedCriteria,
    string? Expectations,
    string? Goals,
    DateOnly? EmployeeAcquaintedOn,
    IReadOnlyList<EvaluationNote> Notes,
    CompletionView? Completion,
    PreviousEvaluation? PreviousEvaluation)
{
    public bool IsPartIConfirmed => StartingConversationStep == StartingConversationStep.Confirmed;

    public IReadOnlyList<Criterion> Criteria => [.. MandatoryCriteria, .. AdditionalCriteria];
}

public sealed class EvaluationService(IDbContextFactory<AppDbContext> dbFactory, NoteCriterionClassifier noteClassifier)
{
    public static readonly TimeSpan ConversationLength = TimeSpan.FromHours(1);

    public const int MaxPartITextLength = 4000;
    private const string EmployeeNotFound = "Nie znaleziono pracownika w Twoim zespole.";
    private const string SheetNotConfirmed = "Najpierw zatwierdź arkusz oceny na stronie pracownika.";
    private const string PartIAlreadyConfirmed = "Część I arkusza jest już zatwierdzona.";
    private const string NotInCompletion = "Datę sporządzenia i zapoznania wpisuje się na etapie zakończenia oceny pracownika z Twojego zespołu.";

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
            .Include(v => v.Conversations)
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
            .SelectMany(v => v.Conversations
                .Select(c => new EvaluationConversation(
                    subordinatesById[v.EmployeeId],
                    c.Type,
                    c.At,
                    c.At + ConversationLength,
                    c.Status,
                    IsEditable: IsConversationEditable(v, c.Type))))
            .OrderBy(c => c.Start)
            .ToList();

        return new SupervisorOverview(tasks, conversations, now);
    }

    public async Task<OperationResult> SaveConversationAsync(
        int supervisorId, int employeeId, ConversationType type, DateTime conversationAt, ConversationStatus requestedStatus,
        DateTime now, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var employee = await FindSubordinateAsync(db, supervisorId, employeeId, ct);
        if (employee is null)
        {
            return OperationResult.Fail(EmployeeNotFound);
        }

        var evaluations = await db.Evaluations.Include(v => v.Conversations).Where(v => v.EmployeeId == employeeId).ToListAsync(ct);
        var open = EvaluationSchedule.OpenEvaluation(evaluations);
        if (open?.SheetConfirmedAt is null)
        {
            return OperationResult.Fail(SheetNotConfirmed);
        }

        var conversation = open.FindConversation(type);
        string? error;
        if (type == ConversationType.Starting)
        {
            if (open.PartIConfirmedAt is not null)
            {
                return OperationResult.Fail(PartIAlreadyConfirmed);
            }

            var cycleStart = EvaluationSchedule.CycleStart(employee, evaluations);
            error = StartingConversationRules.Validate(conversationAt, conversation?.At, requestedStatus, cycleStart, now);
        }
        else
        {
            if (open.PartIConfirmedAt is null)
            {
                return OperationResult.Fail("Najpierw zatwierdź część I arkusza na stronie pracownika.");
            }

            var task = EvaluationSchedule.Describe(employee, evaluations, DateOnly.FromDateTime(now));
            if (!task.IsInConversationWindow)
            {
                return OperationResult.Fail("Rozmowę wyznacza się, gdy do terminu oceny zostają mniej niż 2 miesiące.");
            }

            error = ConversationRules.Validate(conversationAt, conversation?.At, requestedStatus, task.DueDate, now);
        }

        if (error is not null)
        {
            return OperationResult.Fail(error);
        }

        if (conversation is null)
        {
            conversation = new Conversation { Type = type };
            open.Conversations.Add(conversation);
        }

        conversation.At = conversationAt;
        conversation.Status = requestedStatus;
        await db.SaveChangesAsync(ct);
        return OperationResult.Success;
    }

    public async Task<OperationResult> SavePartIDraftAsync(
        int supervisorId, int employeeId, IReadOnlyCollection<Criterion> additionalCriteria, string? expectations, string? goals,
        CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var (open, error) = await FindOpenPartIAsync(db, supervisorId, employeeId, ct);
        if (open is null)
        {
            return OperationResult.Fail(error!);
        }

        error = ApplyPartIDraft(open, additionalCriteria, expectations, goals);
        if (error is not null)
        {
            return OperationResult.Fail(error);
        }

        await db.SaveChangesAsync(ct);
        return OperationResult.Success;
    }

    public async Task<OperationResult> ConfirmPartIAsync(
        int supervisorId, int employeeId, IReadOnlyCollection<Criterion> additionalCriteria, string? expectations, string? goals,
        DateOnly acquaintedOn, DateTime now, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var (open, error) = await FindOpenPartIAsync(db, supervisorId, employeeId, ct);
        if (open is null)
        {
            return OperationResult.Fail(error!);
        }

        var startingConversation = open.FindConversation(ConversationType.Starting);
        if (startingConversation?.Status != ConversationStatus.Held)
        {
            return OperationResult.Fail("Część I zatwierdzisz po rozmowie rozpoczynającej oznaczonej jako odbyta.");
        }

        if (string.IsNullOrWhiteSpace(expectations))
        {
            return OperationResult.Fail("Wpisz oczekiwania omówione z pracownikiem.");
        }

        var conversationDay = DateOnly.FromDateTime(startingConversation.At);
        if (acquaintedOn < conversationDay || acquaintedOn > DateOnly.FromDateTime(now))
        {
            return OperationResult.Fail(
                $"Data zapoznania musi przypadać między dniem rozmowy rozpoczynającej ({EvaluationText.Date(conversationDay)}) a dzisiejszym dniem.");
        }

        error = ApplyPartIDraft(open, additionalCriteria, expectations, goals);
        if (error is not null)
        {
            return OperationResult.Fail(error);
        }

        open.PartIConfirmedAt = now;
        open.EmployeeAcquaintedOn = acquaintedOn;
        open.PeriodStart = acquaintedOn;
        open.Status = EvaluationStatus.CriteriaSet;
        await db.SaveChangesAsync(ct);
        return OperationResult.Success;
    }

    public async Task<IReadOnlyList<EmployeeEvaluationRow>> GetEvaluationRowsAsync(
        IReadOnlyList<Employee> employees, DateOnly today, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var employeeIds = employees.Select(e => e.Id).ToList();
        var evaluations = await db.Evaluations.AsNoTracking()
            .Include(v => v.Conversations)
            .Where(v => employeeIds.Contains(v.EmployeeId))
            .ToListAsync(ct);
        var evaluationsByEmployee = evaluations.ToLookup(v => v.EmployeeId);

        return employees
            .Select(e => new EmployeeEvaluationRow(
                e,
                EvaluationStages.Current(EvaluationSchedule.OpenEvaluation(evaluationsByEmployee[e.Id])),
                EvaluationSchedule.Describe(e, evaluationsByEmployee[e.Id], today)))
            .ToList();
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
        var evaluations = await db.Evaluations.AsNoTracking()
            .Include(v => v.Conversations)
            .Include(v => v.AdditionalCriteria)
            .Where(v => v.EmployeeId == employeeId)
            .ToListAsync(ct);
        var open = EvaluationSchedule.OpenEvaluation(evaluations);
        var task = EvaluationSchedule.Describe(employee, evaluations, today);
        var cycleStart = EvaluationSchedule.CycleStart(employee, evaluations);
        var suggestedCriteria = CriteriaSuggestion.Suggest(task.Sheet, jobDescription);
        var additionalCriteria = open is not null && HasPartIDraft(open)
            ? open.AdditionalCriteria.Select(a => a.Criterion).OrderBy(c => c).ToList()
            : suggestedCriteria.Select(s => s.Criterion).ToList();

        var notes = open is null
            ? []
            : await db.EvaluationNotes.AsNoTracking()
                .Where(n => n.EvaluationId == open.Id)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync(ct);

        return new EmployeeEvaluationView(
            employee,
            jobDescription,
            task,
            open,
            EvaluationStages.Current(open),
            EvaluationSchedule.SheetFor(employee),
            EvaluationText.SheetSuggestionReason(employee),
            cycleStart,
            StartingConversationRules.Deadline(cycleStart),
            open?.FindConversation(ConversationType.Starting),
            StartingConversationRules.Step(open),
            EvaluationCriteria.Mandatory(task.Sheet),
            additionalCriteria,
            suggestedCriteria,
            open?.Expectations,
            open?.Goals,
            open?.EmployeeAcquaintedOn,
            notes,
            open is not null && EvaluationStages.Current(open) == EvaluationStage.Completion ? DescribeCompletion(open, today) : null,
            EvaluationSchedule.LastCompleted(evaluations) is { PreparedOn: DateOnly preparedOn } last
                ? new PreviousEvaluation(last.Id, last.Sheet, preparedOn)
                : null);
    }

    public async Task<OperationResult> SavePreparedOnAsync(
        int supervisorId, int employeeId, DateOnly preparedOn, DateTime now, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var open = await FindCompletionEvaluationAsync(db, supervisorId, employeeId, ct);
        if (open is null)
        {
            return OperationResult.Fail(NotInCompletion);
        }

        var error = CompletionRules.ValidatePreparedOn(open, preparedOn, DateOnly.FromDateTime(now));
        if (error is not null)
        {
            return OperationResult.Fail(error);
        }

        open.PreparedOn = preparedOn;
        await db.SaveChangesAsync(ct);
        return OperationResult.Success;
    }

    public async Task<OperationResult> CloseEvaluationAsync(
        int supervisorId, int employeeId, DateOnly acquaintedOn, DateTime now, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var open = await FindCompletionEvaluationAsync(db, supervisorId, employeeId, ct);
        if (open is null)
        {
            return OperationResult.Fail(NotInCompletion);
        }

        var error = CompletionRules.CloseBlocker(open)
            ?? CompletionRules.ValidateAcquaintedOn(open, acquaintedOn, DateOnly.FromDateTime(now));
        if (error is not null)
        {
            return OperationResult.Fail(error);
        }

        open.PartIIAcquaintedOn = acquaintedOn;
        open.PeriodEnd = open.PreparedOn;
        open.CompletedAt = now;
        open.Status = EvaluationStatus.Completed;
        await db.SaveChangesAsync(ct);
        return OperationResult.Success;
    }

    public async Task<EvaluationSheetData?> GetEvaluationSheetAsync(int supervisorId, int evaluationId, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var evaluation = await db.Evaluations.AsNoTracking()
            .Include(v => v.Employee)
            .Include(v => v.Evaluator)
            .Include(v => v.Conversations)
            .Include(v => v.AdditionalCriteria)
            .SingleOrDefaultAsync(v => v.Id == evaluationId, ct);
        if (evaluation?.PreparedOn is null || await FindSubordinateAsync(db, supervisorId, evaluation.EmployeeId, ct) is null)
        {
            return null;
        }

        var isPrintable = evaluation.Status == EvaluationStatus.Completed
            || EvaluationStages.Current(evaluation) == EvaluationStage.Completion;
        if (!isPrintable)
        {
            return null;
        }

        var previous = await db.Evaluations.AsNoTracking()
            .Include(v => v.AdditionalCriteria)
            .Where(v => v.EmployeeId == evaluation.EmployeeId && v.Id != evaluation.Id
                && v.Status == EvaluationStatus.Completed && v.PreparedOn != null && v.DueDate < evaluation.DueDate)
            .OrderByDescending(v => v.DueDate)
            .FirstOrDefaultAsync(ct);
        var evaluationIds = previous is null ? [evaluation.Id] : new[] { evaluation.Id, previous.Id };
        var decisions = await db.CriterionDecisions.AsNoTracking()
            .Where(d => evaluationIds.Contains(d.EvaluationId))
            .ToListAsync(ct);

        var previousSheet = previous is not null && EvaluationSheetData.Summarize(previous, decisions) is WrittenEvaluationSummary summary
            ? new SheetPreviousEvaluation(summary.IsPositive, summary.OverallLevel, previous.PreparedOn!.Value)
            : null;
        return EvaluationSheetData.From(evaluation, decisions, previousSheet);
    }

    private static CompletionView DescribeCompletion(Evaluation open, DateOnly today) => new(
        CompletionRules.Step(open),
        open.PreparedOn,
        CompletionRules.PreparedOnRange(open, today),
        CompletionRules.AcquaintedOnRange(open, today),
        CompletionRules.AssessmentConversationDay(open),
        CompletionRules.CloseBlocker(open));

    private static async Task<Evaluation?> FindCompletionEvaluationAsync(
        AppDbContext db, int supervisorId, int employeeId, CancellationToken ct)
    {
        if (await FindSubordinateAsync(db, supervisorId, employeeId, ct) is null)
        {
            return null;
        }

        var evaluations = await db.Evaluations.Include(v => v.Conversations).Where(v => v.EmployeeId == employeeId).ToListAsync(ct);
        var open = EvaluationSchedule.OpenEvaluation(evaluations);
        return EvaluationStages.Current(open) == EvaluationStage.Completion ? open : null;
    }

    public async Task<(OperationResult Result, NoteClassification? Classification)> AddNoteAsync(
        int supervisorId, int employeeId, string text, DateTime now, CancellationToken ct = default)
    {
        var trimmed = text.Trim();
        if (trimmed.Length == 0)
        {
            return (OperationResult.Fail("Wpisz treść notatki."), null);
        }

        if (trimmed.Length > EvaluationNote.MaxTextLength)
        {
            return (OperationResult.Fail($"Notatka może mieć najwyżej {EvaluationNote.MaxTextLength} znaków."), null);
        }

        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var open = await FindAssessmentEvaluationAsync(db, supervisorId, employeeId, ct);
        if (open is null)
        {
            return (OperationResult.Fail("Notatki dodaje się na etapie oceny pracownika z Twojego zespołu."), null);
        }

        var evaluationCriteria = EvaluationCriteria.Mandatory(open.Sheet)
            .Concat(open.AdditionalCriteria.Select(a => a.Criterion).OrderBy(c => c))
            .ToList();
        var classification = await noteClassifier.ClassifyAsync(trimmed, evaluationCriteria, ct);

        db.EvaluationNotes.Add(new EvaluationNote
        {
            EvaluationId = open.Id,
            Criterion = classification.Criterion,
            Tone = classification.Tone,
            CreatedAt = now,
            Text = trimmed,
        });
        await db.SaveChangesAsync(ct);
        return (OperationResult.Success, classification);
    }

    public async Task<(OperationResult Result, NoteClassification? Reclassified)> UpdateNoteAsync(
        int supervisorId, int employeeId, int noteId, string text, bool manualClassification, Criterion? criterion, NoteTone tone,
        DateTime now, CancellationToken ct = default)
    {
        var trimmed = text.Trim();
        if (trimmed.Length == 0)
        {
            return (OperationResult.Fail("Wpisz treść notatki."), null);
        }

        if (trimmed.Length > EvaluationNote.MaxTextLength)
        {
            return (OperationResult.Fail($"Notatka może mieć najwyżej {EvaluationNote.MaxTextLength} znaków."), null);
        }

        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var open = await FindAssessmentEvaluationAsync(db, supervisorId, employeeId, ct);
        var note = open is null
            ? null
            : await db.EvaluationNotes.SingleOrDefaultAsync(n => n.Id == noteId && n.EvaluationId == open.Id, ct);
        if (open is null || note is null)
        {
            return (OperationResult.Fail("Nie znaleziono notatki."), null);
        }

        var evaluationCriteria = EvaluationCriteria.Mandatory(open.Sheet)
            .Concat(open.AdditionalCriteria.Select(a => a.Criterion).OrderBy(c => c))
            .ToList();
        if (manualClassification && criterion is Criterion selected && !evaluationCriteria.Contains(selected))
        {
            return (OperationResult.Fail("To kryterium nie jest ustalone w tej ocenie. Wybierz kryterium z listy."), null);
        }

        var textChanged = note.Text != trimmed;
        var classificationChanged = manualClassification && (note.Criterion != criterion || note.Tone != tone);
        NoteClassification? reclassified = null;

        if (textChanged)
        {
            note.Text = trimmed;
            note.TextEditedAt = now;
        }

        if (classificationChanged)
        {
            note.Criterion = criterion;
            note.Tone = tone;
            note.ClassificationEditedAt = now;
        }
        else if (textChanged && !manualClassification)
        {
            reclassified = await noteClassifier.ClassifyAsync(trimmed, evaluationCriteria, ct);
            note.Criterion = reclassified.Criterion;
            note.Tone = reclassified.Tone;
            note.ClassificationEditedAt = null;
        }

        await db.SaveChangesAsync(ct);
        return (OperationResult.Success, reclassified);
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
            return OperationResult.Fail(EmployeeNotFound);
        }

        var evaluations = await db.Evaluations.Include(v => v.Conversations).Where(v => v.EmployeeId == employeeId).ToListAsync(ct);
        var open = EvaluationSchedule.OpenEvaluation(evaluations);
        if (open?.SheetConfirmedAt is not null)
        {
            return OperationResult.Fail("Arkusz oceny jest już zatwierdzony.");
        }

        if (open is null)
        {
            var task = EvaluationSchedule.Describe(employee, evaluations, DateOnly.FromDateTime(now));
            open = CreateOpenEvaluation(employeeId, supervisorId, task, EvaluationSchedule.CycleStart(employee, evaluations));
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

        var evaluations = await db.Evaluations.AsNoTracking().Include(v => v.AdditionalCriteria).Where(v => v.EmployeeId == employeeId).ToListAsync(ct);
        var open = EvaluationSchedule.OpenEvaluation(evaluations);
        return EvaluationStages.Current(open) == EvaluationStage.Assessment ? open : null;
    }

    private static async Task<Employee?> FindSubordinateAsync(AppDbContext db, int supervisorId, int employeeId, CancellationToken ct)
    {
        var isSubordinate = await db.EmployeeSupervisors
            .AnyAsync(l => l.SupervisorId == supervisorId && l.EmployeeId == employeeId, ct);
        return isSubordinate ? await db.Employees.SingleOrDefaultAsync(e => e.Id == employeeId, ct) : null;
    }

    private static async Task<(Evaluation? Open, string? Error)> FindOpenPartIAsync(
        AppDbContext db, int supervisorId, int employeeId, CancellationToken ct)
    {
        var employee = await FindSubordinateAsync(db, supervisorId, employeeId, ct);
        if (employee is null)
        {
            return (null, EmployeeNotFound);
        }

        var evaluations = await db.Evaluations
            .Include(v => v.Conversations)
            .Include(v => v.AdditionalCriteria)
            .Where(v => v.EmployeeId == employeeId)
            .ToListAsync(ct);
        var open = EvaluationSchedule.OpenEvaluation(evaluations);
        if (open?.SheetConfirmedAt is null)
        {
            return (null, SheetNotConfirmed);
        }

        return open.PartIConfirmedAt is null ? (open, null) : (null, PartIAlreadyConfirmed);
    }

    private static bool IsConversationEditable(Evaluation evaluation, ConversationType type) =>
        evaluation.Status != EvaluationStatus.Completed
        && (type != ConversationType.Starting || evaluation.PartIConfirmedAt is null);

    private static bool HasPartIDraft(Evaluation evaluation) =>
        evaluation.Expectations is not null || evaluation.Goals is not null || evaluation.AdditionalCriteria.Count > 0;

    private static string? ApplyPartIDraft(
        Evaluation evaluation, IReadOnlyCollection<Criterion> additionalCriteria, string? expectations, string? goals)
    {
        var error = EvaluationCriteria.ValidateAdditional(evaluation.Sheet, additionalCriteria);
        if (error is not null)
        {
            return error;
        }

        var trimmedExpectations = expectations?.Trim() ?? string.Empty;
        var trimmedGoals = string.IsNullOrWhiteSpace(goals) ? null : goals.Trim();
        if (trimmedExpectations.Length > MaxPartITextLength || trimmedGoals?.Length > MaxPartITextLength)
        {
            return $"Oczekiwania i cele mogą mieć najwyżej {MaxPartITextLength} znaków.";
        }

        evaluation.AdditionalCriteria.RemoveAll(a => !additionalCriteria.Contains(a.Criterion));
        foreach (var criterion in additionalCriteria.Where(c => evaluation.AdditionalCriteria.All(a => a.Criterion != c)))
        {
            evaluation.AdditionalCriteria.Add(new EvaluationAdditionalCriterion { Criterion = criterion });
        }

        evaluation.Expectations = trimmedExpectations;
        evaluation.Goals = trimmedGoals;
        return null;
    }

    private static Evaluation CreateOpenEvaluation(int employeeId, int supervisorId, EvaluationTask task, DateOnly cycleStart) => new()
    {
        EmployeeId = employeeId,
        EvaluatorId = supervisorId,
        Sheet = task.Sheet,
        Status = EvaluationStatus.NotStarted,
        PeriodStart = cycleStart,
        DueDate = task.DueDate,
    };
}
