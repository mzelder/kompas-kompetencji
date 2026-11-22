using KompasKompetencji.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace KompasKompetencji.Web.Evaluations;

public sealed record EvaluationListRow(
    int Id,
    Employee Employee,
    EvaluationSheet Sheet,
    EvaluationStage? Stage,
    DateOnly DueDate,
    DateOnly PeriodStart,
    DateOnly? PreparedOn,
    int DecidedCriteria,
    int TotalCriteria,
    WrittenEvaluationSummary? Summary,
    bool CanDownloadSheet)
{
    public bool IsCompleted => Stage is null;

    public string FullName => Employee.FullName;

    public double? AveragePoints => Summary?.AveragePoints;

    public int CriteriaProgress => TotalCriteria == 0 ? 0 : DecidedCriteria * 100 / TotalCriteria;
}

public sealed record SupervisorEvaluations(IReadOnlyList<EvaluationListRow> InProgress, IReadOnlyList<EvaluationListRow> Completed);

public sealed class EvaluationListService(IDbContextFactory<AppDbContext> dbFactory)
{
    public async Task<SupervisorEvaluations> GetAsync(int supervisorId, CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var employeeIds = await db.EmployeeSupervisors.AsNoTracking()
            .Where(l => l.SupervisorId == supervisorId)
            .Select(l => l.EmployeeId)
            .ToListAsync(ct);
        var evaluations = await db.Evaluations.AsNoTracking()
            .Include(v => v.Employee)
            .Include(v => v.AdditionalCriteria)
            .Include(v => v.Conversations)
            .Where(v => employeeIds.Contains(v.EmployeeId))
            .ToListAsync(ct);
        var evaluationIds = evaluations.Select(v => v.Id).ToList();
        var decisions = (await db.CriterionDecisions.AsNoTracking()
                .Where(d => evaluationIds.Contains(d.EvaluationId))
                .ToListAsync(ct))
            .ToLookup(d => d.EvaluationId);

        var rows = evaluations
            .Where(v => v.Status == EvaluationStatus.Completed
                        || EvaluationSchedule.OpenEvaluation(evaluations.Where(o => o.EmployeeId == v.EmployeeId)) == v)
            .Select(v => ToRow(v, decisions[v.Id].ToList()))
            .ToList();

        return new SupervisorEvaluations(
            rows.Where(r => !r.IsCompleted).OrderBy(r => r.DueDate).ThenBy(r => r.Employee.LastName).ToList(),
            rows.Where(r => r.IsCompleted).OrderByDescending(r => r.PreparedOn).ThenBy(r => r.Employee.LastName).ToList());
    }

    private static EvaluationListRow ToRow(Evaluation evaluation, IReadOnlyList<CriterionDecision> decisions)
    {
        var criteria = EvaluationCriteria.Mandatory(evaluation.Sheet)
            .Concat(evaluation.AdditionalCriteria.Select(a => a.Criterion))
            .ToList();
        var decided = decisions.Where(d => criteria.Contains(d.Criterion)).ToList();
        var summary = criteria.Count > 0 && decided.Count == criteria.Count
            ? WrittenEvaluationSummary.From(decided.Select(d => d.Level).ToList())
            : null;
        var isCompleted = evaluation.Status == EvaluationStatus.Completed;
        var stage = isCompleted ? (EvaluationStage?)null : EvaluationStages.Current(evaluation);

        return new EvaluationListRow(
            evaluation.Id,
            evaluation.Employee,
            evaluation.Sheet,
            stage,
            evaluation.DueDate,
            evaluation.PeriodStart,
            evaluation.PreparedOn,
            decided.Count,
            criteria.Count,
            summary,
            evaluation.PreparedOn is not null && (isCompleted || stage == EvaluationStage.Completion));
    }
}
