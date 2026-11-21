using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261121203358_AddJobDescriptionsAndSheetConfirmation : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "SheetConfirmedAt",
            table: "Evaluations",
            type: "datetime2",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "JobDescriptions",
            columns: table => new
            {
                EmployeeId = table.Column<int>(type: "int", nullable: false),
                Purpose = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                Tasks = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                RequiredCompetencies = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                SubordinatesCount = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_JobDescriptions", x => x.EmployeeId);
                table.ForeignKey(
                    name: "FK_JobDescriptions_Employees_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Employees",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 1,
            column: "SheetConfirmedAt",
            value: new DateTime(2024, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 2,
            column: "SheetConfirmedAt",
            value: new DateTime(2025, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 3,
            column: "SheetConfirmedAt",
            value: new DateTime(2024, 11, 5, 9, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 4,
            column: "SheetConfirmedAt",
            value: new DateTime(2025, 1, 14, 9, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 5,
            column: "SheetConfirmedAt",
            value: new DateTime(2025, 6, 4, 9, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 6,
            column: "SheetConfirmedAt",
            value: new DateTime(2026, 1, 13, 9, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 7,
            column: "SheetConfirmedAt",
            value: new DateTime(2024, 12, 23, 9, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.UpdateData(
            table: "Evaluations",
            keyColumn: "Id",
            keyValue: 8,
            column: "SheetConfirmedAt",
            value: new DateTime(2025, 12, 12, 9, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.InsertData(
            table: "JobDescriptions",
            columns: new[] { "EmployeeId", "Purpose", "RequiredCompetencies", "SubordinatesCount", "Tasks" },
            values: new object[,]
            {
                { 1, "Przygotowywanie analiz danych wspierających decyzje kierownictwa departamentu oraz dbanie o jakość danych wykorzystywanych w raportach.", "Myślenie analityczne\nOrientacja na cele\nKomunikacja pisemna\nWspółpraca", null, "Przygotowywanie analiz i raportów na potrzeby kierownictwa departamentu\nOpracowywanie wskaźników i metod pomiaru realizacji zadań\nWeryfikacja jakości i kompletności danych z systemów dziedzinowych\nPrzygotowywanie projektów stanowisk do dokumentów przekazanych do zaopiniowania\nWsparcie merytoryczne mniej doświadczonych pracowników wydziału" },
                { 2, "Realizacja zadań związanych z gromadzeniem, przetwarzaniem i udostępnianiem danych na potrzeby wydziału.", "Myślenie analityczne\nRzetelność\nKomunikacja\nWspółpraca", null, "Gromadzenie i porządkowanie danych z jednostek podległych\nPrzygotowywanie zestawień i wizualizacji danych\nUdzielanie odpowiedzi na zapytania dotyczące danych publicznych\nProwadzenie dokumentacji źródeł danych" },
                { 3, "Kierowanie Wydziałem Analiz Danych, zapewnienie terminowej i rzetelnej realizacji zadań wydziału oraz rozwój kompetencji podległych pracowników.", "Zarządzanie zespołem\nPodejmowanie decyzji\nMyślenie strategiczne\nZarządzanie rozwojem pracowników\nKomunikacja", 6, "Planowanie, organizowanie i nadzorowanie pracy wydziału\nPrzydzielanie zadań i ocena ich realizacji przez pracowników\nPrzygotowywanie stanowisk i rekomendacji dla dyrektora departamentu\nWspółpraca z innymi komórkami organizacyjnymi i instytucjami\nProwadzenie ocen okresowych i planowanie rozwoju zawodowego pracowników" },
                { 9, "Obsługa administracyjna wydziału i wsparcie pracowników w przygotowywaniu zestawień danych.", "Rzetelność\nOrganizacja pracy własnej\nKomunikacja", null, "Prowadzenie rejestru korespondencji wydziału\nPrzygotowywanie prostych zestawień danych według wskazówek\nArchiwizowanie dokumentacji wydziału\nOrganizacja spotkań i przygotowywanie notatek" },
                { 10, "Przygotowywanie i opiniowanie analiz dotyczących usług cyfrowych oraz koordynacja współpracy z dostawcami danych.", "Myślenie analityczne\nWspółpraca\nKomunikacja\nOrientacja na cele", null, "Opracowywanie analiz wpływu planowanych zmian na usługi cyfrowe\nOpiniowanie projektów dokumentów pod kątem danych i wskaźników\nKoordynowanie współpracy z instytucjami przekazującymi dane\nProwadzenie spotkań roboczych z udziałem innych komórek\nPrzygotowywanie materiałów na posiedzenia zespołów międzyresortowych" },
                { 12, "Realizacja zadań związanych z analizą danych i przygotowywaniem informacji dla kierownictwa.", "Myślenie analityczne\nRzetelność\nKomunikacja pisemna", null, "Przygotowywanie cyklicznych raportów z danych wydziału\nAnaliza danych ankietowych i statystycznych\nPrzygotowywanie projektów odpowiedzi na pisma\nAktualizacja danych publikowanych w serwisie urzędu" }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "JobDescriptions");

        migrationBuilder.DropColumn(
            name: "SheetConfirmedAt",
            table: "Evaluations");
    }
}
