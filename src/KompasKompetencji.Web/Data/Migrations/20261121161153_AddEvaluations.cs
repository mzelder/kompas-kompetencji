using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261121161153_AddEvaluations : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateOnly>(
            name: "EmployedSince",
            table: "Employees",
            type: "date",
            nullable: false,
            defaultValue: new DateOnly(1, 1, 1));

        migrationBuilder.CreateTable(
            name: "Evaluations",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                EmployeeId = table.Column<int>(type: "int", nullable: false),
                EvaluatorId = table.Column<int>(type: "int", nullable: false),
                Sheet = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                PeriodStart = table.Column<DateOnly>(type: "date", nullable: false),
                PeriodEnd = table.Column<DateOnly>(type: "date", nullable: false),
                DueDate = table.Column<DateOnly>(type: "date", nullable: false),
                ConversationAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Evaluations", x => x.Id);
                table.ForeignKey(
                    name: "FK_Evaluations_Employees_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Employees",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Evaluations_Employees_EvaluatorId",
                    column: x => x.EvaluatorId,
                    principalTable: "Employees",
                    principalColumn: "Id");
            });

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 1,
            column: "EmployedSince",
            value: new DateOnly(2019, 12, 15));

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 2,
            column: "EmployedSince",
            value: new DateOnly(2022, 11, 2));

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 3,
            column: "EmployedSince",
            value: new DateOnly(2015, 4, 1));

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 4,
            column: "EmployedSince",
            value: new DateOnly(2012, 9, 1));

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 5,
            column: "EmployedSince",
            value: new DateOnly(2016, 2, 15));

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 6,
            column: "EmployedSince",
            value: new DateOnly(2018, 5, 1));

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 7,
            column: "EmployedSince",
            value: new DateOnly(2021, 9, 1));

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 8,
            column: "EmployedSince",
            value: new DateOnly(2024, 3, 1));

        migrationBuilder.InsertData(
            table: "Employees",
            columns: new[] { "Id", "Department", "EmployedSince", "FirstName", "IsSupervisor", "LastName", "Position" },
            values: new object[,]
            {
                { 9, "Wydział Analiz Danych", new DateOnly(2018, 1, 10), "Michał", false, "Szymański", "Referent" },
                { 10, "Wydział Analiz Danych", new DateOnly(2020, 6, 1), "Agnieszka", false, "Woźniak", "Starszy specjalista" },
                { 11, "Wydział Analiz Danych", new DateOnly(2026, 7, 1), "Paweł", false, "Kozłowski", "Specjalista" },
                { 12, "Wydział Analiz Danych", new DateOnly(2021, 12, 20), "Karolina", false, "Mazur", "Specjalista" }
            });

        migrationBuilder.InsertData(
            table: "Evaluations",
            columns: new[] { "Id", "CompletedAt", "ConversationAt", "DueDate", "EmployeeId", "EvaluatorId", "PeriodEnd", "PeriodStart", "Sheet", "StartedAt", "Status" },
            values: new object[,]
            {
                { 1, new DateTime(2025, 12, 10, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 8, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 12, 15), 1, 3, new DateOnly(2025, 12, 14), new DateOnly(2024, 12, 15), "C", new DateTime(2025, 11, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed" },
                { 2, null, new DateTime(2026, 11, 27, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 12, 15), 1, 3, new DateOnly(2026, 12, 14), new DateOnly(2025, 12, 15), "C", new DateTime(2026, 11, 12, 10, 30, 0, 0, DateTimeKind.Unspecified), "InProgress" },
                { 3, new DateTime(2025, 10, 28, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 27, 11, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 11, 2), 2, 3, new DateOnly(2025, 11, 1), new DateOnly(2024, 11, 2), "C", new DateTime(2025, 10, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed" }
            });

        migrationBuilder.InsertData(
            table: "EmployeeSupervisors",
            columns: new[] { "EmployeeId", "SupervisorId" },
            values: new object[,]
            {
                { 9, 3 },
                { 10, 3 },
                { 11, 3 },
                { 12, 3 }
            });

        migrationBuilder.InsertData(
            table: "Evaluations",
            columns: new[] { "Id", "CompletedAt", "ConversationAt", "DueDate", "EmployeeId", "EvaluatorId", "PeriodEnd", "PeriodStart", "Sheet", "StartedAt", "Status" },
            values: new object[,]
            {
                { 4, new DateTime(2026, 1, 8, 11, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 7, 9, 30, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 1, 10), 9, 3, new DateOnly(2026, 1, 9), new DateOnly(2025, 1, 10), "C", new DateTime(2025, 12, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed" },
                { 5, new DateTime(2026, 5, 27, 13, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 26, 13, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 6, 1), 10, 3, new DateOnly(2026, 5, 31), new DateOnly(2025, 6, 1), "C", new DateTime(2026, 5, 4, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed" },
                { 6, null, new DateTime(2026, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2027, 1, 10), 9, 3, new DateOnly(2027, 1, 9), new DateOnly(2026, 1, 10), "C", new DateTime(2026, 11, 16, 14, 0, 0, 0, DateTimeKind.Unspecified), "CriteriaSet" },
                { 7, new DateTime(2025, 12, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 17, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 12, 20), 12, 3, new DateOnly(2025, 12, 19), new DateOnly(2024, 12, 20), "C", new DateTime(2025, 11, 24, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed" }
            });

        migrationBuilder.CreateIndex(
            name: "IX_Evaluations_EmployeeId_DueDate",
            table: "Evaluations",
            columns: new[] { "EmployeeId", "DueDate" });

        migrationBuilder.CreateIndex(
            name: "IX_Evaluations_EvaluatorId",
            table: "Evaluations",
            column: "EvaluatorId");

        migrationBuilder.Sql("UPDATE Employees SET EmployedSince = CAST(GETDATE() AS date) WHERE EmployedSince = '0001-01-01'");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Evaluations");

        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 9, 3 });

        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 10, 3 });

        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 11, 3 });

        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 12, 3 });

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 9);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 10);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 11);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 12);

        migrationBuilder.DropColumn(
            name: "EmployedSince",
            table: "Employees");
    }
}
