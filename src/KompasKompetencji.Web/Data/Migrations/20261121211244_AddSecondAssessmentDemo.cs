using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261121211244_AddSecondAssessmentDemo : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "Employees",
            columns: new[] { "Id", "Department", "EmployedSince", "FirstName", "IsSupervisor", "LastName", "Position" },
            values: new object[] { 15, "Wydział Analiz Danych", new DateOnly(2022, 1, 17), "Bartosz", false, "Wróbel", "Starszy specjalista" });

        migrationBuilder.InsertData(
            table: "EmployeeSupervisors",
            columns: new[] { "EmployeeId", "SupervisorId" },
            values: new object[] { 15, 3 });

        migrationBuilder.InsertData(
            table: "Evaluations",
            columns: new[] { "Id", "CompletedAt", "ConversationAt", "ConversationStatus", "CriteriaConfirmedAt", "DueDate", "EmployeeId", "EvaluatorId", "PeriodEnd", "PeriodStart", "Sheet", "SheetConfirmedAt", "StartedAt", "Status" },
            values: new object[,]
            {
                { 11, new DateTime(2026, 1, 14, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 12, 11, 0, 0, 0, DateTimeKind.Unspecified), "Held", new DateTime(2025, 2, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 1, 17), 15, 3, new DateOnly(2026, 1, 16), new DateOnly(2025, 1, 17), "C", new DateTime(2025, 1, 21, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed" },
                { 12, null, null, null, new DateTime(2026, 2, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2027, 1, 17), 15, 3, new DateOnly(2027, 1, 16), new DateOnly(2026, 1, 17), "C", new DateTime(2026, 1, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), null, "CriteriaSet" }
            });

        migrationBuilder.InsertData(
            table: "JobDescriptions",
            columns: new[] { "EmployeeId", "Purpose", "RequiredCompetencies", "SubordinatesCount", "Tasks" },
            values: new object[] { 15, "Przygotowywanie analiz statystycznych i prognoz na potrzeby planowania zadań departamentu.", "Myślenie analityczne\nRzetelność\nKomunikacja\nOrientacja na cele", null, "Przygotowywanie prognoz i analiz trendów na podstawie danych wydziału\nOpracowywanie zestawień do planów i sprawozdań departamentu\nWeryfikacja poprawności obliczeń w raportach przygotowywanych przez wydział\nUdzielanie wyjaśnień dotyczących metod analizy innym komórkom" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 15, 3 });

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 11);

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 12);

        migrationBuilder.DeleteData(
            table: "JobDescriptions",
            keyColumn: "EmployeeId",
            keyValue: 15);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 15);
    }
}
