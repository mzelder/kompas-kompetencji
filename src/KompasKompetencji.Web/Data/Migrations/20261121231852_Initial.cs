using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261121231852_Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Employees",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Position = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Department = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                IsSupervisor = table.Column<bool>(type: "bit", nullable: false),
                EmployedSince = table.Column<DateOnly>(type: "date", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Employees", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "EmployeeSupervisors",
            columns: table => new
            {
                EmployeeId = table.Column<int>(type: "int", nullable: false),
                SupervisorId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EmployeeSupervisors", x => new { x.EmployeeId, x.SupervisorId });
                table.CheckConstraint("CK_EmployeeSupervisors_NotSelf", "[EmployeeId] <> [SupervisorId]");
                table.ForeignKey(
                    name: "FK_EmployeeSupervisors_Employees_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Employees",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_EmployeeSupervisors_Employees_SupervisorId",
                    column: x => x.SupervisorId,
                    principalTable: "Employees",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Evaluations",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                EmployeeId = table.Column<int>(type: "int", nullable: false),
                EvaluatorId = table.Column<int>(type: "int", nullable: false),
                Sheet = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                SheetConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                WrittenEvaluationAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                PeriodStart = table.Column<DateOnly>(type: "date", nullable: false),
                PeriodEnd = table.Column<DateOnly>(type: "date", nullable: false),
                DueDate = table.Column<DateOnly>(type: "date", nullable: false),
                Expectations = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                Goals = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                PartIConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                EmployeeAcquaintedOn = table.Column<DateOnly>(type: "date", nullable: true),
                StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Evaluations", x => x.Id);
                table.ForeignKey(
                    name: "FK_Evaluations_Employees_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Employees",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Evaluations_Employees_EvaluatorId",
                    column: x => x.EvaluatorId,
                    principalTable: "Employees",
                    principalColumn: "Id");
            });

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

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                PasswordHash = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                EmployeeId = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
                table.ForeignKey(
                    name: "FK_Users_Employees_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Employees",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "Conversations",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                EvaluationId = table.Column<int>(type: "int", nullable: false),
                Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                At = table.Column<DateTime>(type: "datetime2", nullable: false),
                Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Conversations", x => x.Id);
                table.ForeignKey(
                    name: "FK_Conversations_Evaluations_EvaluationId",
                    column: x => x.EvaluationId,
                    principalTable: "Evaluations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "EvaluationAdditionalCriteria",
            columns: table => new
            {
                EvaluationId = table.Column<int>(type: "int", nullable: false),
                Criterion = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EvaluationAdditionalCriteria", x => new { x.EvaluationId, x.Criterion });
                table.ForeignKey(
                    name: "FK_EvaluationAdditionalCriteria_Evaluations_EvaluationId",
                    column: x => x.EvaluationId,
                    principalTable: "Evaluations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

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
            table: "Employees",
            columns: new[] { "Id", "Department", "EmployedSince", "FirstName", "IsSupervisor", "LastName", "Position" },
            values: new object[,]
            {
                { 1, "Wydział Analiz Danych", new DateOnly(2019, 12, 15), "Anna", false, "Kowalska", "Starszy specjalista" },
                { 2, "Wydział Analiz Danych", new DateOnly(2022, 11, 2), "Jan", false, "Nowak", "Specjalista" },
                { 3, "Wydział Analiz Danych", new DateOnly(2015, 4, 1), "Ewa", true, "Wiśniewska", "Naczelnik wydziału" },
                { 4, "Departament Danych i Usług Cyfrowych", new DateOnly(2012, 9, 1), "Tomasz", true, "Zieliński", "Dyrektor departamentu" },
                { 5, "Wydział Usług Cyfrowych", new DateOnly(2016, 2, 15), "Marek", true, "Lewandowski", "Naczelnik wydziału" },
                { 6, "Wydział Usług Cyfrowych", new DateOnly(2018, 5, 1), "Magdalena", true, "Dąbrowska", "Zastępca naczelnika wydziału" },
                { 7, "Wydział Usług Cyfrowych", new DateOnly(2021, 9, 1), "Katarzyna", false, "Wójcik", "Główny specjalista" },
                { 8, "Wydział Usług Cyfrowych", new DateOnly(2024, 3, 1), "Piotr", false, "Kamiński", "Starszy referent" },
                { 9, "Wydział Analiz Danych", new DateOnly(2018, 1, 10), "Michał", false, "Szymański", "Referent" },
                { 10, "Wydział Analiz Danych", new DateOnly(2020, 6, 1), "Agnieszka", false, "Woźniak", "Starszy specjalista" },
                { 11, "Wydział Analiz Danych", new DateOnly(2026, 11, 5), "Paweł", false, "Kozłowski", "Starszy specjalista" },
                { 12, "Wydział Analiz Danych", new DateOnly(2021, 12, 20), "Karolina", false, "Mazur", "Specjalista" },
                { 13, "Wydział Analiz Danych", new DateOnly(2020, 12, 10), "Łukasz", false, "Grabowski", "Specjalista" },
                { 14, "Wydział Analiz Danych", new DateOnly(2023, 4, 3), "Natalia", false, "Krawczyk", "Główny specjalista" },
                { 15, "Wydział Analiz Danych", new DateOnly(2022, 1, 17), "Bartosz", false, "Wróbel", "Starszy specjalista" },
                { 16, "Wydział Analiz Danych", new DateOnly(2019, 11, 28), "Monika", false, "Sikora", "Specjalista" }
            });

        migrationBuilder.InsertData(
            table: "EmployeeSupervisors",
            columns: new[] { "EmployeeId", "SupervisorId" },
            values: new object[,]
            {
                { 1, 3 },
                { 2, 3 },
                { 3, 4 },
                { 5, 4 },
                { 6, 5 },
                { 7, 5 },
                { 8, 5 },
                { 8, 6 },
                { 9, 3 },
                { 10, 3 },
                { 11, 3 },
                { 12, 3 },
                { 13, 3 },
                { 14, 3 },
                { 15, 3 },
                { 16, 3 }
            });

        migrationBuilder.InsertData(
            table: "Evaluations",
            columns: new[] { "Id", "CompletedAt", "DueDate", "EmployeeAcquaintedOn", "EmployeeId", "EvaluatorId", "Expectations", "Goals", "PartIConfirmedAt", "PeriodEnd", "PeriodStart", "Sheet", "SheetConfirmedAt", "StartedAt", "Status", "WrittenEvaluationAt" },
            values: new object[,]
            {
                { 1, new DateTime(2025, 12, 10, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 12, 15), new DateOnly(2024, 12, 16), 1, 3, "Samodzielne przygotowywanie analiz dla kierownictwa w uzgodnionych terminach, z opisem źródeł i ograniczeń danych.", null, new DateTime(2024, 12, 16, 15, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 12, 14), new DateOnly(2024, 12, 16), "C", new DateTime(2024, 12, 11, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed", null },
                { 2, null, new DateOnly(2026, 12, 15), new DateOnly(2025, 12, 16), 1, 3, "Analizy dla kierownictwa oddawane w terminie i bez poprawek merytorycznych. Propozycje nowych wskaźników zgłaszane z własnej inicjatywy, co najmniej raz na kwartał.", "Opracowanie zestawu wskaźników realizacji zadań departamentu do końca II kwartału 2026 r.", new DateTime(2025, 12, 16, 15, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 12, 14), new DateOnly(2025, 12, 16), "C", new DateTime(2025, 12, 11, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 12, 10, 30, 0, 0, DateTimeKind.Unspecified), "InProgress", null },
                { 3, new DateTime(2025, 10, 28, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 11, 2), new DateOnly(2024, 11, 7), 2, 3, "Zestawienia danych przygotowywane według przyjętego wzoru i sprawdzone przed przekazaniem.", null, new DateTime(2024, 11, 7, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 11, 1), new DateOnly(2024, 11, 7), "C", new DateTime(2024, 11, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed", null },
                { 4, new DateTime(2026, 1, 8, 11, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 1, 10), new DateOnly(2025, 1, 16), 9, 3, "Korespondencja wydziału rejestrowana na bieżąco, odpowiedzi na proste pytania udzielane w ciągu dwóch dni.", null, new DateTime(2025, 1, 16, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 1, 9), new DateOnly(2025, 1, 16), "C", new DateTime(2025, 1, 14, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed", null },
                { 5, new DateTime(2026, 11, 6, 13, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 11, 10), new DateOnly(2025, 11, 17), 10, 3, "Opinie do projektów dokumentów przygotowywane w terminie, z konkretnymi propozycjami zmian wskaźników.", null, new DateTime(2025, 11, 17, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 11, 9), new DateOnly(2025, 11, 17), "C", new DateTime(2025, 11, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed", null },
                { 6, null, new DateOnly(2027, 1, 10), new DateOnly(2026, 1, 15), 9, 3, "Rejestr korespondencji prowadzony bez zaległości. Zestawienia danych przygotowywane samodzielnie według wskazówek, bez konieczności poprawek.", null, new DateTime(2026, 1, 15, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2027, 1, 9), new DateOnly(2026, 1, 15), "C", new DateTime(2026, 1, 13, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 16, 14, 0, 0, 0, DateTimeKind.Unspecified), "CriteriaSet", null },
                { 7, new DateTime(2025, 12, 18, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 12, 20), new DateOnly(2025, 1, 8), 12, 3, "Cykliczne raporty przekazywane w terminie, z krótkim omówieniem najważniejszych zmian w danych.", null, new DateTime(2025, 1, 8, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 12, 19), new DateOnly(2025, 1, 8), "C", new DateTime(2024, 12, 23, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 24, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed", null },
                { 8, null, new DateOnly(2026, 12, 10), new DateOnly(2025, 12, 17), 13, 3, "Analizy danych przygotowywane samodzielnie, z wnioskami zrozumiałymi dla odbiorców spoza wydziału.", null, new DateTime(2025, 12, 17, 15, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 12, 9), new DateOnly(2025, 12, 17), "C", new DateTime(2025, 12, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), null, "NotStarted", null },
                { 9, null, new DateOnly(2027, 11, 10), null, 10, 3, "Analizy wpływu zmian na usługi cyfrowe z rekomendacją i oceną ryzyka. Samodzielne przedstawianie stanowiska wydziału na posiedzeniach zespołów międzyresortowych.", null, null, new DateOnly(2027, 11, 9), new DateOnly(2026, 11, 6), "C", new DateTime(2026, 11, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), null, "NotStarted", null },
                { 10, new DateTime(2026, 3, 30, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 4, 3), new DateOnly(2025, 4, 22), 14, 3, "Zestawy wskaźników przygotowywane z opisem metody pomiaru i źródeł danych, uzgodnione z komórkami merytorycznymi.", null, new DateTime(2025, 4, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 4, 2), new DateOnly(2025, 4, 22), "C", new DateTime(2025, 4, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed", null },
                { 11, null, new DateOnly(2027, 4, 3), new DateOnly(2026, 4, 21), 14, 3, "Analizy porównawcze dla kierownictwa przekazywane w uzgodnionych terminach. Koordynowanie prac analitycznych kilku osób wydziału bez opóźnień.", "Opracowanie standardu opisu i weryfikacji źródeł danych dla wydziału do końca 2026 r.", new DateTime(2026, 4, 21, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2027, 4, 2), new DateOnly(2026, 4, 21), "C", new DateTime(2026, 4, 7, 9, 0, 0, 0, DateTimeKind.Unspecified), null, "CriteriaSet", null },
                { 12, new DateTime(2026, 1, 14, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 1, 17), new DateOnly(2025, 2, 4), 15, 3, "Prognozy i analizy trendów przygotowywane w terminie, z opisem założeń i ograniczeń.", null, new DateTime(2025, 2, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 1, 16), new DateOnly(2025, 2, 4), "C", new DateTime(2025, 1, 21, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed", null },
                { 13, null, new DateOnly(2027, 1, 17), new DateOnly(2026, 2, 3), 15, 3, "Obliczenia w raportach wydziału sprawdzane przed wysłaniem. Wyjaśnienia metod analizy dla innych komórek udzielane w ciągu tygodnia.", null, new DateTime(2026, 2, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2027, 1, 16), new DateOnly(2026, 2, 3), "C", new DateTime(2026, 1, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), null, "CriteriaSet", null },
                { 14, new DateTime(2025, 11, 28, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 11, 28), new DateOnly(2024, 12, 17), 16, 3, "Udostępnianie danych publicznych na wnioski w ustawowym terminie, z kompletnym uzasadnieniem odmowy.", null, new DateTime(2024, 12, 17, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 11, 27), new DateOnly(2024, 12, 17), "C", new DateTime(2024, 12, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed", new DateTime(2025, 11, 20, 13, 0, 0, 0, DateTimeKind.Unspecified) },
                { 15, null, new DateOnly(2026, 11, 28), new DateOnly(2025, 12, 16), 16, 3, "Wnioski o udostępnienie danych rozpatrywane w terminie, także te wymagające uzgodnień z innymi komórkami. Rejestr udostępnionych danych prowadzony na bieżąco.", null, new DateTime(2025, 12, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2026, 11, 27), new DateOnly(2025, 12, 16), "C", new DateTime(2025, 12, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "InProgress", new DateTime(2026, 11, 19, 14, 30, 0, 0, DateTimeKind.Unspecified) }
            });

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
                { 11, "Prowadzenie spraw dotyczących reagowania na incydenty i sytuacje kryzysowe w zakresie danych i usług cyfrowych departamentu.", "Organizacja pracy i orientacja na osiąganie celów\nWspółpraca\nMyślenie analityczne\nWykorzystywanie wiedzy i doskonalenie zawodowe\nRadzenie sobie w sytuacjach kryzysowych", null, "Udział w opracowaniu i aktualizacji planu reagowania na incydenty dotyczące danych i usług cyfrowych\nAnaliza zgłoszonych incydentów i przygotowywanie rekomendacji działań naprawczych\nWspółpraca z zespołami reagowania na incydenty w innych urzędach i instytucjach\nUdział w przygotowaniu i przeprowadzaniu ćwiczeń z reagowania na sytuacje kryzysowe\nProwadzenie kontroli przestrzegania procedur bezpieczeństwa danych w jednostkach podległych" },
                { 12, "Realizacja zadań związanych z analizą danych i przygotowywaniem informacji dla kierownictwa.", "Myślenie analityczne\nRzetelność\nKomunikacja pisemna", null, "Przygotowywanie cyklicznych raportów z danych wydziału\nAnaliza danych ankietowych i statystycznych\nPrzygotowywanie projektów odpowiedzi na pisma\nAktualizacja danych publikowanych w serwisie urzędu" },
                { 14, "Projektowanie i utrzymanie zestawów wskaźników wydziału oraz zapewnienie spójności metod analizy danych.", "Myślenie analityczne\nOrientacja na cele\nWspółpraca\nKomunikacja pisemna", null, "Projektowanie wskaźników i metod pomiaru dla zadań departamentu\nPrzygotowywanie analiz porównawczych na potrzeby kierownictwa\nOpracowywanie standardów opisu i weryfikacji źródeł danych\nKoordynowanie prac analitycznych realizowanych przez kilka osób wydziału" },
                { 15, "Przygotowywanie analiz statystycznych i prognoz na potrzeby planowania zadań departamentu.", "Myślenie analityczne\nRzetelność\nKomunikacja\nOrientacja na cele", null, "Przygotowywanie prognoz i analiz trendów na podstawie danych wydziału\nOpracowywanie zestawień do planów i sprawozdań departamentu\nWeryfikacja poprawności obliczeń w raportach przygotowywanych przez wydział\nUdzielanie wyjaśnień dotyczących metod analizy innym komórkom" },
                { 16, "Obsługa wniosków o udostępnienie danych i przygotowywanie zestawień dla podmiotów zewnętrznych.", "Rzetelność\nKomunikacja pisemna\nWspółpraca", null, "Rozpatrywanie wniosków o udostępnienie danych wydziału\nPrzygotowywanie zestawień danych dla podmiotów zewnętrznych\nProwadzenie rejestru udostępnionych danych\nWspółpraca z działem prawnym przy odpowiedziach na wnioski" }
            });

        migrationBuilder.InsertData(
            table: "Conversations",
            columns: new[] { "Id", "At", "EvaluationId", "Status", "Type" },
            values: new object[,]
            {
                { 1, new DateTime(2024, 12, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, "Held", "Starting" },
                { 2, new DateTime(2025, 12, 8, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, "Held", "Assessment" },
                { 3, new DateTime(2025, 12, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), 2, "Held", "Starting" },
                { 4, new DateTime(2026, 11, 27, 10, 0, 0, 0, DateTimeKind.Unspecified), 2, "Scheduled", "Assessment" },
                { 5, new DateTime(2024, 11, 7, 10, 0, 0, 0, DateTimeKind.Unspecified), 3, "Held", "Starting" },
                { 6, new DateTime(2025, 10, 27, 11, 0, 0, 0, DateTimeKind.Unspecified), 3, "Held", "Assessment" },
                { 7, new DateTime(2025, 1, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), 4, "Held", "Starting" },
                { 8, new DateTime(2026, 1, 7, 9, 30, 0, 0, DateTimeKind.Unspecified), 4, "Held", "Assessment" },
                { 9, new DateTime(2025, 11, 17, 10, 0, 0, 0, DateTimeKind.Unspecified), 5, "Held", "Starting" },
                { 10, new DateTime(2026, 11, 5, 13, 0, 0, 0, DateTimeKind.Unspecified), 5, "Held", "Assessment" },
                { 11, new DateTime(2026, 1, 15, 10, 0, 0, 0, DateTimeKind.Unspecified), 6, "Held", "Starting" },
                { 12, new DateTime(2026, 12, 18, 9, 0, 0, 0, DateTimeKind.Unspecified), 6, "Scheduled", "Assessment" },
                { 13, new DateTime(2025, 1, 8, 10, 0, 0, 0, DateTimeKind.Unspecified), 7, "Held", "Starting" },
                { 14, new DateTime(2025, 12, 17, 12, 0, 0, 0, DateTimeKind.Unspecified), 7, "Held", "Assessment" },
                { 15, new DateTime(2025, 12, 17, 11, 0, 0, 0, DateTimeKind.Unspecified), 8, "Held", "Starting" },
                { 16, new DateTime(2026, 11, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), 8, "Scheduled", "Assessment" },
                { 17, new DateTime(2026, 11, 26, 10, 0, 0, 0, DateTimeKind.Unspecified), 9, "Scheduled", "Starting" },
                { 18, new DateTime(2025, 4, 22, 9, 0, 0, 0, DateTimeKind.Unspecified), 10, "Held", "Starting" },
                { 19, new DateTime(2026, 3, 25, 10, 0, 0, 0, DateTimeKind.Unspecified), 10, "Held", "Assessment" },
                { 20, new DateTime(2026, 4, 21, 9, 0, 0, 0, DateTimeKind.Unspecified), 11, "Held", "Starting" },
                { 21, new DateTime(2025, 2, 4, 9, 0, 0, 0, DateTimeKind.Unspecified), 12, "Held", "Starting" },
                { 22, new DateTime(2026, 1, 12, 11, 0, 0, 0, DateTimeKind.Unspecified), 12, "Held", "Assessment" },
                { 23, new DateTime(2026, 2, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), 13, "Held", "Starting" },
                { 24, new DateTime(2024, 12, 17, 9, 0, 0, 0, DateTimeKind.Unspecified), 14, "Held", "Starting" },
                { 25, new DateTime(2025, 11, 17, 10, 0, 0, 0, DateTimeKind.Unspecified), 14, "Held", "Assessment" },
                { 26, new DateTime(2025, 12, 16, 9, 0, 0, 0, DateTimeKind.Unspecified), 15, "Held", "Starting" },
                { 27, new DateTime(2026, 11, 16, 11, 0, 0, 0, DateTimeKind.Unspecified), 15, "Held", "Assessment" }
            });

        migrationBuilder.InsertData(
            table: "EvaluationAdditionalCriteria",
            columns: new[] { "Criterion", "EvaluationId" },
            values: new object[,]
            {
                { "AnalyticalThinking", 1 },
                { "AnalyticalThinking", 2 },
                { "Creativity", 2 },
                { "AnalyticalThinking", 3 },
                { "CustomerOrientation", 4 },
                { "AnalyticalThinking", 5 },
                { "AnalyticalThinking", 6 },
                { "CustomerOrientation", 6 },
                { "AnalyticalThinking", 7 },
                { "AnalyticalThinking", 8 },
                { "AnalyticalThinking", 9 },
                { "PublicSpeaking", 9 },
                { "AnalyticalThinking", 10 },
                { "AnalyticalThinking", 11 },
                { "AnalyticalThinking", 12 },
                { "AnalyticalThinking", 13 },
                { "AnalyticalThinking", 14 },
                { "AnalyticalThinking", 15 }
            });

        migrationBuilder.InsertData(
            table: "EvaluationNotes",
            columns: new[] { "Id", "CreatedAt", "EvaluationId", "Text" },
            values: new object[,]
            {
                { 1, new DateTime(2026, 5, 14, 15, 20, 0, 0, DateTimeKind.Unspecified), 11, "Przygotowała zestaw wskaźników do sprawozdania kwartalnego tydzień przed terminem. Kierownictwo departamentu przyjęło go bez poprawek." },
                { 2, new DateTime(2026, 9, 8, 11, 0, 0, 0, DateTimeKind.Unspecified), 11, "Z własnej inicjatywy poprowadziła dla wydziału warsztat z weryfikacji źródeł danych. Uczestnicy ocenili go jako przydatny." },
                { 3, new DateTime(2026, 3, 10, 9, 45, 0, 0, DateTimeKind.Unspecified), 13, "Prognoza zatrudnienia na 2027 r. wymagała dwóch poprawek metodologicznych. Po wskazówkach przygotował poprawioną wersję w terminie." },
                { 4, new DateTime(2026, 10, 2, 14, 10, 0, 0, DateTimeKind.Unspecified), 13, "Samodzielnie wykrył błąd w obliczeniach raportu rocznego przed jego wysłaniem do dyrektora i zaproponował dodatkowy krok kontroli." },
                { 5, new DateTime(2026, 3, 4, 10, 15, 0, 0, DateTimeKind.Unspecified), 15, "Rozpatrzyła wszystkie wnioski z pierwszego kwartału w terminie, w tym dwa wymagające uzgodnień z działem prawnym." },
                { 6, new DateTime(2026, 8, 21, 13, 40, 0, 0, DateTimeKind.Unspecified), 15, "Uporządkowała rejestr udostępnionych danych i zaproponowała wzór odpowiedzi skracający obsługę typowych wniosków." }
            });

        migrationBuilder.CreateIndex(
            name: "IX_Conversations_EvaluationId_Type",
            table: "Conversations",
            columns: new[] { "EvaluationId", "Type" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_EmployeeSupervisors_SupervisorId",
            table: "EmployeeSupervisors",
            column: "SupervisorId");

        migrationBuilder.CreateIndex(
            name: "IX_EvaluationNotes_EvaluationId_CreatedAt",
            table: "EvaluationNotes",
            columns: new[] { "EvaluationId", "CreatedAt" });

        migrationBuilder.CreateIndex(
            name: "IX_Evaluations_EmployeeId_DueDate",
            table: "Evaluations",
            columns: new[] { "EmployeeId", "DueDate" });

        migrationBuilder.CreateIndex(
            name: "IX_Evaluations_EvaluatorId",
            table: "Evaluations",
            column: "EvaluatorId");

        migrationBuilder.CreateIndex(
            name: "IX_Users_EmployeeId",
            table: "Users",
            column: "EmployeeId",
            unique: true,
            filter: "[EmployeeId] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_Users_Login",
            table: "Users",
            column: "Login",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Conversations");

        migrationBuilder.DropTable(
            name: "EmployeeSupervisors");

        migrationBuilder.DropTable(
            name: "EvaluationAdditionalCriteria");

        migrationBuilder.DropTable(
            name: "EvaluationNotes");

        migrationBuilder.DropTable(
            name: "JobDescriptions");

        migrationBuilder.DropTable(
            name: "Users");

        migrationBuilder.DropTable(
            name: "Evaluations");

        migrationBuilder.DropTable(
            name: "Employees");
    }
}
