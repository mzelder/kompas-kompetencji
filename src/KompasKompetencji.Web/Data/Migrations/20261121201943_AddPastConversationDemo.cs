using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261121201943_AddPastConversationDemo : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "Employees",
            columns: new[] { "Id", "Department", "EmployedSince", "FirstName", "IsSupervisor", "LastName", "Position" },
            values: new object[] { 13, "Wydział Analiz Danych", new DateOnly(2020, 12, 10), "Łukasz", false, "Grabowski", "Specjalista" });

        migrationBuilder.InsertData(
            table: "EmployeeSupervisors",
            columns: new[] { "EmployeeId", "SupervisorId" },
            values: new object[] { 13, 3 });

        migrationBuilder.InsertData(
            table: "Evaluations",
            columns: new[] { "Id", "CompletedAt", "ConversationAt", "ConversationStatus", "DueDate", "EmployeeId", "EvaluatorId", "PeriodEnd", "PeriodStart", "Sheet", "StartedAt", "Status" },
            values: new object[] { 8, null, new DateTime(2026, 11, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), "Scheduled", new DateOnly(2026, 12, 10), 13, 3, new DateOnly(2026, 12, 9), new DateOnly(2025, 12, 10), "C", null, "NotStarted" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 13, 3 });

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 8);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 13);
    }
}
