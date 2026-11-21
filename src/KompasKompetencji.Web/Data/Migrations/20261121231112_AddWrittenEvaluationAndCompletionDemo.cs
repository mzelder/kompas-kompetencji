using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261121231112_AddWrittenEvaluationAndCompletionDemo : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "WrittenEvaluationAt",
            table: "Evaluations",
            type: "datetime2",
            nullable: true);

        migrationBuilder.InsertData(
            table: "Employees",
            columns: new[] { "Id", "Department", "EmployedSince", "FirstName", "IsSupervisor", "LastName", "Position" },
            values: new object[] { 16, "Wydział Analiz Danych", new DateOnly(2019, 11, 28), "Monika", false, "Sikora", "Specjalista" });

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 1,
            column: "WrittenEvaluationAt",
            value: null);

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 2,
            column: "WrittenEvaluationAt",
            value: null);

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 3,
            column: "WrittenEvaluationAt",
            value: null);

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 4,
            column: "WrittenEvaluationAt",
            value: null);

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 5,
            column: "WrittenEvaluationAt",
            value: null);

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 6,
            column: "WrittenEvaluationAt",
            value: null);

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 7,
            column: "WrittenEvaluationAt",
            value: null);

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 8,
            column: "WrittenEvaluationAt",
            value: null);

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 9,
            column: "WrittenEvaluationAt",
            value: null);

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 10,
            column: "WrittenEvaluationAt",
            value: null);

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 11,
            column: "WrittenEvaluationAt",
            value: null);

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 12,
            column: "WrittenEvaluationAt",
            value: null);

        migrationBuilder.InsertData(
            table: "EmployeeSupervisors",
            columns: new[] { "EmployeeId", "SupervisorId" },
            values: new object[] { 16, 3 });

        migrationBuilder.InsertData(
            table: "Evaluations",
            columns: new[] { "Id", "CompletedAt", "ConversationAt", "ConversationStatus", "CriteriaConfirmedAt", "DueDate", "EmployeeId", "EvaluatorId", "PeriodEnd", "PeriodStart", "Sheet", "SheetConfirmedAt", "StartedAt", "Status", "WrittenEvaluationAt" },
            values: new object[,]
            {
                { 13, new DateTime(2025, 11, 28, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 17, 10, 0, 0, 0, DateTimeKind.Unspecified), "Held", new DateTime(2024, 12, 17, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 11, 28), 16, 3, new DateOnly(2025, 11, 27), new DateOnly(2024, 11, 28), "C", new DateTime(2024, 12, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed", new DateTime(2025, 11, 20, 13, 0, 0, 0, DateTimeKind.Unspecified) },
                { 14, null, new DateTime(2026, 11, 16, 11, 0, 0, 0, DateTimeKind.Unspecified), "Held", new DateTime(2025, 12, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 11, 28), 16, 3, new DateOnly(2026, 11, 27), new DateOnly(2025, 11, 28), "C", new DateTime(2025, 12, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "InProgress", new DateTime(2026, 11, 19, 14, 30, 0, 0, DateTimeKind.Unspecified) }
            });

        migrationBuilder.InsertData(
            table: "JobDescriptions",
            columns: new[] { "EmployeeId", "Purpose", "RequiredCompetencies", "SubordinatesCount", "Tasks" },
            values: new object[] { 16, "Obsługa wniosków o udostępnienie danych i przygotowywanie zestawień dla podmiotów zewnętrznych.", "Rzetelność\nKomunikacja pisemna\nWspółpraca", null, "Rozpatrywanie wniosków o udostępnienie danych wydziału\nPrzygotowywanie zestawień danych dla podmiotów zewnętrznych\nProwadzenie rejestru udostępnionych danych\nWspółpraca z działem prawnym przy odpowiedziach na wnioski" });

        migrationBuilder.InsertData(
            table: "EvaluationNotes",
            columns: new[] { "Id", "CreatedAt", "EvaluationId", "Text" },
            values: new object[,]
            {
                { 5, new DateTime(2026, 3, 4, 10, 15, 0, 0, DateTimeKind.Unspecified), 14, "Rozpatrzyła wszystkie wnioski z pierwszego kwartału w terminie, w tym dwa wymagające uzgodnień z działem prawnym." },
                { 6, new DateTime(2026, 8, 21, 13, 40, 0, 0, DateTimeKind.Unspecified), 14, "Uporządkowała rejestr udostępnionych danych i zaproponowała wzór odpowiedzi skracający obsługę typowych wniosków." }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 16, 3 });

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 5);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 6);

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 13);

        migrationBuilder.DeleteData(
            table: "JobDescriptions",
            keyColumn: "EmployeeId",
            keyValue: 16);

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 14);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 16);

        migrationBuilder.DropColumn(
            name: "WrittenEvaluationAt",
            table: "Evaluations");
    }
}
