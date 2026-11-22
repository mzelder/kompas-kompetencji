using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261122015531_AddDemoCriterionDecisions : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "CriterionDecisions",
            columns: new[] { "Criterion", "EvaluationId", "DecidedAt", "Justification", "Level", "SuggestedLevel" },
            values: new object[,]
            {
                { "AnalyticalThinking", 1, new DateTime(2024, 12, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "Communication", 1, new DateTime(2024, 12, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "Cooperation", 1, new DateTime(2024, 12, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "KnowledgeAndDevelopment", 1, new DateTime(2024, 12, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "Reliability", 1, new DateTime(2024, 12, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "WorkOrganization", 1, new DateTime(2024, 12, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "AnalyticalThinking", 3, new DateTime(2024, 10, 28, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Communication", 3, new DateTime(2024, 10, 28, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Cooperation", 3, new DateTime(2024, 10, 28, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "KnowledgeAndDevelopment", 3, new DateTime(2024, 10, 28, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Reliability", 3, new DateTime(2024, 10, 28, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "WorkOrganization", 3, new DateTime(2024, 10, 28, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Communication", 4, new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Cooperation", 4, new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "CustomerOrientation", 4, new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "KnowledgeAndDevelopment", 4, new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Reliability", 4, new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "WorkOrganization", 4, new DateTime(2025, 1, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "AnalyticalThinking", 5, new DateTime(2026, 11, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 5, 5 },
                { "Communication", 5, new DateTime(2026, 11, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Cooperation", 5, new DateTime(2026, 11, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "KnowledgeAndDevelopment", 5, new DateTime(2026, 11, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "Reliability", 5, new DateTime(2026, 11, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "WorkOrganization", 5, new DateTime(2026, 11, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "AnalyticalThinking", 7, new DateTime(2024, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Communication", 7, new DateTime(2024, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Cooperation", 7, new DateTime(2024, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "KnowledgeAndDevelopment", 7, new DateTime(2024, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Reliability", 7, new DateTime(2024, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 2 },
                { "WorkOrganization", 7, new DateTime(2024, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "AnalyticalThinking", 10, new DateTime(2025, 3, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "Communication", 10, new DateTime(2025, 3, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Cooperation", 10, new DateTime(2025, 3, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "KnowledgeAndDevelopment", 10, new DateTime(2025, 3, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "Reliability", 10, new DateTime(2025, 3, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "WorkOrganization", 10, new DateTime(2025, 3, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "AnalyticalThinking", 12, new DateTime(2025, 1, 14, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "Communication", 12, new DateTime(2025, 1, 14, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Cooperation", 12, new DateTime(2025, 1, 14, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "KnowledgeAndDevelopment", 12, new DateTime(2025, 1, 14, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Reliability", 12, new DateTime(2025, 1, 14, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "WorkOrganization", 12, new DateTime(2025, 1, 14, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 2 },
                { "AnalyticalThinking", 14, new DateTime(2024, 11, 28, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Communication", 14, new DateTime(2024, 11, 28, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "Cooperation", 14, new DateTime(2024, 11, 28, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "KnowledgeAndDevelopment", 14, new DateTime(2024, 11, 28, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Reliability", 14, new DateTime(2024, 11, 28, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "WorkOrganization", 14, new DateTime(2024, 11, 28, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "AnalyticalThinking", 15, new DateTime(2026, 11, 19, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "Communication", 15, new DateTime(2026, 11, 19, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Cooperation", 15, new DateTime(2026, 11, 19, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "KnowledgeAndDevelopment", 15, new DateTime(2026, 11, 19, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "Reliability", 15, new DateTime(2026, 11, 19, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "WorkOrganization", 15, new DateTime(2026, 11, 19, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "AnalyticalThinking", 16, new DateTime(2024, 12, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "Communication", 16, new DateTime(2024, 12, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "Cooperation", 16, new DateTime(2024, 12, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "KnowledgeAndDevelopment", 16, new DateTime(2024, 12, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "Reliability", 16, new DateTime(2024, 12, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "WorkOrganization", 16, new DateTime(2024, 12, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 1 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Communication", 1 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Cooperation", 1 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "KnowledgeAndDevelopment", 1 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Reliability", 1 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "WorkOrganization", 1 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 3 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Communication", 3 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Cooperation", 3 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "KnowledgeAndDevelopment", 3 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Reliability", 3 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "WorkOrganization", 3 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Communication", 4 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Cooperation", 4 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "CustomerOrientation", 4 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "KnowledgeAndDevelopment", 4 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Reliability", 4 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "WorkOrganization", 4 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 5 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Communication", 5 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Cooperation", 5 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "KnowledgeAndDevelopment", 5 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Reliability", 5 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "WorkOrganization", 5 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 7 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Communication", 7 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Cooperation", 7 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "KnowledgeAndDevelopment", 7 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Reliability", 7 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "WorkOrganization", 7 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 10 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Communication", 10 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Cooperation", 10 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "KnowledgeAndDevelopment", 10 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Reliability", 10 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "WorkOrganization", 10 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 12 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Communication", 12 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Cooperation", 12 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "KnowledgeAndDevelopment", 12 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Reliability", 12 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "WorkOrganization", 12 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 14 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Communication", 14 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Cooperation", 14 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "KnowledgeAndDevelopment", 14 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Reliability", 14 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "WorkOrganization", 14 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 15 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Communication", 15 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Cooperation", 15 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "KnowledgeAndDevelopment", 15 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Reliability", 15 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "WorkOrganization", 15 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 16 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Communication", 16 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Cooperation", 16 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "KnowledgeAndDevelopment", 16 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Reliability", 16 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "WorkOrganization", 16 });
    }
}
