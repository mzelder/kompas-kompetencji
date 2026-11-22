using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261122062258_AddMonikaSikoraRatingsAndNotes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "EvaluationNotes",
            columns: new[] { "Id", "ClassificationEditedAt", "CreatedAt", "Criterion", "EvaluationId", "Text", "TextEditedAt", "Tone" },
            values: new object[,]
            {
                { 10, null, new DateTime(2025, 2, 11, 10, 30, 0, 0, DateTimeKind.Unspecified), "Reliability", 15, "Przy wniosku o dane z rejestru zawierającego dane osobowe sama ustaliła, które zasady anonimizacji mają zastosowanie, i przed wysyłką dodała drugą kontrolę zestawienia.", null, "Positive" },
                { 11, null, new DateTime(2025, 5, 20, 14, 0, 0, 0, DateTimeKind.Unspecified), "KnowledgeAndDevelopment", 15, "Przy nietypowym wniosku o dane z trzech rejestrów połączyła znajomość przepisów o dostępie do informacji publicznej z metodami łączenia danych i wyjaśniła, dlaczego części danych nie da się zestawić.", null, "Positive" },
                { 12, null, new DateTime(2025, 9, 16, 9, 45, 0, 0, DateTimeKind.Unspecified), "Cooperation", 15, "Z własnej inicjatywy przygotowała dla wydziału instrukcję obsługi wniosków o dane z rejestru i odpowiadała na pytania nowych osób. Po kolejnych pytaniach uzupełniła instrukcję o przykłady.", null, "Positive" },
                { 13, null, new DateTime(2025, 10, 7, 15, 10, 0, 0, DateTimeKind.Unspecified), "Reliability", 15, "Dwie odpowiedzi na wnioski o dane przygotowała dopiero po przypomnieniu, w ostatnim dniu terminu. O tym, że czeka na dane z innej komórki, poinformowała dzień przed terminem.", null, "Negative" },
                { 14, null, new DateTime(2026, 1, 21, 11, 20, 0, 0, DateTimeKind.Unspecified), "Communication", 15, "Odpowiedzi na wnioski są poprawne i podają termin przekazania brakujących danych, ale przy złożonych sprawach wnioskodawcy dopytują o kontekst i znaczenie skrótów.", null, "Neutral" },
                { 15, null, new DateTime(2026, 3, 10, 13, 0, 0, 0, DateTimeKind.Unspecified), "KnowledgeAndDevelopment", 15, "Przed zmianą przepisów o ponownym wykorzystywaniu informacji sama zapisała się na szkolenie, a potem sprawdziła na kilku wnioskach, czy nowy wzór odpowiedzi skraca ich obsługę.", null, "Positive" },
                { 16, null, new DateTime(2026, 4, 15, 16, 0, 0, 0, DateTimeKind.Unspecified), "WorkOrganization", 15, "Po pilnym zleceniu raportu dla ministerstwa przesunęła obsługę wniosków, ale o skutkach dla ich terminów poinformowała dopiero po pytaniu naczelnika.", null, "Negative" },
                { 17, null, new DateTime(2026, 6, 9, 10, 15, 0, 0, DateTimeKind.Unspecified), "AnalyticalThinking", 15, "Zauważyła, że wzrost liczby wniosków o dane zbiega się ze zmianą strony BIP. Sprawdziła to na danych z dwóch lat i odrzuciła wyjaśnienie sezonowe.", null, "Positive" },
                { 18, null, new DateTime(2026, 8, 26, 12, 30, 0, 0, DateTimeKind.Unspecified), "Communication", 15, "Na spotkaniu z Biurem Prawnym uzasadniła odmowę udostępnienia części danych, odpowiedziała na zastrzeżenia i wyjaśniła ograniczenia danych z rejestru.", null, "Positive" },
                { 19, null, new DateTime(2026, 9, 22, 9, 0, 0, 0, DateTimeKind.Unspecified), "WorkOrganization", 15, "Przy trzech wnioskach z tym samym terminem porównała skutki opóźnienia każdego z nich i uzgodniła z naczelnikiem kolejność obsługi.", null, "Positive" }
            });

        migrationBuilder.InsertData(
            table: "IndicatorRatings",
            columns: new[] { "EvaluationId", "IndicatorId", "Level" },
            values: new object[,]
            {
                { 15, 1, 4 },
                { 15, 2, 2 },
                { 15, 3, 3 },
                { 15, 4, 3 },
                { 15, 5, 3 },
                { 15, 6, 4 },
                { 15, 7, 4 },
                { 15, 8, 4 },
                { 15, 9, 3 },
                { 15, 10, 3 },
                { 15, 11, 4 },
                { 15, 12, null },
                { 15, 13, 2 },
                { 15, 14, 3 },
                { 15, 17, 4 },
                { 15, 18, 3 },
                { 15, 19, 4 },
                { 15, 20, 4 },
                { 15, 21, 3 },
                { 15, 22, 5 },
                { 15, 23, 4 },
                { 15, 24, 4 },
                { 15, 25, 3 }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 10);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 11);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 12);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 13);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 14);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 15);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 16);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 17);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 18);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 19);

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 1 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 2 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 3 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 4 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 5 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 6 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 7 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 8 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 9 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 10 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 11 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 12 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 13 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 14 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 17 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 18 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 19 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 20 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 21 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 22 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 23 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 24 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 15, 25 });
    }
}
