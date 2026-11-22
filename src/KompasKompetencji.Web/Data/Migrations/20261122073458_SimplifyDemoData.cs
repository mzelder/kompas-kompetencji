using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261122073458_SimplifyDemoData : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 1);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 2);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 3);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 4);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 5);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 6);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 7);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 8);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 11);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 12);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 13);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 14);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 15);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 16);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 18);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 19);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 21);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 22);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 23);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 24);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 25);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 26);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 27);

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
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 1, 3 });

        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 2, 3 });

        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 5, 4 });

        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 6, 5 });

        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 7, 5 });

        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 8, 5 });

        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 8, 6 });

        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 9, 3 });

        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 12, 3 });

        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 13, 3 });

        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 15, 3 });

        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 16, 3 });

        migrationBuilder.DeleteData(
            table: "EvaluationAdditionalCriteria",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 1 });

        migrationBuilder.DeleteData(
            table: "EvaluationAdditionalCriteria",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 2 });

        migrationBuilder.DeleteData(
            table: "EvaluationAdditionalCriteria",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Creativity", 2 });

        migrationBuilder.DeleteData(
            table: "EvaluationAdditionalCriteria",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 3 });

        migrationBuilder.DeleteData(
            table: "EvaluationAdditionalCriteria",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "CustomerOrientation", 4 });

        migrationBuilder.DeleteData(
            table: "EvaluationAdditionalCriteria",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 6 });

        migrationBuilder.DeleteData(
            table: "EvaluationAdditionalCriteria",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "CustomerOrientation", 6 });

        migrationBuilder.DeleteData(
            table: "EvaluationAdditionalCriteria",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 7 });

        migrationBuilder.DeleteData(
            table: "EvaluationAdditionalCriteria",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 8 });

        migrationBuilder.DeleteData(
            table: "EvaluationAdditionalCriteria",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 10 });

        migrationBuilder.DeleteData(
            table: "EvaluationAdditionalCriteria",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 12 });

        migrationBuilder.DeleteData(
            table: "EvaluationAdditionalCriteria",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 13 });

        migrationBuilder.DeleteData(
            table: "EvaluationAdditionalCriteria",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 14 });

        migrationBuilder.DeleteData(
            table: "EvaluationAdditionalCriteria",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 15 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 1 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 2 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 3 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 4 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 5 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 6 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 7 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 8 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 9 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 10 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 11 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 12 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 13 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 14 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 17 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 18 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 19 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 20 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 21 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 22 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 23 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 24 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 25 });

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 5);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 6);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 7);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 8);

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 1);

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 2);

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 3);

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 4);

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 6);

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 7);

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 8);

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 10);

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 12);

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 13);

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 14);

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 15);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 1);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 2);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 9);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 12);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 13);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 15);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 16);

        migrationBuilder.DeleteData(
            table: "JobPositions",
            keyColumn: "Id",
            keyValue: 1);

        migrationBuilder.DeleteData(
            table: "JobPositions",
            keyColumn: "Id",
            keyValue: 2);

        migrationBuilder.DeleteData(
            table: "JobPositions",
            keyColumn: "Id",
            keyValue: 4);

        migrationBuilder.DeleteData(
            table: "JobPositions",
            keyColumn: "Id",
            keyValue: 7);

        migrationBuilder.DeleteData(
            table: "JobPositions",
            keyColumn: "Id",
            keyValue: 9);

        migrationBuilder.DeleteData(
            table: "JobPositions",
            keyColumn: "Id",
            keyValue: 10);

        migrationBuilder.UpdateData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 17,
            column: "At",
            value: new DateTime(2026, 11, 19, 10, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.UpdateData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 20,
            column: "At",
            value: new DateTime(2025, 2, 10, 9, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 14,
            column: "EmployedSince",
            value: new DateOnly(2025, 1, 20));

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 11,
            columns: new[] { "DueDate", "EmployeeAcquaintedOn", "PartIConfirmedAt", "PeriodStart", "SheetConfirmedAt" },
            values: new object[] { new DateOnly(2027, 1, 20), new DateOnly(2025, 2, 10), new DateTime(2025, 2, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 2, 10), new DateTime(2025, 1, 27, 9, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.UpdateData(
            table: "JobPositions",
            keyColumn: "Id",
            keyValue: 3,
            column: "Subordinates",
            value: "UpTo4");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 17,
            column: "At",
            value: new DateTime(2026, 11, 26, 10, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.UpdateData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 20,
            column: "At",
            value: new DateTime(2025, 4, 21, 9, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 14,
            column: "EmployedSince",
            value: new DateOnly(2023, 4, 3));

        migrationBuilder.InsertData(
            table: "Employees",
            columns: new[] { "Id", "Department", "EmployedSince", "FirstName", "IsSupervisor", "JobPositionId", "LastName", "Position" },
            values: new object[,]
            {
                { 5, "Wydział Usług Cyfrowych", new DateOnly(2016, 2, 15), "Marek", true, null, "Lewandowski", "Naczelnik wydziału" },
                { 6, "Wydział Usług Cyfrowych", new DateOnly(2018, 5, 1), "Magdalena", true, null, "Dąbrowska", "Zastępca naczelnika wydziału" },
                { 7, "Wydział Usług Cyfrowych", new DateOnly(2021, 9, 1), "Katarzyna", false, null, "Wójcik", "Główny specjalista" },
                { 8, "Wydział Usług Cyfrowych", new DateOnly(2024, 3, 1), "Piotr", false, null, "Kamiński", "Starszy referent" },
                { 13, "Wydział Analiz Danych", new DateOnly(2024, 12, 10), "Łukasz", false, null, "Grabowski", "Specjalista" }
            });

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 11,
            columns: new[] { "DueDate", "EmployeeAcquaintedOn", "PartIConfirmedAt", "PeriodStart", "SheetConfirmedAt" },
            values: new object[] { new DateOnly(2027, 4, 3), new DateOnly(2025, 4, 21), new DateTime(2025, 4, 21, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 4, 21), new DateTime(2025, 4, 7, 9, 0, 0, 0, DateTimeKind.Unspecified) });

        migrationBuilder.InsertData(
            table: "Evaluations",
            columns: new[] { "Id", "CompletedAt", "DueDate", "EmployeeAcquaintedOn", "EmployeeId", "EvaluatorId", "Expectations", "Goals", "PartIConfirmedAt", "PartIIAcquaintedOn", "PeriodEnd", "PeriodStart", "PreparedOn", "Sheet", "SheetConfirmedAt", "StartedAt", "Status", "WrittenEvaluationAt" },
            values: new object[] { 10, new DateTime(2025, 3, 30, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 4, 3), new DateOnly(2023, 4, 22), 14, 3, "Zestawy wskaźników przygotowywane z opisem metody pomiaru i źródeł danych, uzgodnione z komórkami merytorycznymi.", null, new DateTime(2023, 4, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 3, 30), new DateOnly(2025, 3, 30), new DateOnly(2023, 4, 22), new DateOnly(2025, 3, 30), "C", new DateTime(2023, 4, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed", null });

        migrationBuilder.UpdateData(
            table: "JobPositions",
            keyColumn: "Id",
            keyValue: 3,
            column: "Subordinates",
            value: "Over9");

        migrationBuilder.InsertData(
            table: "JobPositions",
            columns: new[] { "Id", "Department", "DirectSupervisorTitle", "IndirectSupervisorTitle", "Purpose", "RequiredCompetencies", "Subordinates", "Symbol", "Tasks", "Title", "Unit" },
            values: new object[,]
            {
                { 1, "Departament Danych i Usług Cyfrowych", "Naczelnik wydziału", "Dyrektor departamentu", "Przygotowywanie analiz danych wspierających decyzje kierownictwa departamentu oraz dbanie o jakość danych wykorzystywanych w raportach.", "Myślenie analityczne\nOrientacja na cele\nKomunikacja pisemna\nWspółpraca", "None", "DDU WAD 4", "Przygotowywanie analiz i raportów na potrzeby kierownictwa departamentu\nOpracowywanie wskaźników i metod pomiaru realizacji zadań\nWeryfikacja jakości i kompletności danych z systemów dziedzinowych\nPrzygotowywanie projektów stanowisk do dokumentów przekazanych do zaopiniowania\nWsparcie merytoryczne mniej doświadczonych pracowników wydziału", "Starszy specjalista", "Wydział Analiz Danych" },
                { 2, "Departament Danych i Usług Cyfrowych", "Naczelnik wydziału", "Dyrektor departamentu", "Realizacja zadań związanych z gromadzeniem, przetwarzaniem i udostępnianiem danych na potrzeby wydziału.", "Myślenie analityczne\nRzetelność\nKomunikacja\nWspółpraca", "None", "DDU WAD 7", "Gromadzenie i porządkowanie danych z jednostek podległych\nPrzygotowywanie zestawień i wizualizacji danych\nUdzielanie odpowiedzi na zapytania dotyczące danych publicznych\nProwadzenie dokumentacji źródeł danych", "Specjalista", "Wydział Analiz Danych" },
                { 4, "Departament Danych i Usług Cyfrowych", "Naczelnik wydziału", "Dyrektor departamentu", "Obsługa administracyjna wydziału i wsparcie pracowników w przygotowywaniu zestawień danych.", "Rzetelność\nOrganizacja pracy własnej\nKomunikacja", "None", "DDU WAD 12", "Prowadzenie rejestru korespondencji wydziału\nPrzygotowywanie prostych zestawień danych według wskazówek\nArchiwizowanie dokumentacji wydziału\nOrganizacja spotkań i przygotowywanie notatek", "Referent", "Wydział Analiz Danych" },
                { 7, "Departament Danych i Usług Cyfrowych", "Naczelnik wydziału", "Dyrektor departamentu", "Realizacja zadań związanych z analizą danych i przygotowywaniem informacji dla kierownictwa.", "Myślenie analityczne\nRzetelność\nKomunikacja pisemna", "None", "DDU WAD 8", "Przygotowywanie cyklicznych raportów z danych wydziału\nAnaliza danych ankietowych i statystycznych\nPrzygotowywanie projektów odpowiedzi na pisma\nAktualizacja danych publikowanych w serwisie urzędu", "Specjalista", "Wydział Analiz Danych" },
                { 9, "Departament Danych i Usług Cyfrowych", "Naczelnik wydziału", "Dyrektor departamentu", "Przygotowywanie analiz statystycznych i prognoz na potrzeby planowania zadań departamentu.", "Myślenie analityczne\nRzetelność\nKomunikacja\nOrientacja na cele", "None", "DDU WAD 9", "Przygotowywanie prognoz i analiz trendów na podstawie danych wydziału\nOpracowywanie zestawień do planów i sprawozdań departamentu\nWeryfikacja poprawności obliczeń w raportach przygotowywanych przez wydział\nUdzielanie wyjaśnień dotyczących metod analizy innym komórkom", "Starszy specjalista", "Wydział Analiz Danych" },
                { 10, "Departament Danych i Usług Cyfrowych", "Naczelnik wydziału", "Dyrektor departamentu", "Obsługa wniosków o udostępnienie danych i przygotowywanie zestawień dla podmiotów zewnętrznych.", "Rzetelność\nKomunikacja pisemna\nWspółpraca", "None", "DDU WAD 10", "Rozpatrywanie wniosków o udostępnienie danych wydziału\nPrzygotowywanie zestawień danych dla podmiotów zewnętrznych\nProwadzenie rejestru udostępnionych danych\nWspółpraca z działem prawnym przy odpowiedziach na wnioski", "Specjalista", "Wydział Analiz Danych" }
            });

        migrationBuilder.InsertData(
            table: "Conversations",
            columns: new[] { "Id", "At", "EvaluationId", "Status", "Type" },
            values: new object[,]
            {
                { 18, new DateTime(2023, 4, 22, 9, 0, 0, 0, DateTimeKind.Unspecified), 10, "Held", "Starting" },
                { 19, new DateTime(2025, 3, 25, 10, 0, 0, 0, DateTimeKind.Unspecified), 10, "Held", "Assessment" }
            });

        migrationBuilder.InsertData(
            table: "CriterionDecisions",
            columns: new[] { "Criterion", "EvaluationId", "DecidedAt", "Justification", "Level", "SuggestedLevel" },
            values: new object[,]
            {
                { "AnalyticalThinking", 10, new DateTime(2025, 3, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "Communication", 10, new DateTime(2025, 3, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Cooperation", 10, new DateTime(2025, 3, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "KnowledgeAndDevelopment", 10, new DateTime(2025, 3, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "Reliability", 10, new DateTime(2025, 3, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "WorkOrganization", 10, new DateTime(2025, 3, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 }
            });

        migrationBuilder.InsertData(
            table: "EmployeeSupervisors",
            columns: new[] { "EmployeeId", "SupervisorId" },
            values: new object[,]
            {
                { 5, 4 },
                { 6, 5 },
                { 7, 5 },
                { 8, 5 },
                { 8, 6 },
                { 13, 3 }
            });

        migrationBuilder.InsertData(
            table: "Employees",
            columns: new[] { "Id", "Department", "EmployedSince", "FirstName", "IsSupervisor", "JobPositionId", "LastName", "Position" },
            values: new object[,]
            {
                { 1, "Wydział Analiz Danych", new DateOnly(2019, 12, 15), "Anna", false, 1, "Kowalska", "Starszy specjalista" },
                { 2, "Wydział Analiz Danych", new DateOnly(2022, 11, 2), "Jan", false, 2, "Nowak", "Specjalista" },
                { 9, "Wydział Analiz Danych", new DateOnly(2018, 1, 10), "Michał", false, 4, "Szymański", "Referent" },
                { 12, "Wydział Analiz Danych", new DateOnly(2021, 12, 20), "Karolina", false, 7, "Mazur", "Specjalista" },
                { 15, "Wydział Analiz Danych", new DateOnly(2022, 1, 17), "Bartosz", false, 9, "Wróbel", "Starszy specjalista" },
                { 16, "Wydział Analiz Danych", new DateOnly(2019, 11, 28), "Monika", false, 10, "Sikora", "Specjalista" }
            });

        migrationBuilder.InsertData(
            table: "EvaluationAdditionalCriteria",
            columns: new[] { "Criterion", "EvaluationId" },
            values: new object[] { "AnalyticalThinking", 10 });

        migrationBuilder.InsertData(
            table: "Evaluations",
            columns: new[] { "Id", "CompletedAt", "DueDate", "EmployeeAcquaintedOn", "EmployeeId", "EvaluatorId", "Expectations", "Goals", "PartIConfirmedAt", "PartIIAcquaintedOn", "PeriodEnd", "PeriodStart", "PreparedOn", "Sheet", "SheetConfirmedAt", "StartedAt", "Status", "WrittenEvaluationAt" },
            values: new object[] { 8, null, new DateOnly(2026, 12, 10), new DateOnly(2024, 12, 17), 13, 3, "Analizy danych przygotowywane samodzielnie, z wnioskami zrozumiałymi dla odbiorców spoza wydziału.", null, new DateTime(2024, 12, 17, 15, 0, 0, 0, DateTimeKind.Unspecified), null, null, new DateOnly(2024, 12, 17), null, "C", new DateTime(2024, 12, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), null, "NotStarted", null });

        migrationBuilder.InsertData(
            table: "Conversations",
            columns: new[] { "Id", "At", "EvaluationId", "Status", "Type" },
            values: new object[,]
            {
                { 15, new DateTime(2024, 12, 17, 11, 0, 0, 0, DateTimeKind.Unspecified), 8, "Held", "Starting" },
                { 16, new DateTime(2026, 11, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), 8, "Scheduled", "Assessment" }
            });

        migrationBuilder.InsertData(
            table: "EmployeeSupervisors",
            columns: new[] { "EmployeeId", "SupervisorId" },
            values: new object[,]
            {
                { 1, 3 },
                { 2, 3 },
                { 9, 3 },
                { 12, 3 },
                { 15, 3 },
                { 16, 3 }
            });

        migrationBuilder.InsertData(
            table: "EvaluationAdditionalCriteria",
            columns: new[] { "Criterion", "EvaluationId" },
            values: new object[] { "AnalyticalThinking", 8 });

        migrationBuilder.InsertData(
            table: "Evaluations",
            columns: new[] { "Id", "CompletedAt", "DueDate", "EmployeeAcquaintedOn", "EmployeeId", "EvaluatorId", "Expectations", "Goals", "PartIConfirmedAt", "PartIIAcquaintedOn", "PeriodEnd", "PeriodStart", "PreparedOn", "Sheet", "SheetConfirmedAt", "StartedAt", "Status", "WrittenEvaluationAt" },
            values: new object[,]
            {
                { 1, new DateTime(2024, 12, 10, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2024, 12, 15), new DateOnly(2022, 12, 16), 1, 3, "Samodzielne przygotowywanie analiz dla kierownictwa w uzgodnionych terminach, z opisem źródeł i ograniczeń danych.", null, new DateTime(2022, 12, 16, 15, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2024, 12, 10), new DateOnly(2024, 12, 10), new DateOnly(2022, 12, 16), new DateOnly(2024, 12, 10), "C", new DateTime(2022, 12, 11, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed", null },
                { 2, null, new DateOnly(2026, 12, 15), new DateOnly(2024, 12, 16), 1, 3, "Analizy dla kierownictwa oddawane w terminie i bez poprawek merytorycznych. Propozycje nowych wskaźników zgłaszane z własnej inicjatywy, co najmniej raz na kwartał.", "Opracowanie zestawu wskaźników realizacji zadań departamentu do końca II kwartału 2026 r.", new DateTime(2024, 12, 16, 15, 0, 0, 0, DateTimeKind.Unspecified), null, null, new DateOnly(2024, 12, 16), null, "C", new DateTime(2024, 12, 11, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 12, 10, 30, 0, 0, DateTimeKind.Unspecified), "InProgress", null },
                { 3, new DateTime(2024, 10, 28, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2024, 11, 2), new DateOnly(2022, 11, 7), 2, 3, "Zestawienia danych przygotowywane według przyjętego wzoru i sprawdzone przed przekazaniem.", null, new DateTime(2022, 11, 7, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2024, 10, 28), new DateOnly(2024, 10, 28), new DateOnly(2022, 11, 7), new DateOnly(2024, 10, 28), "C", new DateTime(2022, 11, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 10, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed", null },
                { 4, new DateTime(2025, 1, 8, 11, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 1, 10), new DateOnly(2023, 1, 16), 9, 3, "Korespondencja wydziału rejestrowana na bieżąco, odpowiedzi na proste pytania udzielane w ciągu dwóch dni.", null, new DateTime(2023, 1, 16, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 1, 8), new DateOnly(2025, 1, 8), new DateOnly(2023, 1, 16), new DateOnly(2025, 1, 8), "C", new DateTime(2023, 1, 14, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed", null },
                { 6, null, new DateOnly(2027, 1, 10), new DateOnly(2025, 1, 15), 9, 3, "Rejestr korespondencji prowadzony bez zaległości. Zestawienia danych przygotowywane samodzielnie według wskazówek, bez konieczności poprawek.", null, new DateTime(2025, 1, 15, 14, 0, 0, 0, DateTimeKind.Unspecified), null, null, new DateOnly(2025, 1, 15), null, "C", new DateTime(2025, 1, 13, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 16, 14, 0, 0, 0, DateTimeKind.Unspecified), "CriteriaSet", null },
                { 7, new DateTime(2024, 12, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2024, 12, 20), new DateOnly(2023, 1, 8), 12, 3, "Cykliczne raporty przekazywane w terminie, z krótkim omówieniem najważniejszych zmian w danych.", null, new DateTime(2023, 1, 8, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2024, 12, 18), new DateOnly(2024, 12, 18), new DateOnly(2023, 1, 8), new DateOnly(2024, 12, 18), "C", new DateTime(2022, 12, 23, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 24, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed", null },
                { 12, new DateTime(2025, 1, 14, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 1, 17), new DateOnly(2023, 2, 4), 15, 3, "Prognozy i analizy trendów przygotowywane w terminie, z opisem założeń i ograniczeń.", null, new DateTime(2023, 2, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 1, 14), new DateOnly(2025, 1, 14), new DateOnly(2023, 2, 4), new DateOnly(2025, 1, 14), "C", new DateTime(2023, 1, 21, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed", null },
                { 13, null, new DateOnly(2027, 1, 17), new DateOnly(2025, 2, 3), 15, 3, "Obliczenia w raportach wydziału sprawdzane przed wysłaniem. Wyjaśnienia metod analizy dla innych komórek udzielane w ciągu tygodnia.", null, new DateTime(2025, 2, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), null, null, new DateOnly(2025, 2, 3), null, "C", new DateTime(2025, 1, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), null, "CriteriaSet", null },
                { 14, new DateTime(2024, 11, 28, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2024, 11, 28), new DateOnly(2022, 12, 17), 16, 3, "Udostępnianie danych publicznych na wnioski w ustawowym terminie, z kompletnym uzasadnieniem odmowy.", null, new DateTime(2022, 12, 17, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2024, 11, 28), new DateOnly(2024, 11, 28), new DateOnly(2022, 12, 17), new DateOnly(2024, 11, 28), "C", new DateTime(2022, 12, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 10, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed", new DateTime(2024, 11, 20, 13, 0, 0, 0, DateTimeKind.Unspecified) },
                { 15, null, new DateOnly(2026, 11, 28), new DateOnly(2024, 12, 16), 16, 3, "Wnioski o udostępnienie danych rozpatrywane w terminie, także te wymagające uzgodnień z innymi komórkami. Rejestr udostępnionych danych prowadzony na bieżąco.", null, new DateTime(2024, 12, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), null, null, new DateOnly(2024, 12, 16), null, "C", new DateTime(2024, 12, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "InProgress", new DateTime(2026, 11, 19, 14, 30, 0, 0, DateTimeKind.Unspecified) }
            });

        migrationBuilder.InsertData(
            table: "Conversations",
            columns: new[] { "Id", "At", "EvaluationId", "Status", "Type" },
            values: new object[,]
            {
                { 1, new DateTime(2022, 12, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, "Held", "Starting" },
                { 2, new DateTime(2024, 12, 8, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, "Held", "Assessment" },
                { 3, new DateTime(2024, 12, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), 2, "Held", "Starting" },
                { 4, new DateTime(2026, 11, 27, 10, 0, 0, 0, DateTimeKind.Unspecified), 2, "Scheduled", "Assessment" },
                { 5, new DateTime(2022, 11, 7, 10, 0, 0, 0, DateTimeKind.Unspecified), 3, "Held", "Starting" },
                { 6, new DateTime(2024, 10, 27, 11, 0, 0, 0, DateTimeKind.Unspecified), 3, "Held", "Assessment" },
                { 7, new DateTime(2023, 1, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), 4, "Held", "Starting" },
                { 8, new DateTime(2025, 1, 7, 9, 30, 0, 0, DateTimeKind.Unspecified), 4, "Held", "Assessment" },
                { 11, new DateTime(2025, 1, 15, 10, 0, 0, 0, DateTimeKind.Unspecified), 6, "Held", "Starting" },
                { 12, new DateTime(2026, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), 6, "Scheduled", "Assessment" },
                { 13, new DateTime(2023, 1, 8, 10, 0, 0, 0, DateTimeKind.Unspecified), 7, "Held", "Starting" },
                { 14, new DateTime(2024, 12, 17, 12, 0, 0, 0, DateTimeKind.Unspecified), 7, "Held", "Assessment" },
                { 21, new DateTime(2023, 2, 4, 9, 0, 0, 0, DateTimeKind.Unspecified), 12, "Held", "Starting" },
                { 22, new DateTime(2025, 1, 12, 11, 0, 0, 0, DateTimeKind.Unspecified), 12, "Held", "Assessment" },
                { 23, new DateTime(2025, 2, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), 13, "Held", "Starting" },
                { 24, new DateTime(2022, 12, 17, 9, 0, 0, 0, DateTimeKind.Unspecified), 14, "Held", "Starting" },
                { 25, new DateTime(2024, 11, 17, 10, 0, 0, 0, DateTimeKind.Unspecified), 14, "Held", "Assessment" },
                { 26, new DateTime(2024, 12, 16, 9, 0, 0, 0, DateTimeKind.Unspecified), 15, "Held", "Starting" },
                { 27, new DateTime(2026, 11, 16, 11, 0, 0, 0, DateTimeKind.Unspecified), 15, "Held", "Assessment" }
            });

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
                { "AnalyticalThinking", 7, new DateTime(2024, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Communication", 7, new DateTime(2024, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Cooperation", 7, new DateTime(2024, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "KnowledgeAndDevelopment", 7, new DateTime(2024, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Reliability", 7, new DateTime(2024, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 2 },
                { "WorkOrganization", 7, new DateTime(2024, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
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
                { "WorkOrganization", 15, new DateTime(2026, 11, 19, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 }
            });

        migrationBuilder.InsertData(
            table: "EvaluationAdditionalCriteria",
            columns: new[] { "Criterion", "EvaluationId" },
            values: new object[,]
            {
                { "AnalyticalThinking", 1 },
                { "AnalyticalThinking", 2 },
                { "Creativity", 2 },
                { "AnalyticalThinking", 3 },
                { "CustomerOrientation", 4 },
                { "AnalyticalThinking", 6 },
                { "CustomerOrientation", 6 },
                { "AnalyticalThinking", 7 },
                { "AnalyticalThinking", 12 },
                { "AnalyticalThinking", 13 },
                { "AnalyticalThinking", 14 },
                { "AnalyticalThinking", 15 }
            });

        migrationBuilder.InsertData(
            table: "IndicatorRatings",
            columns: new[] { "EvaluationId", "IndicatorId", "Level" },
            values: new object[,]
            {
                { 15, 1, 4 },
                { 15, 2, 2 },
                { 15, 3, 3 },
                { 15, 4, 3 },
                { 15, 5, 3 },
                { 15, 6, 4 },
                { 15, 7, 4 },
                { 15, 8, 4 },
                { 15, 9, 3 },
                { 15, 10, 3 },
                { 15, 11, 4 },
                { 15, 12, null },
                { 15, 13, 2 },
                { 15, 14, 3 },
                { 15, 17, 4 },
                { 15, 18, 3 },
                { 15, 19, 4 },
                { 15, 20, 4 },
                { 15, 21, 3 },
                { 15, 22, 5 },
                { 15, 23, 4 },
                { 15, 24, 4 },
                { 15, 25, 3 }
            });
    }
}
