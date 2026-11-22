using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261121233824_AddNoteCriterion : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Criterion",
            table: "EvaluationNotes",
            type: "nvarchar(40)",
            maxLength: 40,
            nullable: false,
            defaultValue: "");

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 1,
            column: "Criterion",
            value: "WorkOrganization");

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 2,
            column: "Criterion",
            value: "KnowledgeAndDevelopment");

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 3,
            column: "Criterion",
            value: "Reliability");

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 4,
            column: "Criterion",
            value: "AnalyticalThinking");

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 5,
            column: "Criterion",
            value: "Cooperation");

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 6,
            column: "Criterion",
            value: "Reliability");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Criterion",
            table: "EvaluationNotes");
    }
}
