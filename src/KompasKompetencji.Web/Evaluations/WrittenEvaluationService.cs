using KompasKompetencji.Web.Assistant;
using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Organization;
using Microsoft.EntityFrameworkCore;

namespace KompasKompetencji.Web.Evaluations;

public sealed record IndicatorAssessment(CompetencyIndicator Indicator, bool IsAnswered, int? Level);

public sealed record CriterionAssessment(
    Criterion Criterion,
    bool IsMandatory,
    IReadOnlyList<IndicatorAssessment> Indicators,
    CriterionDecision? Decision)
{
    public bool IsDecided => Decision is not null;

    public bool AllAnswered => Indicators.All(i => i.IsAnswered);

    public int AnsweredCount => Indicators.Count(i => i.IsAnswered);

    public LevelSuggestion Suggestion => CriterionLevelRule.Suggest(Indicators.Where(i => i.IsAnswered).Select(i => i.Level).ToList());
}

public sealed record WrittenEvaluationView(string? LockReason, bool IsReadOnly, IReadOnlyList<CriterionAssessment> Criteria)
{
    public bool IsUnlocked => LockReason is null;

    public int DecidedCount => Criteria.Count(c => c.IsDecided);

    public bool AllDecided => Criteria.Count > 0 && Criteria.All(c => c.IsDecided);

    public WrittenEvaluationSummary? Summary => AllDecided
        ? WrittenEvaluationSummary.From(Criteria.Select(c => c.Decision!.Level).ToList())
        : null;
}

public sealed class WrittenEvaluationService(IDbContextFactory<AppDbContext> dbFactory, IndicatorSuggester suggester)
{
    private sealed record Context(Evaluation Open, IReadOnlyList<Criterion> Criteria, EvaluationStage Stage, string? LockReason);

    public async Task<WrittenEvaluationView?> GetAsync(int supervisorId, int employeeId, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        if (await LoadContextAsync(db, supervisorId, employeeId, ct) is not Context context)
        {
            return null;
        }

        var indicators = await db.CompetencyIndicators.AsNoTracking()
            .Include(i => i.Behaviors)
            .Where(i => context.Criteria.Contains(i.Criterion))
            .ToListAsync(ct);
        var ratings = await db.IndicatorRatings.AsNoTracking()
            .Where(r => r.EvaluationId == context.Open.Id)
            .ToDictionaryAsync(r => r.IndicatorId, ct);
        var decisions = await db.CriterionDecisions.AsNoTracking()
            .Where(d => d.EvaluationId == context.Open.Id)
            .ToDictionaryAsync(d => d.Criterion, ct);
        var mandatory = EvaluationCriteria.Mandatory(context.Open.Sheet);

        var criteria = context.Criteria
            .Select(criterion => new CriterionAssessment(
                criterion,
                mandatory.Contains(criterion),
                indicators
                    .Where(i => i.Criterion == criterion)
                    .OrderBy(i => i.Number)
                    .Select(i => new IndicatorAssessment(i, ratings.ContainsKey(i.Id), ratings.GetValueOrDefault(i.Id)?.Level))
                    .ToList(),
                decisions.GetValueOrDefault(criterion)))
            .ToList();

        return new WrittenEvaluationView(context.LockReason, context.Stage != EvaluationStage.Assessment, criteria);
    }

    public async Task<OperationResult> RateIndicatorAsync(
        int supervisorId, int employeeId, int indicatorId, int? level, DateTime now, CancellationToken ct = default)
    {
        if (level is int value && !CompetencyLevels.IsValid(value))
        {
            return OperationResult.Fail("Nieznany poziom zachowania.");
        }

        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var (context, error) = await LoadEditableContextAsync(db, supervisorId, employeeId, ct);
        if (context is null)
        {
            return OperationResult.Fail(error!);
        }

        var indicator = await db.CompetencyIndicators.SingleOrDefaultAsync(i => i.Id == indicatorId, ct);
        if (indicator is null || !context.Criteria.Contains(indicator.Criterion))
        {
            return OperationResult.Fail("Ten wskaźnik nie należy do kryteriów tej oceny.");
        }

        var rating = await db.IndicatorRatings.SingleOrDefaultAsync(r => r.EvaluationId == context.Open.Id && r.IndicatorId == indicatorId, ct);
        if (rating is null)
        {
            db.IndicatorRatings.Add(new IndicatorRating { EvaluationId = context.Open.Id, IndicatorId = indicatorId, Level = level });
        }
        else
        {
            rating.Level = level;
        }
        await db.SaveChangesAsync(ct);

        await UpdateCriterionLevelAsync(db, context.Open.Id, indicator.Criterion, now, ct);
        return OperationResult.Success;
    }

    private static async Task UpdateCriterionLevelAsync(AppDbContext db, int evaluationId, Criterion criterion, DateTime now, CancellationToken ct)
    {
        var indicatorIds = await db.CompetencyIndicators.Where(i => i.Criterion == criterion).Select(i => i.Id).ToListAsync(ct);
        var ratings = await db.IndicatorRatings
            .Where(r => r.EvaluationId == evaluationId && indicatorIds.Contains(r.IndicatorId))
            .ToListAsync(ct);
        var level = ratings.Count == indicatorIds.Count
            ? CriterionLevelRule.Suggest(ratings.Select(r => r.Level).ToList()).Level
            : null;

        var decision = await db.CriterionDecisions.SingleOrDefaultAsync(d => d.EvaluationId == evaluationId && d.Criterion == criterion, ct);
        if (level is int computed)
        {
            if (decision is null)
            {
                decision = new CriterionDecision { EvaluationId = evaluationId, Criterion = criterion };
                db.CriterionDecisions.Add(decision);
            }
            decision.SuggestedLevel = computed;
            decision.Level = computed;
            decision.Justification = null;
            decision.DecidedAt = now;
        }
        else if (decision is not null)
        {
            db.CriterionDecisions.Remove(decision);
        }
        await db.SaveChangesAsync(ct);
    }

    public async Task<OperationResult> FinishAsync(int supervisorId, int employeeId, DateTime now, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var (context, error) = await LoadEditableContextAsync(db, supervisorId, employeeId, ct);
        if (context is null)
        {
            return OperationResult.Fail(error!);
        }

        var decided = await db.CriterionDecisions
            .Where(d => d.EvaluationId == context.Open.Id)
            .Select(d => d.Criterion)
            .ToListAsync(ct);
        var missing = context.Criteria.Except(decided).ToList();
        if (missing.Count > 0)
        {
            return OperationResult.Fail($"Oceń jeszcze wskaźniki kryteriów: {string.Join(", ", missing.Select(EvaluationCriteria.Name))}.");
        }

        var evaluation = await db.Evaluations.SingleAsync(v => v.Id == context.Open.Id, ct);
        evaluation.WrittenEvaluationAt = now;
        await db.SaveChangesAsync(ct);
        return OperationResult.Success;
    }

    public async Task<IndicatorSuggestions> SuggestFromNotesAsync(int supervisorId, int employeeId, Criterion criterion, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var (context, _) = await LoadEditableContextAsync(db, supervisorId, employeeId, ct);
        if (context is null || !context.Criteria.Contains(criterion))
        {
            return IndicatorSuggestions.Unavailable;
        }

        var indicators = await db.CompetencyIndicators.AsNoTracking()
            .Include(i => i.Behaviors)
            .Where(i => i.Criterion == criterion)
            .ToListAsync(ct);
        var notes = await db.EvaluationNotes.AsNoTracking()
            .Where(n => n.EvaluationId == context.Open.Id && n.Criterion == criterion)
            .ToListAsync(ct);
        return await suggester.SuggestAsync(criterion, indicators, notes, ct);
    }

    private async Task<(Context? Context, string? Error)> LoadEditableContextAsync(
        AppDbContext db, int supervisorId, int employeeId, CancellationToken ct)
    {
        var context = await LoadContextAsync(db, supervisorId, employeeId, ct);
        return context switch
        {
            null => (null, "Nie znaleziono pracownika w Twoim zespole."),
            { Stage: not EvaluationStage.Assessment } => (null, "Oceny kryteriów nie można już zmieniać."),
            { LockReason: string reason } => (null, reason),
            _ => (context, null),
        };
    }

    private static async Task<Context?> LoadContextAsync(AppDbContext db, int supervisorId, int employeeId, CancellationToken ct)
    {
        var isSubordinate = await db.EmployeeSupervisors.AnyAsync(l => l.SupervisorId == supervisorId && l.EmployeeId == employeeId, ct);
        if (!isSubordinate)
        {
            return null;
        }

        var evaluations = await db.Evaluations.AsNoTracking()
            .Include(v => v.Conversations)
            .Include(v => v.AdditionalCriteria)
            .Where(v => v.EmployeeId == employeeId)
            .ToListAsync(ct);
        var open = EvaluationSchedule.OpenEvaluation(evaluations);
        var stage = EvaluationStages.Current(open);
        if (open is null || stage < EvaluationStage.Assessment)
        {
            return null;
        }

        var criteria = EvaluationCriteria.Mandatory(open.Sheet)
            .Concat(open.AdditionalCriteria.Select(a => a.Criterion).OrderBy(c => c))
            .ToList();
        var conversation = open.FindConversation(ConversationType.Assessment);
        var lockReason = conversation switch
        {
            { Status: ConversationStatus.Held } => null,
            { } scheduled => $"Formularz odblokuje się, gdy oznaczysz rozmowę oceniającą ({EvaluationText.Date(scheduled.At)}) jako odbytą.",
            null => "Formularz odblokuje się po rozmowie oceniającej. Najpierw wyznacz jej termin, a po rozmowie oznacz ją jako odbytą.",
        };
        return new Context(open, criteria, stage, lockReason);
    }
}
