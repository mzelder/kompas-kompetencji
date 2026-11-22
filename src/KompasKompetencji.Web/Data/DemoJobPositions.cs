namespace KompasKompetencji.Web.Data;

public static class DemoJobPositions
{
    private const string DataDepartment = "Departament Danych i Usług Cyfrowych";
    private const string DataUnit = "Wydział Analiz Danych";
    private const string UnitHead = "Naczelnik wydziału";
    private const string DepartmentDirector = "Dyrektor departamentu";

    public static IReadOnlyList<JobPosition> All { get; } =
    [
        DataPosition(2, "DDU WAD 7", "Specjalista",
            "Realizacja zadań związanych z gromadzeniem, przetwarzaniem i udostępnianiem danych na potrzeby wydziału.",
            Lines(
                "Gromadzenie i porządkowanie danych z jednostek podległych",
                "Przygotowywanie zestawień i wizualizacji danych",
                "Udzielanie odpowiedzi na zapytania dotyczące danych publicznych",
                "Prowadzenie dokumentacji źródeł danych"),
            Lines("Myślenie analityczne", "Rzetelność", "Komunikacja", "Współpraca")),
        new JobPosition
        {
            Id = 3, Symbol = "DDU WAD 1", Title = UnitHead, Department = DataDepartment, Unit = DataUnit,
            DirectSupervisorTitle = DepartmentDirector,
            Purpose = "Kierowanie Wydziałem Analiz Danych, zapewnienie terminowej i rzetelnej realizacji zadań wydziału oraz rozwój kompetencji podległych pracowników.",
            Tasks = Lines(
                "Planowanie, organizowanie i nadzorowanie pracy wydziału",
                "Przydzielanie zadań i ocena ich realizacji przez pracowników",
                "Przygotowywanie stanowisk i rekomendacji dla dyrektora departamentu",
                "Współpraca z innymi komórkami organizacyjnymi i instytucjami",
                "Prowadzenie ocen okresowych i planowanie rozwoju zawodowego pracowników"),
            RequiredCompetencies = Lines("Zarządzanie zespołem", "Podejmowanie decyzji", "Myślenie strategiczne", "Zarządzanie rozwojem pracowników", "Komunikacja"),
            Subordinates = SubordinatesRange.UpTo4,
        },
        DataPosition(5, "DDU WAD 5", "Starszy specjalista",
            "Przygotowywanie i opiniowanie analiz dotyczących usług cyfrowych oraz koordynacja współpracy z dostawcami danych.",
            Lines(
                "Opracowywanie analiz wpływu planowanych zmian na usługi cyfrowe",
                "Opiniowanie projektów dokumentów pod kątem danych i wskaźników",
                "Koordynowanie współpracy z instytucjami przekazującymi dane",
                "Prowadzenie spotkań roboczych z udziałem innych komórek",
                "Przygotowywanie materiałów na posiedzenia zespołów międzyresortowych"),
            Lines("Myślenie analityczne", "Współpraca", "Komunikacja", "Orientacja na cele")),
        DataPosition(6, "DDU WAD 6", "Starszy specjalista",
            "Prowadzenie spraw dotyczących reagowania na incydenty i sytuacje kryzysowe w zakresie danych i usług cyfrowych departamentu.",
            Lines(
                "Udział w opracowaniu i aktualizacji planu reagowania na incydenty dotyczące danych i usług cyfrowych",
                "Analiza zgłoszonych incydentów i przygotowywanie rekomendacji działań naprawczych",
                "Współpraca z zespołami reagowania na incydenty w innych urzędach i instytucjach",
                "Udział w przygotowaniu i przeprowadzaniu ćwiczeń z reagowania na sytuacje kryzysowe",
                "Prowadzenie kontroli przestrzegania procedur bezpieczeństwa danych w jednostkach podległych"),
            Lines(
                "Organizacja pracy i orientacja na osiąganie celów",
                "Współpraca",
                "Myślenie analityczne",
                "Wykorzystywanie wiedzy i doskonalenie zawodowe",
                "Radzenie sobie w sytuacjach kryzysowych")),
        DataPosition(7, "DDU WAD 8", "Specjalista",
            "Realizacja zadań związanych z analizą danych i przygotowywaniem informacji dla kierownictwa.",
            Lines(
                "Przygotowywanie cyklicznych raportów z danych wydziału",
                "Analiza danych ankietowych i statystycznych",
                "Przygotowywanie projektów odpowiedzi na pisma",
                "Aktualizacja danych publikowanych w serwisie urzędu"),
            Lines("Myślenie analityczne", "Rzetelność", "Komunikacja pisemna")),
        DataPosition(8, "DDU WAD 2", "Główny specjalista",
            "Projektowanie i utrzymanie zestawów wskaźników wydziału oraz zapewnienie spójności metod analizy danych.",
            Lines(
                "Projektowanie wskaźników i metod pomiaru dla zadań departamentu",
                "Przygotowywanie analiz porównawczych na potrzeby kierownictwa",
                "Opracowywanie standardów opisu i weryfikacji źródeł danych",
                "Koordynowanie prac analitycznych realizowanych przez kilka osób wydziału"),
            Lines("Myślenie analityczne", "Orientacja na cele", "Współpraca", "Komunikacja pisemna")),
        DataPosition(11, "DDU WAD 3", "Główny specjalista",
            "Przygotowywanie raportów i analiz danych o wnioskach obsługiwanych przez departament oraz dbanie o jakość danych źródłowych.",
            Lines(
                "Przygotowywanie raportów kwartalnych i rocznych o wnioskach",
                "Analiza trendów i przyczyn zmian w liczbie wniosków",
                "Weryfikacja zgodności danych z rejestrami źródłowymi",
                "Przygotowywanie notatek informacyjnych dla kierownictwa",
                "Wsparcie nowych pracowników w pracy z danymi"),
            Lines("Myślenie analityczne", "Rzetelność", "Komunikacja pisemna", "Kreatywność")),
        new JobPosition
        {
            Id = 12, Symbol = "DC WZKSOiC 6", Title = "Starszy specjalista",
            Department = "Departament Cyberbezpieczeństwa",
            Unit = "Wydział Zarządzania Kryzysowego, Spraw Obronnych i Cyberbezpieczeństwa",
            DirectSupervisorTitle = UnitHead, IndirectSupervisorTitle = "Dyrektor",
            Purpose = "Prowadzenie spraw dotyczących zarządzania kryzysowego w cyberprzestrzeni pozostających w zakresie ministra właściwego ds. informatyzacji.",
            Tasks = Lines(
                "Bierze udział w opracowaniu i aktualizacji dokumentacji w zakresie zarządzania kryzysowego (np. Krajowego planu reagowania na incydenty i sytuacje kryzysowe w cyberbezpieczeństwie na dużą skalę) w celu zapewnienia obsługi zadań Ministra w zakresie zarządzania kryzysowego w cyberprzestrzeni.",
                "Bierze udział w realizacji zadań z obszaru zarządzania kryzysowego wynikających z członkostwa Polski w Organizacji Paktu Północnoatlantyckiego (NATO), Unii Europejskiej (UE) i Organizacji Narodów Zjednoczonych (ONZ) w celu zapewnienia stałej gotowości do reagowania na zagrożenia płynące z cyberprzestrzeni.",
                "Współpracuje z zespołami CSIRT i organami właściwymi do spraw cyberbezpieczeństwa w celu wzmocnienia krajowego systemu cyberbezpieczeństwa.",
                "Bierze udział w wykonywaniu działań zmierzających do integracji krajowego systemu cyberbezpieczeństwa z krajowym systemem zarządzania kryzysowego poprzez unifikację procedur oraz czynności kontrolnych w celu zwiększenia odporności tych systemów.",
                "Bierze udział w wykonywaniu zadań w ramach Krajowych ram zarządzania kryzysowego w cyberbezpieczeństwie w celu zwiększenia odporności systemu zarządzania kryzysowego, koordynacji działań między organami publicznymi a sektorem prywatnym oraz szybszego reagowania na zagrożenia.",
                "Dokonuje oceny obiektów oraz procesów pod kątem Infrastruktury krytycznej w celu prowadzenia wykazu obiektów IK będących we właściwości Ministra Cyfryzacji oraz zwiększenia odporności Państwa na kryzysy.",
                "Wykonuje zadania organu właściwego ds. podmiotów krytycznych (m.in. prowadzenie kontroli, aktualizacja dokumentacji) w celu zwiększenia odporności Państwa na kryzysy."),
            RequiredCompetencies = Lines(
                "Organizacja pracy i orientacja na osiąganie celów",
                "Współpraca",
                "Myślenie analityczne",
                "Wykorzystywanie wiedzy i doskonalenie zawodowe"),
            Subordinates = SubordinatesRange.None,
        },
        new JobPosition
        {
            Id = 13, Symbol = "DSI WWI 5", Title = "Radca",
            Department = "Departament Rozwoju i Bezpieczeństwa Sztucznej Inteligencji",
            Unit = "Wydział Wspierania Innowacyjności",
            DirectSupervisorTitle = UnitHead, IndirectSupervisorTitle = "Zastępca dyrektora",
            Purpose = "Zapewnienie wsparcia merytorycznego w zakresie rozwoju innowacyjnych rozwiązań opartych na sztucznej inteligencji poprzez rozwijanie mechanizmów wspierających zgodność z przepisami prawa oraz identyfikowanie barier dla rozwoju innowacyjności.",
            Tasks = Lines(
                "Analizuje i dokonuje oceny prawa krajowego, prawa Unii Europejskiej, orzecznictwa krajowego i międzynarodowego, wytycznych organów unijnych oraz praktyki stosowania prawa w obszarze sztucznej inteligencji, a także przygotowuje propozycje zmian systemowych, w celu zapewnienia skutecznego i spójnego stosowania regulacji dotyczących sztucznej inteligencji oraz wspierania rozwoju otoczenia prawnego sprzyjającego innowacjom i bezpieczeństwu.",
                "Prowadzi sprawy dotyczące opinii indywidualnych oraz wyjaśnień dotyczących stosowania przepisów krajowych i unijnych z zakresu sztucznej inteligencji, w celu zapewnienia prawidłowej i jednolitej interpretacji oraz stosowania przepisów w tym obszarze.",
                "Przygotowuje projekty informacji zawierające pogląd Komisji w sprawach sztucznej inteligencji, w celu zapewnienia jednolitego stanowiska Komisji oraz wsparcia prawidłowego stosowania przepisów w obszarze sztucznej inteligencji.",
                "Analizuje bariery prawne i organizacyjne dla rozwoju innowacji w obszarze sztucznej inteligencji w celu opracowywania propozycji działań usprawniających.",
                "Przygotowuje projekty odpowiedzi na pisma i inne wystąpienia kierowane do Komisji Rozwoju i Bezpieczeństwa Sztucznej Inteligencji w ramach właściwości wydziału, w celu zapewnienia sprawnej, rzetelnej i zgodnej z obowiązującymi przepisami realizacji zadań Komisji oraz właściwej komunikacji z podmiotami zewnętrznymi."),
            RequiredCompetencies = Lines(
                "Organizacja pracy i orientacja na osiąganie celów",
                "Wykorzystywanie wiedzy i doskonalenie zawodowe",
                "Współpraca"),
            Subordinates = SubordinatesRange.None,
        },
        new JobPosition
        {
            Id = 14, Symbol = "DWM WKAM 1", Title = UnitHead,
            Department = "Departament Współpracy Międzynarodowej",
            Unit = "Wydział Koordynacji Aktywności Międzynarodowych",
            DirectSupervisorTitle = "Zastępca dyrektora", IndirectSupervisorTitle = "Dyrektor",
            Purpose = "Zapewnienie organizacji aktywności członków kierownictwa MC na arenie międzynarodowej, w tym przygotowania i realizacji zagranicznych wizyt członków kierownictwa MC oraz podejmowanych przez nich wizyt gości zagranicznych w Polsce, a także spotkań o charakterze międzynarodowym z podmiotami krajowymi, w zakresie Wydziału Koordynacji Aktywności Międzynarodowych. Koordynacja obsługi spraw budżetowo-finansowych Departamentu, w tym udzielania, obsługi i rozliczania dotacji i wydawanie stosownych decyzji administracyjnych. Zapewnienie prawidłowej realizacji umowy na świadczenia usług zakupu krajowych i zagranicznych biletów lotniczych i kolejowych w MC oraz zarządzanie procesem delegacji zagranicznych pracowników MC.",
            Tasks = Lines(
                "Koordynuje pracę zespołu poprzez planowanie, organizowanie oraz nadzorowanie realizacji zadań zgodnie z wyznaczonymi celami. Zapewnia skuteczne zarządzanie zespołem poprzez monitorowanie postępów, delegowanie obowiązków oraz dostosowywanie działań do zmieniających się warunków organizacyjnych i prawnych. Wspiera pracowników poprzez wyznaczanie priorytetów, rozwiązywanie problemów oraz udzielanie wsparcia merytorycznego. Podejmuje działania motywacyjne i rozwojowe, sprzyjające podnoszeniu kwalifikacji zespołu oraz zwiększaniu zaangażowania pracowników w celu sprawnej i skutecznej realizacji zadań Wydziału.",
                "Nadzoruje oraz bierze udział w przygotowaniu i organizacji zagranicznych wizyt członków kierownictwa MC oraz wizyt gości zagranicznych w Polsce, a także spotkań o charakterze międzynarodowym z podmiotami krajowymi w celu aktywnego kształtowania cyfrowej polityki międzynarodowej.",
                "Koordynuje obsługę spraw finansowo-budżetowych departamentu, w tym udzielanie i rozliczanie dotacji, łącznie z wydawaniem stosownych decyzji administracyjnych oraz przygotowywaniem projektu budżetu i zmian w planie wydatków DWM w celu terminowej i efektywnej realizacji budżetu DWM.",
                "Koordynuje przygotowania materiałów tezowo-informacyjnych (stanowiska, opinie) z zakresu dwustronnej lub wielostronnej współpracy z państwami europejskimi, pozaeuropejskimi lub organizacjami międzynarodowymi w obszarze cyfryzacji na potrzeby kierownictwa MC, Kancelarii Prezesa Rady Ministrów, Kancelarii Prezydenta, Kancelarii Sejmu i Senatu, innych ministerstw oraz urzędów na spotkania z odpowiednimi przedstawicielami tych krajów i organizacji (lub inicjatyw) w celu realizacji spotkań Kierownictwa MC.",
                "Organizuje wydarzenia o charakterze międzynarodowym w obszarze cyfryzacji (m.in. konferencje, kongresy, seminaria, warsztaty, konsultacje), a także przygotowuje rekomendacje w zakresie uczestnictwa członków Kierownictwa MC w wydarzeniach o charakterze międzynarodowym w celu aktywnego kształtowania cyfrowej polityki międzynarodowej.",
                "Bierze udział w pracach dotyczących realizacji umowy na świadczenia usług zakupu krajowych i zagranicznych biletów lotniczych i kolejowych oraz umowy na tłumaczenia ustne i pisemne (przeprowadzanie postępowań o udzielenie zamówień publicznych, ustalanie zapotrzebowania Ministerstwa na świadczenie usług, monitorowanie realizacji umów) w celu zapewnienia usług dla Ministerstwa Cyfryzacji.",
                "Koordynuje przygotowanie wkładów do odpowiedzi na zapytania publiczne, medialne, interpelacje poselskie, kontrole (w tym kontrole KAS i NIK) oraz audytu w celu dostarczenia rzetelnych informacji wnioskującym.",
                "Uczestniczy w przygotowywaniu wkładu do planu zamówień publicznych Ministerstwa, jego aktualizacji oraz koordynowania zamówień publicznych w ramach Departamentu, w celu terminowej i efektywnej realizacji projektów DWM."),
            RequiredCompetencies = Lines(
                "Zarządzanie ludźmi",
                "Podejmowanie decyzji i odpowiedzialność",
                "Wykorzystywanie wiedzy i doskonalenie zawodowe",
                "Organizacja pracy i orientacja na osiąganie celów",
                "Współpraca"),
            Subordinates = SubordinatesRange.From5To9,
        },
    ];

    private static JobPosition DataPosition(int id, string symbol, string title, string purpose, string tasks, string competencies) => new()
    {
        Id = id,
        Symbol = symbol,
        Title = title,
        Department = DataDepartment,
        Unit = DataUnit,
        DirectSupervisorTitle = UnitHead,
        IndirectSupervisorTitle = DepartmentDirector,
        Purpose = purpose,
        Tasks = tasks,
        RequiredCompetencies = competencies,
        Subordinates = SubordinatesRange.None,
    };

    private static string Lines(params string[] lines) => string.Join('\n', lines);
}
