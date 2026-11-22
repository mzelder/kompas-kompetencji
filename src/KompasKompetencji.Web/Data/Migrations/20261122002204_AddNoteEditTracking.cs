using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261122002204_AddNoteEditTracking : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "ClassificationEditedAt",
            table: "EvaluationNotes",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "TextEditedAt",
            table: "EvaluationNotes",
            type: "datetime2",
            nullable: true);

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 1,
            columns: new[] { "ClassificationEditedAt", "TextEditedAt" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 2,
            columns: new[] { "ClassificationEditedAt", "TextEditedAt" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 3,
            columns: new[] { "ClassificationEditedAt", "TextEditedAt" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 4,
            columns: new[] { "ClassificationEditedAt", "TextEditedAt" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 5,
            columns: new[] { "ClassificationEditedAt", "TextEditedAt" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 6,
            columns: new[] { "ClassificationEditedAt", "TextEditedAt" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 7,
            columns: new[] { "ClassificationEditedAt", "TextEditedAt" },
            values: new object[] { null, null });

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 8,
            columns: new[] { "ClassificationEditedAt", "TextEditedAt" },
            values: new object[] { null, null });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "ClassificationEditedAt",
            table: "EvaluationNotes");

        migrationBuilder.DropColumn(
            name: "TextEditedAt",
            table: "EvaluationNotes");
    }
}
