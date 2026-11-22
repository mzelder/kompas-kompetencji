using System.Globalization;
using System.Text;
using KompasKompetencji.Web.Data;

namespace KompasKompetencji.Web.Evaluations.Pdf;

public sealed record SheetPerson(string FirstName, string LastName, string Department, string Position)
{
    public static SheetPerson From(Employee employee) =>
        new(employee.FirstName, employee.LastName, employee.Department, employee.Position);
}

public sealed record SheetCriterion(string Name, int? Level);

public sealed record SheetPreviousEvaluation(bool IsPositive, string OverallLevel, DateOnly PreparedOn);

public sealed record EvaluationSheetData(
    EvaluationSheet Sheet,
    SheetPerson Employee,
    SheetPerson Evaluator,
    IReadOnlyList<SheetCriterion> MandatoryCriteria,
    IReadOnlyList<SheetCriterion> AdditionalCriteria,
    DateOnly DueDate,
    DateOnly? PartIConfirmedOn,
    DateOnly? EmployeeAcquaintedOn,
    DateOnly? AssessmentConversationOn,
    DateOnly PeriodStart,
    DateOnly PreparedOn,
    DateOnly? PartIIAcquaintedOn,
    WrittenEvaluationSummary? Summary,
    SheetPreviousEvaluation? Previous)
{
    public static EvaluationSheetData From(
        Evaluation evaluation, IReadOnlyCollection<CriterionDecision> decisions, SheetPreviousEvaluation? previous)
    {
        var levels = decisions.Where(d => d.EvaluationId == evaluation.Id).ToDictionary(d => d.Criterion, d => d.Level);
        var mandatory = EvaluationCriteria.Mandatory(evaluation.Sheet);
        var additional = evaluation.AdditionalCriteria.Select(a => a.Criterion).OrderBy(c => c).ToList();

        return new(
            evaluation.Sheet,
            SheetPerson.From(evaluation.Employee),
            SheetPerson.From(evaluation.Evaluator),
            mandatory.Select(c => Criterion(c, levels)).ToList(),
            additional.Select(c => Criterion(c, levels)).ToList(),
            evaluation.DueDate,
            evaluation.PartIConfirmedAt is DateTime confirmedAt ? DateOnly.FromDateTime(confirmedAt) : null,
            evaluation.EmployeeAcquaintedOn,
            CompletionRules.AssessmentConversationDay(evaluation),
            evaluation.PeriodStart,
            evaluation.PreparedOn!.Value,
            evaluation.PartIIAcquaintedOn,
            Summarize(evaluation, decisions),
            previous);
    }

    public static WrittenEvaluationSummary? Summarize(Evaluation evaluation, IReadOnlyCollection<CriterionDecision> decisions)
    {
        var criteria = EvaluationCriteria.Mandatory(evaluation.Sheet)
            .Concat(evaluation.AdditionalCriteria.Select(a => a.Criterion))
            .ToList();
        var levels = decisions
            .Where(d => d.EvaluationId == evaluation.Id && criteria.Contains(d.Criterion))
            .Select(d => d.Level)
            .ToList();
        return criteria.Count > 0 && levels.Count == criteria.Count ? WrittenEvaluationSummary.From(levels) : null;
    }

    public string FileName =>
        $"Arkusz_{Sheet}_{AsciiName(Employee.LastName)}_{AsciiName(Employee.FirstName)}_{PreparedOn:yyyy-MM-dd}.pdf";

    private static SheetCriterion Criterion(Criterion criterion, IReadOnlyDictionary<Criterion, int> levels) =>
        new(EvaluationCriteria.Name(criterion), levels.TryGetValue(criterion, out var level) ? level : null);

    private static string AsciiName(string text)
    {
        var builder = new StringBuilder();
        foreach (var c in text.Replace('ł', 'l').Replace('Ł', 'L').Normalize(NormalizationForm.FormD))
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            builder.Append(char.IsAsciiLetterOrDigit(c) ? c : '_');
        }
        return builder.ToString();
    }
}
