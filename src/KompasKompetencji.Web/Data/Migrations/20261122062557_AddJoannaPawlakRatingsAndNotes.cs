using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261122062557_AddJoannaPawlakRatingsAndNotes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "CriterionDecisions",
            columns: new[] { "Criterion", "EvaluationId", "DecidedAt", "Justification", "Level", "SuggestedLevel" },
            values: new object[,]
            {
                { "AnalyticalThinking", 17, new DateTime(2026, 11, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "Communication", 17, new DateTime(2026, 11, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "Cooperation", 17, new DateTime(2026, 11, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "Creativity", 17, new DateTime(2026, 11, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "KnowledgeAndDevelopment", 17, new DateTime(2026, 11, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 4 },
                { "Reliability", 17, new DateTime(2026, 11, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 },
                { "WorkOrganization", 17, new DateTime(2026, 11, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 3 }
            });

        migrationBuilder.InsertData(
            table: "EvaluationNotes",
            columns: new[] { "Id", "ClassificationEditedAt", "CreatedAt", "Criterion", "EvaluationId", "Text", "TextEditedAt", "Tone" },
            values: new object[,]
            {
                { 20, null, new DateTime(2025, 1, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), "WorkOrganization", 17, "Przy raporcie rocznym rozpisała etapy z terminami cząstkowymi i punktami kontroli danych od trzech komórek. Zaplanowała też, co zrobi, jeśli któraś komórka spóźni się z danymi.", null, "Positive" },
                { 21, null, new DateTime(2025, 4, 23, 13, 30, 0, 0, DateTimeKind.Unspecified), "Communication", 17, "Na spotkaniu z Wydziałem Budżetu powtórzyła własnymi słowami stanowisko rozmówców i wyjaśniła, że obie strony inaczej rozumieją pojęcie „wydatki wykonane”.", null, "Positive" },
                { 22, null, new DateTime(2025, 7, 8, 11, 15, 0, 0, DateTimeKind.Unspecified), "Creativity", 17, "Zaproponowała zastąpienie ręcznego scalania arkuszy prostym skryptem. Wypróbowała go na jednym raporcie, porównała wyniki z wersją ręczną i poprawiła skrypt po uwagach zespołu.", null, "Positive" },
                { 23, null, new DateTime(2025, 10, 21, 9, 40, 0, 0, DateTimeKind.Unspecified), "KnowledgeAndDevelopment", 17, "Do zestawienia o zatrudnieniu porównała dane z dwóch systemów kadrowych, wyjaśniła rozbieżności i wybrała źródło zgodne z zasadami ochrony danych.", null, "Positive" },
                { 24, null, new DateTime(2026, 1, 15, 14, 20, 0, 0, DateTimeKind.Unspecified), "Cooperation", 17, "W podsumowaniu raportu rocznego wymieniła osoby z Wydziału Budżetu, które przygotowały dane źródłowe, i opisała, dlaczego ich praca była ważna dla wyniku.", null, "Positive" },
                { 25, null, new DateTime(2026, 3, 18, 15, 0, 0, 0, DateTimeKind.Unspecified), "Reliability", 17, "Kolejne zestawienia przekazywała w terminie, ale o zagrożeniu terminu raportu półrocznego poinformowała dopiero w dniu wysyłki.", null, "Neutral" },
                { 26, null, new DateTime(2026, 7, 1, 10, 30, 0, 0, DateTimeKind.Unspecified), "Creativity", 17, "W raporcie o skargach trzymała się dotychczasowego podziału na kategorie, mimo że nowe rodzaje skarg do nich nie pasowały. Zmieniła podział dopiero po sugestii naczelnika.", null, "Negative" }
            });

        migrationBuilder.InsertData(
            table: "IndicatorRatings",
            columns: new[] { "EvaluationId", "IndicatorId", "Level" },
            values: new object[,]
            {
                { 17, 1, 4 },
                { 17, 2, 2 },
                { 17, 3, 4 },
                { 17, 4, 3 },
                { 17, 5, 4 },
                { 17, 6, 3 },
                { 17, 7, 3 },
                { 17, 8, 4 },
                { 17, 9, 4 },
                { 17, 10, 4 },
                { 17, 11, 3 },
                { 17, 12, null },
                { 17, 13, 3 },
                { 17, 14, 3 },
                { 17, 15, 4 },
                { 17, 16, 2 },
                { 17, 17, 3 },
                { 17, 18, 3 },
                { 17, 19, 3 },
                { 17, 20, 4 },
                { 17, 21, 3 },
                { 17, 22, 3 },
                { 17, 23, 4 },
                { 17, 24, 4 },
                { 17, 25, 3 }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 17 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Communication", 17 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Cooperation", 17 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Creativity", 17 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "KnowledgeAndDevelopment", 17 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Reliability", 17 });

        migrationBuilder.DeleteData(
            table: "CriterionDecisions",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "WorkOrganization", 17 });

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 20);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 21);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 22);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 23);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 24);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 25);

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 26);

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 1 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 2 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 3 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 4 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 5 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 6 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 7 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 8 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 9 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 10 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 11 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 12 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 13 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 14 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 15 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 16 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 17 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 18 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 19 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 20 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 21 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 22 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 23 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 24 });

        migrationBuilder.DeleteData(
            table: "IndicatorRatings",
            keyColumns: new[] { "EvaluationId", "IndicatorId" },
            keyValues: new object[] { 17, 25 });
    }
}
