using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261121212147_AddEvaluationNotes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "EvaluationNotes",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                EvaluationId = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                Text = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EvaluationNotes", x => x.Id);
                table.ForeignKey(
                    name: "FK_EvaluationNotes_Evaluations_EvaluationId",
                    column: x => x.EvaluationId,
                    principalTable: "Evaluations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "EvaluationNotes",
            columns: new[] { "Id", "CreatedAt", "EvaluationId", "Text" },
            values: new object[,]
            {
                { 1, new DateTime(2026, 5, 14, 15, 20, 0, 0, DateTimeKind.Unspecified), 10, "Przygotowała zestaw wskaźników do sprawozdania kwartalnego tydzień przed terminem. Kierownictwo departamentu przyjęło go bez poprawek." },
                { 2, new DateTime(2026, 9, 8, 11, 0, 0, 0, DateTimeKind.Unspecified), 10, "Z własnej inicjatywy poprowadziła dla wydziału warsztat z weryfikacji źródeł danych. Uczestnicy ocenili go jako przydatny." },
                { 3, new DateTime(2026, 3, 10, 9, 45, 0, 0, DateTimeKind.Unspecified), 12, "Prognoza zatrudnienia na 2027 r. wymagała dwóch poprawek metodologicznych. Po wskazówkach przygotował poprawioną wersję w terminie." },
                { 4, new DateTime(2026, 10, 2, 14, 10, 0, 0, DateTimeKind.Unspecified), 12, "Samodzielnie wykrył błąd w obliczeniach raportu rocznego przed jego wysłaniem do dyrektora i zaproponował dodatkowy krok kontroli." }
            });

        migrationBuilder.CreateIndex(
            name: "IX_EvaluationNotes_EvaluationId_CreatedAt",
            table: "EvaluationNotes",
            columns: new[] { "EvaluationId", "CreatedAt" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "EvaluationNotes");
    }
}
