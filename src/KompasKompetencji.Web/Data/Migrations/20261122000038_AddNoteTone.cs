using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261122000038_AddNoteTone : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Criterion",
            table: "EvaluationNotes",
            type: "nvarchar(40)",
            maxLength: 40,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(40)",
            oldMaxLength: 40);

        migrationBuilder.AddColumn<string>(
            name: "Tone",
            table: "EvaluationNotes",
            type: "nvarchar(20)",
            maxLength: 20,
            nullable: false,
            defaultValue: "");

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 1,
            column: "Tone",
            value: "Positive");

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 2,
            column: "Tone",
            value: "Positive");

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 3,
            column: "Tone",
            value: "Neutral");

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 4,
            column: "Tone",
            value: "Positive");

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 5,
            column: "Tone",
            value: "Positive");

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 6,
            column: "Tone",
            value: "Positive");

        migrationBuilder.InsertData(
            table: "EvaluationNotes",
            columns: new[] { "Id", "CreatedAt", "Criterion", "EvaluationId", "Text", "Tone" },
            values: new object[,]
            {
                { 7, new DateTime(2026, 6, 12, 16, 5, 0, 0, DateTimeKind.Unspecified), "WorkOrganization", 13, "Dwukrotnie przekazał zestawienie miesięczne po terminie, bez wcześniejszej informacji o opóźnieniu.", "Negative" },
                { 8, new DateTime(2026, 10, 20, 9, 30, 0, 0, DateTimeKind.Unspecified), null, 11, "Zgłosiła chęć udziału w przyszłorocznym projekcie międzyresortowym.", "Neutral" }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 7);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 8);

        migrationBuilder.DropColumn(
            name: "Tone",
            table: "EvaluationNotes");

        migrationBuilder.AlterColumn<string>(
            name: "Criterion",
            table: "EvaluationNotes",
            type: "nvarchar(40)",
            maxLength: 40,
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(40)",
            oldMaxLength: 40,
            oldNullable: true);
    }
}
