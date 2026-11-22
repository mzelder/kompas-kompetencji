using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261122003620_TrimDemoNotes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 3);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 4);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 5);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 6);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 7);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 8);

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 1,
            columns: new[] { "CreatedAt", "Criterion", "Text" },
            values: new object[] { new DateTime(2026, 9, 8, 11, 0, 0, 0, DateTimeKind.Unspecified), "KnowledgeAndDevelopment", "Z własnej inicjatywy poprowadziła dla wydziału warsztat z weryfikacji źródeł danych. Uczestnicy ocenili go jako przydatny." });

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 2,
            columns: new[] { "CreatedAt", "Criterion", "Text", "Tone" },
            values: new object[] { new DateTime(2026, 10, 14, 13, 20, 0, 0, DateTimeKind.Unspecified), "Reliability", "Przekazała kierownictwu zestawienie kwartalne z błędnymi sumami w dwóch tabelach. Poprawioną wersję trzeba było rozesłać ponownie.", "Negative" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 1,
            columns: new[] { "CreatedAt", "Criterion", "Text" },
            values: new object[] { new DateTime(2026, 5, 14, 15, 20, 0, 0, DateTimeKind.Unspecified), "WorkOrganization", "Przygotowała zestaw wskaźników do sprawozdania kwartalnego tydzień przed terminem. Kierownictwo departamentu przyjęło go bez poprawek." });

        migrationBuilder.UpdateData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 2,
            columns: new[] { "CreatedAt", "Criterion", "Text", "Tone" },
            values: new object[] { new DateTime(2026, 9, 8, 11, 0, 0, 0, DateTimeKind.Unspecified), "KnowledgeAndDevelopment", "Z własnej inicjatywy poprowadziła dla wydziału warsztat z weryfikacji źródeł danych. Uczestnicy ocenili go jako przydatny.", "Positive" });

        migrationBuilder.InsertData(
            table: "EvaluationNotes",
            columns: new[] { "Id", "ClassificationEditedAt", "CreatedAt", "Criterion", "EvaluationId", "Text", "TextEditedAt", "Tone" },
            values: new object[,]
            {
                { 3, null, new DateTime(2026, 3, 10, 9, 45, 0, 0, DateTimeKind.Unspecified), "Reliability", 13, "Prognoza zatrudnienia na 2027 r. wymagała dwóch poprawek metodologicznych. Po wskazówkach przygotował poprawioną wersję w terminie.", null, "Neutral" },
                { 4, null, new DateTime(2026, 10, 2, 14, 10, 0, 0, DateTimeKind.Unspecified), "AnalyticalThinking", 13, "Samodzielnie wykrył błąd w obliczeniach raportu rocznego przed jego wysłaniem do dyrektora i zaproponował dodatkowy krok kontroli.", null, "Positive" },
                { 5, null, new DateTime(2026, 3, 4, 10, 15, 0, 0, DateTimeKind.Unspecified), "Cooperation", 15, "Rozpatrzyła wszystkie wnioski z pierwszego kwartału w terminie, w tym dwa wymagające uzgodnień z działem prawnym.", null, "Positive" },
                { 6, null, new DateTime(2026, 8, 21, 13, 40, 0, 0, DateTimeKind.Unspecified), "Reliability", 15, "Uporządkowała rejestr udostępnionych danych i zaproponowała wzór odpowiedzi skracający obsługę typowych wniosków.", null, "Positive" },
                { 7, null, new DateTime(2026, 6, 12, 16, 5, 0, 0, DateTimeKind.Unspecified), "WorkOrganization", 13, "Dwukrotnie przekazał zestawienie miesięczne po terminie, bez wcześniejszej informacji o opóźnieniu.", null, "Negative" },
                { 8, null, new DateTime(2026, 10, 20, 9, 30, 0, 0, DateTimeKind.Unspecified), null, 11, "Zgłosiła chęć udziału w przyszłorocznym projekcie międzyresortowym.", null, "Neutral" }
            });
    }
}
