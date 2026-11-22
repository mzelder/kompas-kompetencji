using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261122074020_AddFullProcessAndOverdueDemo : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "JobPositions",
            columns: new[] { "Id", "Department", "DirectSupervisorTitle", "IndirectSupervisorTitle", "Purpose", "RequiredCompetencies", "Subordinates", "Symbol", "Tasks", "Title", "Unit" },
            values: new object[,]
            {
                { 2, "Departament Danych i Usług Cyfrowych", "Naczelnik wydziału", "Dyrektor departamentu", "Realizacja zadań związanych z gromadzeniem, przetwarzaniem i udostępnianiem danych na potrzeby wydziału.", "Myślenie analityczne\nRzetelność\nKomunikacja\nWspółpraca", "None", "DDU WAD 7", "Gromadzenie i porządkowanie danych z jednostek podległych\nPrzygotowywanie zestawień i wizualizacji danych\nUdzielanie odpowiedzi na zapytania dotyczące danych publicznych\nProwadzenie dokumentacji źródeł danych", "Specjalista", "Wydział Analiz Danych" },
                { 7, "Departament Danych i Usług Cyfrowych", "Naczelnik wydziału", "Dyrektor departamentu", "Realizacja zadań związanych z analizą danych i przygotowywaniem informacji dla kierownictwa.", "Myślenie analityczne\nRzetelność\nKomunikacja pisemna", "None", "DDU WAD 8", "Przygotowywanie cyklicznych raportów z danych wydziału\nAnaliza danych ankietowych i statystycznych\nPrzygotowywanie projektów odpowiedzi na pisma\nAktualizacja danych publikowanych w serwisie urzędu", "Specjalista", "Wydział Analiz Danych" }
            });

        migrationBuilder.InsertData(
            table: "Employees",
            columns: new[] { "Id", "Department", "EmployedSince", "FirstName", "IsSupervisor", "JobPositionId", "LastName", "Position" },
            values: new object[,]
            {
                { 2, "Wydział Analiz Danych", new DateOnly(2025, 1, 11), "Jan", false, 2, "Nowak", "Specjalista" },
                { 12, "Wydział Analiz Danych", new DateOnly(2024, 11, 15), "Karolina", false, 7, "Mazur", "Specjalista" }
            });

        migrationBuilder.InsertData(
            table: "EmployeeSupervisors",
            columns: new[] { "EmployeeId", "SupervisorId" },
            values: new object[,]
            {
                { 2, 3 },
                { 12, 3 }
            });

        migrationBuilder.InsertData(
            table: "Evaluations",
            columns: new[] { "Id", "CompletedAt", "DueDate", "EmployeeAcquaintedOn", "EmployeeId", "EvaluatorId", "Expectations", "Goals", "PartIConfirmedAt", "PartIIAcquaintedOn", "PeriodEnd", "PeriodStart", "PreparedOn", "Sheet", "SheetConfirmedAt", "StartedAt", "Status", "WrittenEvaluationAt" },
            values: new object[] { 6, null, new DateOnly(2026, 11, 15), new DateOnly(2024, 12, 2), 12, 3, "Cykliczne raporty przekazywane w terminie, z krótkim omówieniem najważniejszych zmian w danych.", null, new DateTime(2024, 12, 2, 14, 0, 0, 0, DateTimeKind.Unspecified), null, null, new DateOnly(2024, 12, 2), null, "C", new DateTime(2024, 11, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), null, "CriteriaSet", null });

        migrationBuilder.InsertData(
            table: "Conversations",
            columns: new[] { "Id", "At", "EvaluationId", "Status", "Type" },
            values: new object[,]
            {
                { 1, new DateTime(2024, 12, 2, 10, 0, 0, 0, DateTimeKind.Unspecified), 6, "Held", "Starting" },
                { 2, new DateTime(2026, 11, 26, 10, 0, 0, 0, DateTimeKind.Unspecified), 6, "Scheduled", "Assessment" }
            });

        migrationBuilder.InsertData(
            table: "EvaluationAdditionalCriteria",
            columns: new[] { "Criterion", "EvaluationId" },
            values: new object[] { "AnalyticalThinking", 6 });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
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
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 2, 3 });

        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 12, 3 });

        migrationBuilder.DeleteData(
            table: "EvaluationAdditionalCriteria",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 6 });

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 2);

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 6);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 12);

        migrationBuilder.DeleteData(
            table: "JobPositions",
            keyColumn: "Id",
            keyValue: 2);

        migrationBuilder.DeleteData(
            table: "JobPositions",
            keyColumn: "Id",
            keyValue: 7);
    }
}
