using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261121182425_AddConversationStatus : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "ConversationStatus",
            table: "Evaluations",
            type: "nvarchar(30)",
            maxLength: 30,
            nullable: true);

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 1,
            column: "ConversationStatus",
            value: "Held");

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 2,
            column: "ConversationStatus",
            value: "AcceptedByEmployee");

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 3,
            column: "ConversationStatus",
            value: "Held");

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 4,
            column: "ConversationStatus",
            value: "Held");

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 5,
            column: "ConversationStatus",
            value: "Held");

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 6,
            column: "ConversationStatus",
            value: "Scheduled");

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 7,
            column: "ConversationStatus",
            value: "Held");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "ConversationStatus",
            table: "Evaluations");
    }
}
