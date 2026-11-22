using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261122014324_AddWrittenEvaluationDemo : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "Employees",
            columns: new[] { "Id", "Department", "EmployedSince", "FirstName", "IsSupervisor", "LastName", "Position" },
            values: new object[] { 17, "Wydział Analiz Danych", new DateOnly(2020, 12, 7), "Joanna", false, "Pawlak", "Główny specjalista" });

        migrationBuilder.InsertData(
            table: "EmployeeSupervisors",
            columns: new[] { "EmployeeId", "SupervisorId" },
            values: new object[] { 17, 3 });

        migrationBuilder.InsertData(
            table: "Evaluations",
            columns: new[] { "Id", "CompletedAt", "DueDate", "EmployeeAcquaintedOn", "EmployeeId", "EvaluatorId", "Expectations", "Goals", "PartIConfirmedAt", "PartIIAcquaintedOn", "PeriodEnd", "PeriodStart", "PreparedOn", "Sheet", "SheetConfirmedAt", "StartedAt", "Status", "WrittenEvaluationAt" },
            values: new object[,]
            {
                { 16, new DateTime(2024, 12, 5, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2024, 12, 7), new DateOnly(2022, 12, 20), 17, 3, "Raporty kwartalne przekazywane w terminie, z danymi sprawdzonymi z rejestrami źródłowymi.", null, new DateTime(2022, 12, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2024, 12, 5), new DateOnly(2024, 12, 5), new DateOnly(2022, 12, 20), new DateOnly(2024, 12, 5), "C", new DateTime(2022, 12, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 10, 14, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed", null },
                { 17, null, new DateOnly(2026, 12, 7), new DateOnly(2024, 12, 18), 17, 3, "Dane w raportach sprawdzone z rejestrami przed wysłaniem. Notatki dla kierownictwa z wyraźnie oddzielonymi ustaleniami i kwestiami otwartymi.", null, new DateTime(2024, 12, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), null, null, new DateOnly(2024, 12, 18), null, "C", new DateTime(2024, 12, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), "InProgress", null }
            });

        migrationBuilder.InsertData(
            table: "JobDescriptions",
            columns: new[] { "EmployeeId", "Purpose", "RequiredCompetencies", "SubordinatesCount", "Tasks" },
            values: new object[] { 17, "Przygotowywanie raportów i analiz danych o wnioskach obsługiwanych przez departament oraz dbanie o jakość danych źródłowych.", "Myślenie analityczne\nRzetelność\nKomunikacja pisemna\nKreatywność", null, "Przygotowywanie raportów kwartalnych i rocznych o wnioskach\nAnaliza trendów i przyczyn zmian w liczbie wniosków\nWeryfikacja zgodności danych z rejestrami źródłowymi\nPrzygotowywanie notatek informacyjnych dla kierownictwa\nWsparcie nowych pracowników w pracy z danymi" });

        migrationBuilder.InsertData(
            table: "Conversations",
            columns: new[] { "Id", "At", "EvaluationId", "Status", "Type" },
            values: new object[,]
            {
                { 28, new DateTime(2022, 12, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), 16, "Held", "Starting" },
                { 29, new DateTime(2024, 11, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 16, "Held", "Assessment" },
                { 30, new DateTime(2024, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), 17, "Held", "Starting" },
                { 31, new DateTime(2026, 11, 19, 10, 0, 0, 0, DateTimeKind.Unspecified), 17, "Held", "Assessment" }
            });

        migrationBuilder.InsertData(
            table: "EvaluationAdditionalCriteria",
            columns: new[] { "Criterion", "EvaluationId" },
            values: new object[,]
            {
                { "AnalyticalThinking", 16 },
                { "AnalyticalThinking", 17 },
                { "Creativity", 17 }
            });

        migrationBuilder.InsertData(
            table: "EvaluationNotes",
            columns: new[] { "Id", "ClassificationEditedAt", "CreatedAt", "Criterion", "EvaluationId", "Text", "TextEditedAt", "Tone" },
            values: new object[,]
            {
                { 3, null, new DateTime(2025, 3, 12, 10, 15, 0, 0, DateTimeKind.Unspecified), "Reliability", 17, "Przed wysłaniem raportu rocznego samodzielnie sprawdziła dane z rejestrem źródłowym i wykryła dwie rozbieżności, które poprawiła przed publikacją.", null, "Positive" },
                { 4, null, new DateTime(2025, 9, 3, 15, 40, 0, 0, DateTimeKind.Unspecified), "Reliability", 17, "Przekazała zestawienie dla departamentu dwa dni po uzgodnionym terminie i nie uprzedziła wcześniej o opóźnieniu.", null, "Negative" },
                { 5, null, new DateTime(2025, 6, 17, 11, 0, 0, 0, DateTimeKind.Unspecified), "Communication", 17, "Przygotowała notatkę dla dyrektora, w której jasno oddzieliła ustalenia od kwestii otwartych. Dyrektor przyjął ją bez dopytywania.", null, "Positive" },
                { 6, null, new DateTime(2025, 11, 5, 9, 20, 0, 0, DateTimeKind.Unspecified), "Cooperation", 17, "Pomogła nowej osobie w wydziale przygotować pierwsze zestawienie i wyjaśniła jej zasady weryfikacji danych.", null, "Positive" },
                { 7, null, new DateTime(2026, 2, 24, 14, 5, 0, 0, DateTimeKind.Unspecified), "AnalyticalThinking", 17, "Zauważyła, że spadek liczby wniosków wynika ze zmiany formularza, a nie z mniejszego zainteresowania, i pokazała to na danych z trzech kwartałów.", null, "Positive" },
                { 8, null, new DateTime(2026, 5, 14, 16, 30, 0, 0, DateTimeKind.Unspecified), "WorkOrganization", 17, "Zrealizowała plan kwartalny, ale dwa zadania wymagały przesunięcia terminu po uzgodnieniu z naczelnikiem.", null, "Neutral" },
                { 9, null, new DateTime(2026, 9, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), "KnowledgeAndDevelopment", 17, "Ukończyła szkolenie z analizy danych i zastosowała nową metodę w raporcie kwartalnym. Pokazała ją też zespołowi na spotkaniu wydziału.", null, "Positive" }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 28);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 29);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 30);

        migrationBuilder.DeleteData(
            table: "Conversations",
            keyColumn: "Id",
            keyValue: 31);

        migrationBuilder.DeleteData(
            table: "EmployeeSupervisors",
            keyColumns: new[] { "EmployeeId", "SupervisorId" },
            keyValues: new object[] { 17, 3 });

        migrationBuilder.DeleteData(
            table: "EvaluationAdditionalCriteria",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 16 });

        migrationBuilder.DeleteData(
            table: "EvaluationAdditionalCriteria",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "AnalyticalThinking", 17 });

        migrationBuilder.DeleteData(
            table: "EvaluationAdditionalCriteria",
            keyColumns: new[] { "Criterion", "EvaluationId" },
            keyValues: new object[] { "Creativity", 17 });

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

        migrationBuilder.DeleteData(
            table: "EvaluationNotes",
            keyColumn: "Id",
            keyValue: 9);

        migrationBuilder.DeleteData(
            table: "JobDescriptions",
            keyColumn: "EmployeeId",
            keyValue: 17);

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 16);

        migrationBuilder.DeleteData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 17);

        migrationBuilder.DeleteData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 17);
    }
}
