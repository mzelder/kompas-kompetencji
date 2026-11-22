using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Evaluations;
using KompasKompetencji.Web.Evaluations.Pdf;
using QuestPDF.Fluent;

namespace KompasKompetencji.Tests.Evaluations;

public class EvaluationSheetTests
{
    private static Evaluation CreateEvaluation() => new()
    {
        Id = 1,
        Sheet = EvaluationSheet.C,
        Employee = new Employee { FirstName = "Anna", LastName = "Kowalska", Department = "Wydział Analiz", Position = "Specjalista" },
        Evaluator = new Employee { FirstName = "Ewa", LastName = "Wiśniewska", Department = "Wydział Analiz", Position = "Naczelnik" },
        PeriodStart = new DateOnly(2024, 12, 16),
        DueDate = new DateOnly(2026, 11, 28),
        PreparedOn = new DateOnly(2026, 11, 20),
        AdditionalCriteria = [new EvaluationAdditionalCriterion { EvaluationId = 1, Criterion = Criterion.AnalyticalThinking }],
    };

    private static List<CriterionDecision> Decisions(params (Criterion Criterion, int Level)[] levels) =>
        levels.Select(l => new CriterionDecision { EvaluationId = 1, Criterion = l.Criterion, Level = l.Level }).ToList();

    private static readonly (Criterion, int)[] AllDecided =
    [
        (Criterion.KnowledgeAndDevelopment, 4),
        (Criterion.Reliability, 3),
        (Criterion.WorkOrganization, 3),
        (Criterion.Cooperation, 4),
        (Criterion.Communication, 3),
        (Criterion.AnalyticalThinking, 4),
    ];

    [Fact]
    public void SheetCarriesCriterionLevelsAndSummary()
    {
        var data = EvaluationSheetData.From(CreateEvaluation(), Decisions(AllDecided), null);

        Assert.Equal(4, data.MandatoryCriteria.Single(c => c.Name == EvaluationCriteria.Name(Criterion.KnowledgeAndDevelopment)).Level);
        Assert.Equal(3, data.MandatoryCriteria.Single(c => c.Name == EvaluationCriteria.Name(Criterion.Reliability)).Level);
        Assert.Equal(4, Assert.Single(data.AdditionalCriteria).Level);
        Assert.NotNull(data.Summary);
        Assert.Equal(6.0, data.Summary.AveragePoints);
        Assert.Equal(CompetencyLevels.Name(3), data.Summary.OverallLevel);
        Assert.True(data.Summary.IsPositive);
    }

    [Fact]
    public void SheetHasNoSummaryWhenCriterionIsMissing()
    {
        var data = EvaluationSheetData.From(CreateEvaluation(), Decisions(AllDecided[..^1]), null);

        Assert.Null(data.Summary);
        Assert.Null(Assert.Single(data.AdditionalCriteria).Level);
    }

    [Fact]
    public void SheetIgnoresDecisionsOfOtherEvaluations()
    {
        var decisions = Decisions(AllDecided);
        decisions.Add(new CriterionDecision { EvaluationId = 2, Criterion = Criterion.Creativity, Level = 1 });

        var data = EvaluationSheetData.From(CreateEvaluation(), decisions, null);

        Assert.True(data.Summary?.IsPositive);
    }

    [Fact]
    public void DocumentRendersFilledAndEmptySheets()
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        var previous = new SheetPreviousEvaluation(true, CompetencyLevels.Name(3), new DateOnly(2024, 11, 28));

        var filled = new EvaluationSheetDocument(EvaluationSheetData.From(CreateEvaluation(), Decisions(AllDecided), previous)).Generate();
        var empty = new EvaluationSheetDocument(EvaluationSheetData.From(CreateEvaluation(), [], null)).Generate();

        Assert.NotEmpty(filled);
        Assert.NotEmpty(empty);
    }

    [Theory]
    [InlineData(EvaluationSheet.B)]
    [InlineData(EvaluationSheet.C)]
    public void DocumentKeepsFootnotesWithinThreePages(EvaluationSheet sheet)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        var evaluation = CreateEvaluation();
        evaluation.Sheet = sheet;
        evaluation.AdditionalCriteria =
        [
            new EvaluationAdditionalCriterion { EvaluationId = 1, Criterion = Criterion.AnalyticalThinking },
            new EvaluationAdditionalCriterion { EvaluationId = 1, Criterion = Criterion.Creativity },
            new EvaluationAdditionalCriterion { EvaluationId = 1, Criterion = Criterion.CustomerOrientation },
        ];
        var previous = new SheetPreviousEvaluation(true, CompetencyLevels.Name(3), new DateOnly(2024, 11, 28));

        var pages = new EvaluationSheetDocument(EvaluationSheetData.From(evaluation, [], previous))
            .GenerateImages(new QuestPDF.Infrastructure.ImageGenerationSettings { RasterDpi = 20 })
            .Count();

        Assert.Equal(3, pages);
    }

    [Theory]
    [InlineData(EvaluationSheet.B, 3)]
    [InlineData(EvaluationSheet.C, 4)]
    public void FootnotesFollowSheet(EvaluationSheet sheet, int annex)
    {
        Assert.Equal(annex, EvaluationSheetFootnotes.Annex(sheet));
        Assert.Contains(sheet == EvaluationSheet.B ? "polega na bezpośrednim kierowaniu" : "które nie jest",
            EvaluationSheetFootnotes.Text(EvaluationSheetFootnotes.SheetPurpose, sheet));
        Assert.All(EvaluationSheetFootnotes.PartI.Concat(EvaluationSheetFootnotes.PartII),
            number => Assert.NotEmpty(EvaluationSheetFootnotes.Text(number, sheet)));
    }
}
