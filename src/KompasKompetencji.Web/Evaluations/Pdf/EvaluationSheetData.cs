using System.Globalization;
using System.Text;
using KompasKompetencji.Web.Data;

namespace KompasKompetencji.Web.Evaluations.Pdf;

public sealed record SheetPerson(string FirstName, string LastName, string Department, string Position)
{
    public static SheetPerson From(Employee employee) =>
        new(employee.FirstName, employee.LastName, employee.Department, employee.Position);
}

public sealed record EvaluationSheetData(
    EvaluationSheet Sheet,
    SheetPerson Employee,
    SheetPerson Evaluator,
    IReadOnlyList<string> MandatoryCriteria,
    IReadOnlyList<string> AdditionalCriteria,
    DateOnly DueDate,
    DateOnly? PartIConfirmedOn,
    DateOnly? EmployeeAcquaintedOn,
    DateOnly? AssessmentConversationOn,
    DateOnly PeriodStart,
    DateOnly PreparedOn,
    DateOnly? PartIIAcquaintedOn)
{
    public static EvaluationSheetData From(Evaluation evaluation) => new(
        evaluation.Sheet,
        SheetPerson.From(evaluation.Employee),
        SheetPerson.From(evaluation.Evaluator),
        EvaluationCriteria.Mandatory(evaluation.Sheet).Select(EvaluationCriteria.Name).ToList(),
        evaluation.AdditionalCriteria.Select(a => a.Criterion).OrderBy(c => c).Select(EvaluationCriteria.Name).ToList(),
        evaluation.DueDate,
        evaluation.PartIConfirmedAt is DateTime confirmedAt ? DateOnly.FromDateTime(confirmedAt) : null,
        evaluation.EmployeeAcquaintedOn,
        CompletionRules.AssessmentConversationDay(evaluation),
        evaluation.PeriodStart,
        evaluation.PreparedOn!.Value,
        evaluation.PartIIAcquaintedOn);

    public string FileName =>
        $"Arkusz_{Sheet}_{AsciiName(Employee.LastName)}_{AsciiName(Employee.FirstName)}_{PreparedOn:yyyy-MM-dd}.pdf";

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
