using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261121192200_MergeConversationStatuses : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 2,
            column: "ConversationStatus",
            value: "Scheduled");

        migrationBuilder.Sql("UPDATE Evaluations SET ConversationStatus = 'Scheduled' WHERE ConversationStatus = 'AcceptedByEmployee'");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 2,
            column: "ConversationStatus",
            value: "AcceptedByEmployee");
    }
}
