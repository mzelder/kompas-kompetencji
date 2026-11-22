using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261122071537_JobPositions : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "JobDescriptions");

        migrationBuilder.AddColumn<int>(
            name: "JobPositionId",
            table: "Employees",
            type: "int",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "JobPositions",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Symbol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Department = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Unit = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                DirectSupervisorTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                IndirectSupervisorTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                Purpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Tasks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                RequiredCompetencies = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                Subordinates = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_JobPositions", x => x.Id);
            });

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 1,
            column: "JobPositionId",
            value: 1);

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 2,
            column: "JobPositionId",
            value: 2);

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 3,
            column: "JobPositionId",
            value: 3);

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 4,
            column: "JobPositionId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 5,
            column: "JobPositionId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 6,
            column: "JobPositionId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 7,
            column: "JobPositionId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 8,
            column: "JobPositionId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 9,
            column: "JobPositionId",
            value: 4);

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 10,
            column: "JobPositionId",
            value: 5);

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 11,
            column: "JobPositionId",
            value: 6);

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 12,
            column: "JobPositionId",
            value: 7);

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 13,
            column: "JobPositionId",
            value: null);

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 14,
            column: "JobPositionId",
            value: 8);

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 15,
            column: "JobPositionId",
            value: 9);

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 16,
            column: "JobPositionId",
            value: 10);

        migrationBuilder.UpdateData(
            table: "Employees",
            keyColumn: "Id",
            keyValue: 17,
            column: "JobPositionId",
            value: 11);

        migrationBuilder.InsertData(
            table: "JobPositions",
            columns: new[] { "Id", "Department", "DirectSupervisorTitle", "IndirectSupervisorTitle", "Purpose", "RequiredCompetencies", "Subordinates", "Symbol", "Tasks", "Title", "Unit" },
            values: new object[,]
            {
                { 1, "Departament Danych i Usług Cyfrowych", "Naczelnik wydziału", "Dyrektor departamentu", "Przygotowywanie analiz danych wspierających decyzje kierownictwa departamentu oraz dbanie o jakość danych wykorzystywanych w raportach.", "Myślenie analityczne\nOrientacja na cele\nKomunikacja pisemna\nWspółpraca", "None", "DDU WAD 4", "Przygotowywanie analiz i raportów na potrzeby kierownictwa departamentu\nOpracowywanie wskaźników i metod pomiaru realizacji zadań\nWeryfikacja jakości i kompletności danych z systemów dziedzinowych\nPrzygotowywanie projektów stanowisk do dokumentów przekazanych do zaopiniowania\nWsparcie merytoryczne mniej doświadczonych pracowników wydziału", "Starszy specjalista", "Wydział Analiz Danych" },
                { 2, "Departament Danych i Usług Cyfrowych", "Naczelnik wydziału", "Dyrektor departamentu", "Realizacja zadań związanych z gromadzeniem, przetwarzaniem i udostępnianiem danych na potrzeby wydziału.", "Myślenie analityczne\nRzetelność\nKomunikacja\nWspółpraca", "None", "DDU WAD 7", "Gromadzenie i porządkowanie danych z jednostek podległych\nPrzygotowywanie zestawień i wizualizacji danych\nUdzielanie odpowiedzi na zapytania dotyczące danych publicznych\nProwadzenie dokumentacji źródeł danych", "Specjalista", "Wydział Analiz Danych" },
                { 3, "Departament Danych i Usług Cyfrowych", "Dyrektor departamentu", null, "Kierowanie Wydziałem Analiz Danych, zapewnienie terminowej i rzetelnej realizacji zadań wydziału oraz rozwój kompetencji podległych pracowników.", "Zarządzanie zespołem\nPodejmowanie decyzji\nMyślenie strategiczne\nZarządzanie rozwojem pracowników\nKomunikacja", "Over9", "DDU WAD 1", "Planowanie, organizowanie i nadzorowanie pracy wydziału\nPrzydzielanie zadań i ocena ich realizacji przez pracowników\nPrzygotowywanie stanowisk i rekomendacji dla dyrektora departamentu\nWspółpraca z innymi komórkami organizacyjnymi i instytucjami\nProwadzenie ocen okresowych i planowanie rozwoju zawodowego pracowników", "Naczelnik wydziału", "Wydział Analiz Danych" },
                { 4, "Departament Danych i Usług Cyfrowych", "Naczelnik wydziału", "Dyrektor departamentu", "Obsługa administracyjna wydziału i wsparcie pracowników w przygotowywaniu zestawień danych.", "Rzetelność\nOrganizacja pracy własnej\nKomunikacja", "None", "DDU WAD 12", "Prowadzenie rejestru korespondencji wydziału\nPrzygotowywanie prostych zestawień danych według wskazówek\nArchiwizowanie dokumentacji wydziału\nOrganizacja spotkań i przygotowywanie notatek", "Referent", "Wydział Analiz Danych" },
                { 5, "Departament Danych i Usług Cyfrowych", "Naczelnik wydziału", "Dyrektor departamentu", "Przygotowywanie i opiniowanie analiz dotyczących usług cyfrowych oraz koordynacja współpracy z dostawcami danych.", "Myślenie analityczne\nWspółpraca\nKomunikacja\nOrientacja na cele", "None", "DDU WAD 5", "Opracowywanie analiz wpływu planowanych zmian na usługi cyfrowe\nOpiniowanie projektów dokumentów pod kątem danych i wskaźników\nKoordynowanie współpracy z instytucjami przekazującymi dane\nProwadzenie spotkań roboczych z udziałem innych komórek\nPrzygotowywanie materiałów na posiedzenia zespołów międzyresortowych", "Starszy specjalista", "Wydział Analiz Danych" },
                { 6, "Departament Danych i Usług Cyfrowych", "Naczelnik wydziału", "Dyrektor departamentu", "Prowadzenie spraw dotyczących reagowania na incydenty i sytuacje kryzysowe w zakresie danych i usług cyfrowych departamentu.", "Organizacja pracy i orientacja na osiąganie celów\nWspółpraca\nMyślenie analityczne\nWykorzystywanie wiedzy i doskonalenie zawodowe\nRadzenie sobie w sytuacjach kryzysowych", "None", "DDU WAD 6", "Udział w opracowaniu i aktualizacji planu reagowania na incydenty dotyczące danych i usług cyfrowych\nAnaliza zgłoszonych incydentów i przygotowywanie rekomendacji działań naprawczych\nWspółpraca z zespołami reagowania na incydenty w innych urzędach i instytucjach\nUdział w przygotowaniu i przeprowadzaniu ćwiczeń z reagowania na sytuacje kryzysowe\nProwadzenie kontroli przestrzegania procedur bezpieczeństwa danych w jednostkach podległych", "Starszy specjalista", "Wydział Analiz Danych" },
                { 7, "Departament Danych i Usług Cyfrowych", "Naczelnik wydziału", "Dyrektor departamentu", "Realizacja zadań związanych z analizą danych i przygotowywaniem informacji dla kierownictwa.", "Myślenie analityczne\nRzetelność\nKomunikacja pisemna", "None", "DDU WAD 8", "Przygotowywanie cyklicznych raportów z danych wydziału\nAnaliza danych ankietowych i statystycznych\nPrzygotowywanie projektów odpowiedzi na pisma\nAktualizacja danych publikowanych w serwisie urzędu", "Specjalista", "Wydział Analiz Danych" },
                { 8, "Departament Danych i Usług Cyfrowych", "Naczelnik wydziału", "Dyrektor departamentu", "Projektowanie i utrzymanie zestawów wskaźników wydziału oraz zapewnienie spójności metod analizy danych.", "Myślenie analityczne\nOrientacja na cele\nWspółpraca\nKomunikacja pisemna", "None", "DDU WAD 2", "Projektowanie wskaźników i metod pomiaru dla zadań departamentu\nPrzygotowywanie analiz porównawczych na potrzeby kierownictwa\nOpracowywanie standardów opisu i weryfikacji źródeł danych\nKoordynowanie prac analitycznych realizowanych przez kilka osób wydziału", "Główny specjalista", "Wydział Analiz Danych" },
                { 9, "Departament Danych i Usług Cyfrowych", "Naczelnik wydziału", "Dyrektor departamentu", "Przygotowywanie analiz statystycznych i prognoz na potrzeby planowania zadań departamentu.", "Myślenie analityczne\nRzetelność\nKomunikacja\nOrientacja na cele", "None", "DDU WAD 9", "Przygotowywanie prognoz i analiz trendów na podstawie danych wydziału\nOpracowywanie zestawień do planów i sprawozdań departamentu\nWeryfikacja poprawności obliczeń w raportach przygotowywanych przez wydział\nUdzielanie wyjaśnień dotyczących metod analizy innym komórkom", "Starszy specjalista", "Wydział Analiz Danych" },
                { 10, "Departament Danych i Usług Cyfrowych", "Naczelnik wydziału", "Dyrektor departamentu", "Obsługa wniosków o udostępnienie danych i przygotowywanie zestawień dla podmiotów zewnętrznych.", "Rzetelność\nKomunikacja pisemna\nWspółpraca", "None", "DDU WAD 10", "Rozpatrywanie wniosków o udostępnienie danych wydziału\nPrzygotowywanie zestawień danych dla podmiotów zewnętrznych\nProwadzenie rejestru udostępnionych danych\nWspółpraca z działem prawnym przy odpowiedziach na wnioski", "Specjalista", "Wydział Analiz Danych" },
                { 11, "Departament Danych i Usług Cyfrowych", "Naczelnik wydziału", "Dyrektor departamentu", "Przygotowywanie raportów i analiz danych o wnioskach obsługiwanych przez departament oraz dbanie o jakość danych źródłowych.", "Myślenie analityczne\nRzetelność\nKomunikacja pisemna\nKreatywność", "None", "DDU WAD 3", "Przygotowywanie raportów kwartalnych i rocznych o wnioskach\nAnaliza trendów i przyczyn zmian w liczbie wniosków\nWeryfikacja zgodności danych z rejestrami źródłowymi\nPrzygotowywanie notatek informacyjnych dla kierownictwa\nWsparcie nowych pracowników w pracy z danymi", "Główny specjalista", "Wydział Analiz Danych" },
                { 12, "Departament Cyberbezpieczeństwa", "Naczelnik wydziału", "Dyrektor", "Prowadzenie spraw dotyczących zarządzania kryzysowego w cyberprzestrzeni pozostających w zakresie ministra właściwego ds. informatyzacji.", "Organizacja pracy i orientacja na osiąganie celów\nWspółpraca\nMyślenie analityczne\nWykorzystywanie wiedzy i doskonalenie zawodowe", "None", "DC WZKSOiC 6", "Bierze udział w opracowaniu i aktualizacji dokumentacji w zakresie zarządzania kryzysowego (np. Krajowego planu reagowania na incydenty i sytuacje kryzysowe w cyberbezpieczeństwie na dużą skalę) w celu zapewnienia obsługi zadań Ministra w zakresie zarządzania kryzysowego w cyberprzestrzeni.\nBierze udział w realizacji zadań z obszaru zarządzania kryzysowego wynikających z członkostwa Polski w Organizacji Paktu Północnoatlantyckiego (NATO), Unii Europejskiej (UE) i Organizacji Narodów Zjednoczonych (ONZ) w celu zapewnienia stałej gotowości do reagowania na zagrożenia płynące z cyberprzestrzeni.\nWspółpracuje z zespołami CSIRT i organami właściwymi do spraw cyberbezpieczeństwa w celu wzmocnienia krajowego systemu cyberbezpieczeństwa.\nBierze udział w wykonywaniu działań zmierzających do integracji krajowego systemu cyberbezpieczeństwa z krajowym systemem zarządzania kryzysowego poprzez unifikację procedur oraz czynności kontrolnych w celu zwiększenia odporności tych systemów.\nBierze udział w wykonywaniu zadań w ramach Krajowych ram zarządzania kryzysowego w cyberbezpieczeństwie w celu zwiększenia odporności systemu zarządzania kryzysowego, koordynacji działań między organami publicznymi a sektorem prywatnym oraz szybszego reagowania na zagrożenia.\nDokonuje oceny obiektów oraz procesów pod kątem Infrastruktury krytycznej w celu prowadzenia wykazu obiektów IK będących we właściwości Ministra Cyfryzacji oraz zwiększenia odporności Państwa na kryzysy.\nWykonuje zadania organu właściwego ds. podmiotów krytycznych (m.in. prowadzenie kontroli, aktualizacja dokumentacji) w celu zwiększenia odporności Państwa na kryzysy.", "Starszy specjalista", "Wydział Zarządzania Kryzysowego, Spraw Obronnych i Cyberbezpieczeństwa" },
                { 13, "Departament Rozwoju i Bezpieczeństwa Sztucznej Inteligencji", "Naczelnik wydziału", "Zastępca dyrektora", "Zapewnienie wsparcia merytorycznego w zakresie rozwoju innowacyjnych rozwiązań opartych na sztucznej inteligencji poprzez rozwijanie mechanizmów wspierających zgodność z przepisami prawa oraz identyfikowanie barier dla rozwoju innowacyjności.", "Organizacja pracy i orientacja na osiąganie celów\nWykorzystywanie wiedzy i doskonalenie zawodowe\nWspółpraca", "None", "DSI WWI 5", "Analizuje i dokonuje oceny prawa krajowego, prawa Unii Europejskiej, orzecznictwa krajowego i międzynarodowego, wytycznych organów unijnych oraz praktyki stosowania prawa w obszarze sztucznej inteligencji, a także przygotowuje propozycje zmian systemowych, w celu zapewnienia skutecznego i spójnego stosowania regulacji dotyczących sztucznej inteligencji oraz wspierania rozwoju otoczenia prawnego sprzyjającego innowacjom i bezpieczeństwu.\nProwadzi sprawy dotyczące opinii indywidualnych oraz wyjaśnień dotyczących stosowania przepisów krajowych i unijnych z zakresu sztucznej inteligencji, w celu zapewnienia prawidłowej i jednolitej interpretacji oraz stosowania przepisów w tym obszarze.\nPrzygotowuje projekty informacji zawierające pogląd Komisji w sprawach sztucznej inteligencji, w celu zapewnienia jednolitego stanowiska Komisji oraz wsparcia prawidłowego stosowania przepisów w obszarze sztucznej inteligencji.\nAnalizuje bariery prawne i organizacyjne dla rozwoju innowacji w obszarze sztucznej inteligencji w celu opracowywania propozycji działań usprawniających.\nPrzygotowuje projekty odpowiedzi na pisma i inne wystąpienia kierowane do Komisji Rozwoju i Bezpieczeństwa Sztucznej Inteligencji w ramach właściwości wydziału, w celu zapewnienia sprawnej, rzetelnej i zgodnej z obowiązującymi przepisami realizacji zadań Komisji oraz właściwej komunikacji z podmiotami zewnętrznymi.", "Radca", "Wydział Wspierania Innowacyjności" },
                { 14, "Departament Współpracy Międzynarodowej", "Zastępca dyrektora", "Dyrektor", "Zapewnienie organizacji aktywności członków kierownictwa MC na arenie międzynarodowej, w tym przygotowania i realizacji zagranicznych wizyt członków kierownictwa MC oraz podejmowanych przez nich wizyt gości zagranicznych w Polsce, a także spotkań o charakterze międzynarodowym z podmiotami krajowymi, w zakresie Wydziału Koordynacji Aktywności Międzynarodowych. Koordynacja obsługi spraw budżetowo-finansowych Departamentu, w tym udzielania, obsługi i rozliczania dotacji i wydawanie stosownych decyzji administracyjnych. Zapewnienie prawidłowej realizacji umowy na świadczenia usług zakupu krajowych i zagranicznych biletów lotniczych i kolejowych w MC oraz zarządzanie procesem delegacji zagranicznych pracowników MC.", "Zarządzanie ludźmi\nPodejmowanie decyzji i odpowiedzialność\nWykorzystywanie wiedzy i doskonalenie zawodowe\nOrganizacja pracy i orientacja na osiąganie celów\nWspółpraca", "From5To9", "DWM WKAM 1", "Koordynuje pracę zespołu poprzez planowanie, organizowanie oraz nadzorowanie realizacji zadań zgodnie z wyznaczonymi celami. Zapewnia skuteczne zarządzanie zespołem poprzez monitorowanie postępów, delegowanie obowiązków oraz dostosowywanie działań do zmieniających się warunków organizacyjnych i prawnych. Wspiera pracowników poprzez wyznaczanie priorytetów, rozwiązywanie problemów oraz udzielanie wsparcia merytorycznego. Podejmuje działania motywacyjne i rozwojowe, sprzyjające podnoszeniu kwalifikacji zespołu oraz zwiększaniu zaangażowania pracowników w celu sprawnej i skutecznej realizacji zadań Wydziału.\nNadzoruje oraz bierze udział w przygotowaniu i organizacji zagranicznych wizyt członków kierownictwa MC oraz wizyt gości zagranicznych w Polsce, a także spotkań o charakterze międzynarodowym z podmiotami krajowymi w celu aktywnego kształtowania cyfrowej polityki międzynarodowej.\nKoordynuje obsługę spraw finansowo-budżetowych departamentu, w tym udzielanie i rozliczanie dotacji, łącznie z wydawaniem stosownych decyzji administracyjnych oraz przygotowywaniem projektu budżetu i zmian w planie wydatków DWM w celu terminowej i efektywnej realizacji budżetu DWM.\nKoordynuje przygotowania materiałów tezowo-informacyjnych (stanowiska, opinie) z zakresu dwustronnej lub wielostronnej współpracy z państwami europejskimi, pozaeuropejskimi lub organizacjami międzynarodowymi w obszarze cyfryzacji na potrzeby kierownictwa MC, Kancelarii Prezesa Rady Ministrów, Kancelarii Prezydenta, Kancelarii Sejmu i Senatu, innych ministerstw oraz urzędów na spotkania z odpowiednimi przedstawicielami tych krajów i organizacji (lub inicjatyw) w celu realizacji spotkań Kierownictwa MC.\nOrganizuje wydarzenia o charakterze międzynarodowym w obszarze cyfryzacji (m.in. konferencje, kongresy, seminaria, warsztaty, konsultacje), a także przygotowuje rekomendacje w zakresie uczestnictwa członków Kierownictwa MC w wydarzeniach o charakterze międzynarodowym w celu aktywnego kształtowania cyfrowej polityki międzynarodowej.\nBierze udział w pracach dotyczących realizacji umowy na świadczenia usług zakupu krajowych i zagranicznych biletów lotniczych i kolejowych oraz umowy na tłumaczenia ustne i pisemne (przeprowadzanie postępowań o udzielenie zamówień publicznych, ustalanie zapotrzebowania Ministerstwa na świadczenie usług, monitorowanie realizacji umów) w celu zapewnienia usług dla Ministerstwa Cyfryzacji.\nKoordynuje przygotowanie wkładów do odpowiedzi na zapytania publiczne, medialne, interpelacje poselskie, kontrole (w tym kontrole KAS i NIK) oraz audytu w celu dostarczenia rzetelnych informacji wnioskującym.\nUczestniczy w przygotowywaniu wkładu do planu zamówień publicznych Ministerstwa, jego aktualizacji oraz koordynowania zamówień publicznych w ramach Departamentu, w celu terminowej i efektywnej realizacji projektów DWM.", "Naczelnik wydziału", "Wydział Koordynacji Aktywności Międzynarodowych" }
            });

        migrationBuilder.CreateIndex(
            name: "IX_Employees_JobPositionId",
            table: "Employees",
            column: "JobPositionId");

        migrationBuilder.CreateIndex(
            name: "IX_JobPositions_Symbol",
            table: "JobPositions",
            column: "Symbol",
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_Employees_JobPositions_JobPositionId",
            table: "Employees",
            column: "JobPositionId",
            principalTable: "JobPositions",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Employees_JobPositions_JobPositionId",
            table: "Employees");

        migrationBuilder.DropTable(
            name: "JobPositions");

        migrationBuilder.DropIndex(
            name: "IX_Employees_JobPositionId",
            table: "Employees");

        migrationBuilder.DropColumn(
            name: "JobPositionId",
            table: "Employees");

        migrationBuilder.CreateTable(
            name: "JobDescriptions",
            columns: table => new
            {
                EmployeeId = table.Column<int>(type: "int", nullable: false),
                Purpose = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                RequiredCompetencies = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                SubordinatesCount = table.Column<int>(type: "int", nullable: true),
                Tasks = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false)
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
                { 16, "Obsługa wniosków o udostępnienie danych i przygotowywanie zestawień dla podmiotów zewnętrznych.", "Rzetelność\nKomunikacja pisemna\nWspółpraca", null, "Rozpatrywanie wniosków o udostępnienie danych wydziału\nPrzygotowywanie zestawień danych dla podmiotów zewnętrznych\nProwadzenie rejestru udostępnionych danych\nWspółpraca z działem prawnym przy odpowiedziach na wnioski" },
                { 17, "Przygotowywanie raportów i analiz danych o wnioskach obsługiwanych przez departament oraz dbanie o jakość danych źródłowych.", "Myślenie analityczne\nRzetelność\nKomunikacja pisemna\nKreatywność", null, "Przygotowywanie raportów kwartalnych i rocznych o wnioskach\nAnaliza trendów i przyczyn zmian w liczbie wniosków\nWeryfikacja zgodności danych z rejestrami źródłowymi\nPrzygotowywanie notatek informacyjnych dla kierownictwa\nWsparcie nowych pracowników w pracy z danymi" }
            });
    }
}
