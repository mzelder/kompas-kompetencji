using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261121210508_AddCriteriaConfirmationAndAssessmentDemo : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "CriteriaConfirmedAt",
            table: "Evaluations",
            type: "datetime2",
            nullable: true);

        migrationBuilder.InsertData(
            table: "Employees",
            columns: new[] { "Id", "Department", "EmployedSince", "FirstName", "IsSupervisor", "LastName", "Position" },
            values: new object[] { 14, "Wydział Analiz Danych", new DateOnly(2023, 4, 3), "Natalia", false, "Krawczyk", "Główny specjalista" });

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 1,
            column: "CriteriaConfirmedAt",
            value: null);

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 2,
            column: "CriteriaConfirmedAt",
            value: null);

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 3,
            column: "CriteriaConfirmedAt",
            value: null);

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 4,
            column: "CriteriaConfirmedAt",
            value: null);

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 5,
            column: "CriteriaConfirmedAt",
            value: null);

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 6,
            column: "CriteriaConfirmedAt",
            value: null);

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 7,
            column: "CriteriaConfirmedAt",
            value: null);

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 8,
            column: "CriteriaConfirmedAt",
            value: null);

        migrationBuilder.InsertData(
            table: "EmployeeSupervisors",
            columns: new[] { "EmployeeId", "SupervisorId" },
            values: new object[] { 14, 3 });

        migrationBuilder.InsertData(
            table: "Evaluations",
            columns: new[] { "Id", "CompletedAt", "ConversationAt", "ConversationStatus", "CriteriaConfirmedAt", "DueDate", "EmployeeId", "EvaluatorId", "PeriodEnd", "PeriodStart", "Sheet", "SheetConfirmedAt", "StartedAt", "Status" },
            values: new object[,]
            {
                { 9, new DateTime(2026, 3, 30, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 25, 10, 0, 0, 0, DateTimeKind.Unspecified), "Held", new DateTime(2025, 4, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 4, 3), 14, 3, new DateOnly(2026, 4, 2), new DateOnly(2025, 4, 3), "C", new DateTime(2025, 4, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed" },
                { 10, null, null, null, new DateTime(2026, 4, 21, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2027, 4, 3), 14, 3, new DateOnly(2027, 4, 2), new DateOnly(2026, 4, 3), "C", new DateTime(2026, 4, 7, 9, 0, 0, 0, DateTimeKind.Unspecified), null, "CriteriaSet" }
            });

        migrationBuilder.InsertData(
            table: "JobDescriptions",
            columns: new[] { "EmployeeId", "Purpose", "RequiredCompetencies", "SubordinatesCount", "Tasks" },
            values: new object[] { 14, "Projektowanie i utrzymanie zestawów wskaźników wydziału oraz zapewnienie spójności metod analizy danych.", "Myślenie analityczne\nOrientacja na cele\nWspółpraca\nKomunikacja pisemna", null, "Projektowanie wskaźników i metod pomiaru dla zadań departamentu\nPrzygotowywanie analiz porównawczych na potrzeby kierownictwa\nOpracowywanie standardów opisu i weryfikacji źródeł danych\nKoordynowanie prac analitycznych realizowanych przez kilka osób wydziału" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 14, 3 });

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 9);

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 10);

        migrationBuilder.DeleteData(
            table: "JobDescriptions",
            keyColumn: "EmployeeId",
            keyValue: 14);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 14);

        migrationBuilder.DropColumn(
            name: "CriteriaConfirmedAt",
            table: "Evaluations");
    }
}
