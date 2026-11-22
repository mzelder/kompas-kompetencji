using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KompasKompetencji.Web.Data.Migrations;

/// <inheritdoc />
public partial class _20261122012748_AddCompetencyModelAndWrittenEvaluation : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "CompetencyIndicators",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Criterion = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                Number = table.Column<int>(type: "int", nullable: false),
                Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CompetencyIndicators", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CriterionDecisions",
            columns: table => new
            {
                EvaluationId = table.Column<int>(type: "int", nullable: false),
                Criterion = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                SuggestedLevel = table.Column<int>(type: "int", nullable: true),
                Level = table.Column<int>(type: "int", nullable: false),
                Justification = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                DecidedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CriterionDecisions", x => new { x.EvaluationId, x.Criterion });
                table.ForeignKey(
                    name: "FK_CriterionDecisions_Evaluations_EvaluationId",
                    column: x => x.EvaluationId,
                    principalTable: "Evaluations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "IndicatorBehaviors",
            columns: table => new
            {
                IndicatorId = table.Column<int>(type: "int", nullable: false),
                Level = table.Column<int>(type: "int", nullable: false),
                Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IndicatorBehaviors", x => new { x.IndicatorId, x.Level });
                table.ForeignKey(
                    name: "FK_IndicatorBehaviors_CompetencyIndicators_IndicatorId",
                    column: x => x.IndicatorId,
                    principalTable: "CompetencyIndicators",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "IndicatorRatings",
            columns: table => new
            {
                EvaluationId = table.Column<int>(type: "int", nullable: false),
                IndicatorId = table.Column<int>(type: "int", nullable: false),
                Level = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IndicatorRatings", x => new { x.EvaluationId, x.IndicatorId });
                table.ForeignKey(
                    name: "FK_IndicatorRatings_CompetencyIndicators_IndicatorId",
                    column: x => x.IndicatorId,
                    principalTable: "CompetencyIndicators",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_IndicatorRatings_Evaluations_EvaluationId",
                    column: x => x.EvaluationId,
                    principalTable: "Evaluations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "CompetencyIndicators",
            columns: new[] { "Id", "Criterion", "Number", "Text" },
            values: new object[,]
            {
                { 1, "Reliability", 1, "Wykonywanie zadań zgodnie z obowiązującymi standardami (przepisami, zasadami, procedurami) oraz dbałość o jakość rezultatów." },
                { 2, "Reliability", 2, "Dotrzymywanie zobowiązań (realizacja zadań) w wyznaczonym czasie." },
                { 3, "Communication", 1, "Koncentrowanie się na wypowiedzi rozmówcy i dbanie o wzajemne zrozumienie." },
                { 4, "Communication", 2, "Wypowiadanie się w sposób jasny i precyzyjny, dostosowany do sytuacji i rozmówcy (dobieranie stylu, języka i treści)." },
                { 5, "Communication", 3, "Formułowanie jasnych i precyzyjnych wypowiedzi pisemnych dostosowanych do sytuacji i odbiorcy (dobieranie stylu, języka i treści)." },
                { 6, "Communication", 4, "Przedstawianie przekonujących argumentów odpowiednio do sytuacji." },
                { 7, "KnowledgeAndDevelopment", 1, "Wykorzystywanie wiedzy przy realizacji zadań na zajmowanym stanowisku pracy." },
                { 8, "KnowledgeAndDevelopment", 2, "Doskonalenie wiedzy i umiejętności niezbędnych do wykonywania zadań na zajmowanym stanowisku pracy, w szczególności wyszukiwanie i pozyskiwanie wiedzy, korzystanie z dobrych praktyk i doświadczeń innych." },
                { 9, "KnowledgeAndDevelopment", 3, "Korzystanie z dostępnych źródeł informacji, rozwiązań i technologii." },
                { 10, "WorkOrganization", 1, "Określanie sposobu realizacji zadań/celów." },
                { 11, "WorkOrganization", 2, "Realizowanie zadań/celów w kolejności uwzględniającej ich wagę i pilność (ustalanie priorytetów działania)." },
                { 12, "WorkOrganization", 3, "Dobieranie niezbędnych zasobów umożliwiających realizację zadań/celów." },
                { 13, "WorkOrganization", 4, "Dostosowywanie się do zmieniających się okoliczności." },
                { 14, "WorkOrganization", 5, "Wytrwałość i zaangażowanie w realizację zadań/celów." },
                { 15, "Creativity", 1, "Proponowanie lub tworzenie nowych rozwiązań." },
                { 16, "Creativity", 2, "Nieschematyczne podejście do zadań." },
                { 17, "Cooperation", 1, "Włączanie się w działania realizowane razem z innymi osobami, jeśli istnieje taka możliwość." },
                { 18, "Cooperation", 2, "Dbanie o przepływ informacji." },
                { 19, "Cooperation", 3, "Dbanie o dobrą atmosferę i otwartość we współpracy." },
                { 20, "Cooperation", 4, "Dostrzeganie wkładu pracy innych osób." },
                { 21, "Cooperation", 5, "Branie pod uwagę zdania innych osób." },
                { 22, "Cooperation", 6, "Dzielenie się wiedzą i doświadczeniem." },
                { 23, "AnalyticalThinking", 1, "Rozróżnianie informacji istotnych i nieistotnych." },
                { 24, "AnalyticalThinking", 2, "Dostrzeganie zależności między informacjami." },
                { 25, "AnalyticalThinking", 3, "Formułowanie wniosków." },
                { 26, "Negotiation", 1, "Przygotowywanie się do negocjacji, w szczególności opracowanie projektu stanowiska, zakresu możliwych ustępstw i argumentacji." },
                { 27, "Negotiation", 2, "Stosowanie technik i strategii negocjacyjnych." },
                { 28, "Negotiation", 3, "Osiągnięcie efektu negocjacji akceptowanego przez przełożonych." },
                { 29, "PublicSpeaking", 1, "Wykorzystywanie różnych środków przekazu (np. narzędzi multimedialnych) i dbałość o sposób prezentacji (w tym autoprezentacji)." },
                { 30, "PublicSpeaking", 2, "Nawiązywanie i utrzymywanie kontaktu z audytorium." },
                { 31, "PublicSpeaking", 3, "Zarządzanie czasem wystąpienia, w szczególności przestrzeganie ram czasowych, elastyczne reagowanie na potrzeby audytorium." },
                { 32, "PublicSpeaking", 4, "Radzenie sobie ze stresem i emocjami, w szczególności wywołanymi krytyką i trudnymi pytaniami." },
                { 33, "CustomerOrientation", 1, "Taktowne zachowanie, wyrażające szacunek i zrozumienie dla drugiej osoby." },
                { 34, "CustomerOrientation", 2, "Reagowanie na potrzeby klienta/interesanta, w szczególności rozpoznawanie jego potrzeb i proponowanie rozwiązań." },
                { 35, "CrisisManagement", 1, "Rozpoznawanie sytuacji kryzysowych i informowanie o nich zainteresowanych." },
                { 36, "CrisisManagement", 2, "Podejmowanie działań mających na celu rozwiązanie kryzysu." },
                { 37, "CrisisManagement", 3, "Wyciąganie wniosków z sytuacji kryzysowych." },
                { 38, "StrategicManagement", 1, "Tworzenie wizji oraz wyznaczanie celów strategicznych w oparciu o całościowe i perspektywiczne widzenie organizacji, otoczenia oraz strategie rozwoju." },
                { 39, "StrategicManagement", 2, "Formułowanie celów operacyjnych na podstawie celów strategicznych." },
                { 40, "StrategicManagement", 3, "Analiza ryzyka planowanych działań o charakterze strategicznym." },
                { 41, "PeopleManagement", 1, "Przydzielanie zakresu odpowiedzialności, zadań i uprawnień pracownikom (delegowanie zadań)." },
                { 42, "PeopleManagement", 2, "Monitorowanie realizacji zadań oraz w razie potrzeby podejmowanie działań korygujących lub zapobiegawczych." },
                { 43, "PeopleManagement", 3, "Motywowanie pracowników." },
                { 44, "PeopleManagement", 4, "Udzielanie pracownikom informacji zwrotnej opartej na ocenie ich zaangażowania i sposobu realizacji zadań." },
                { 45, "PeopleManagement", 5, "Rozwiązywanie konfliktów." },
                { 46, "PeopleManagement", 6, "Wspieranie pracowników w rozwoju zawodowym, w szczególności identyfikowanie potrzeb rozwojowych, proponowanie działań rozwojowych oraz monitorowanie ich realizacji." },
                { 47, "DecisionMaking", 1, "Wypracowywanie alternatywnych rozwiązań na podstawie analizy informacji, w szczególności potencjalnego ryzyka." },
                { 48, "DecisionMaking", 2, "Podejmowanie decyzji (decyzyjność)." },
                { 49, "DecisionMaking", 3, "Przyjmowanie odpowiedzialności za efekty podjętych decyzji." }
            });

        migrationBuilder.InsertData(
            table: "IndicatorBehaviors",
            columns: new[] { "IndicatorId", "Level", "Description" },
            values: new object[,]
            {
                { 1, 1, "Pomija wymagane czynności lub kontrole, przez co rezultat wymaga istotnej korekty." },
                { 1, 2, "Stosuje podstawowe procedury, lecz pomija część wymagań lub sprawdza rezultat dopiero po zwróceniu uwagi." },
                { 1, 3, "Sprawdza aktualne wymagania, stosuje właściwą procedurę i weryfikuje rezultat przed przekazaniem." },
                { 1, 4, "W nietypowych sprawach ustala, które wymagania mają zastosowanie, i dobiera dodatkowe kontrole do ryzyka błędu." },
                { 1, 5, "Rozpoznaje powtarzające się przyczyny błędów i wdraża w swoim zakresie kontrole, które im zapobiegają." },
                { 2, 1, "Przekazuje zadania po terminie, a opóźnienie zgłasza dopiero po interwencji lub upływie terminu." },
                { 2, 2, "Realizuje zadania w terminie przy przypomnieniach lub zgłasza zagrożenie zbyt późno, by zmienić plan." },
                { 2, 3, "Dotrzymuje uzgodnionych terminów. Z wyprzedzeniem zgłasza ryzyko opóźnienia i uzgadnia dalsze kroki." },
                { 2, 4, "Przy zadaniach zależnych od innych ustala terminy cząstkowe i reaguje na odchylenia, zanim zagrożą zobowiązaniu." },
                { 2, 5, "Przy powtarzających się zagrożeniach identyfikuje ich przyczyny, uzgadnia zmiany w organizacji pracy i sprawdza ich wpływ na terminowość." },
                { 3, 1, "Przerywa wypowiedź lub odpowiada, pomijając kluczowe informacje przekazane przez rozmówcę." },
                { 3, 2, "Wysłuchuje wypowiedzi, lecz pozostawia istotne niejasności i sprawdza swoje rozumienie dopiero po zwróceniu uwagi." },
                { 3, 3, "Pozwala rozmówcy dokończyć wypowiedź, dopytuje o niejasności i potwierdza kluczowe ustalenia." },
                { 3, 4, "W rozmowie o niejednoznacznych kwestiach parafrazuje stanowisko rozmówcy i wyjaśnia rozbieżności w interpretacji." },
                { 3, 5, "W rozmowie wielostronnej porządkuje różne interpretacje i sprawdza, czy uczestnicy tak samo rozumieją ustalenia oraz kwestie otwarte." },
                { 4, 1, "Przekazuje informacje bez wskazania głównej myśli lub używa pojęć, których rozmówca nie może zrozumieć bez wyjaśnienia." },
                { 4, 2, "Przekazuje sedno sprawy, lecz rozmówca musi dopytywać o znaczenie, kontekst lub dalsze kroki." },
                { 4, 3, "Porządkuje wypowiedź, wyjaśnia potrzebne pojęcia i dostosowuje język oraz szczegółowość do rozmówcy i sytuacji." },
                { 4, 4, "Wyjaśnia złożone kwestie prostym językiem, zachowując istotne rozróżnienia, i dostosowuje wyjaśnienie do pytań rozmówcy." },
                { 4, 5, "W złożonej rozmowie z osobami o różnym poziomie wiedzy przekazuje wspólną konkluzję i dobiera wyjaśnienia do potrzeb poszczególnych odbiorców." },
                { 5, 1, "Tworzy tekst, z którego odbiorca nie może ustalić celu, stanowiska lub oczekiwanego działania." },
                { 5, 2, "Przekazuje główną informację, lecz tekst wymaga uzupełnienia kontekstu, uporządkowania albo doprecyzowania ustaleń." },
                { 5, 3, "Formułuje spójny tekst. Wskazuje cel i kluczowe informacje, a w razie potrzeby określa dalsze działanie i termin." },
                { 5, 4, "W złożonej sprawie wyodrębnia najważniejszą informację, oddziela ustalenia od kwestii otwartych i precyzuje odpowiedzialność." },
                { 5, 5, "Przygotowuje jednoznaczne komunikaty dla odbiorców o różnych potrzebach, zachowując spójność danych, stanowiska i dalszych kroków." },
                { 6, 1, "Przedstawia stanowisko bez uzasadnienia lub przywołuje argumenty niezwiązane ze sprawą." },
                { 6, 2, "Podaje częściowe uzasadnienie, lecz pomija przesłanki ważne dla rozmówcy lub podstawowe zastrzeżenia." },
                { 6, 3, "Uzasadnia stanowisko trafnymi faktami i przykładami oraz odpowiada na główne zastrzeżenia." },
                { 6, 4, "W złożonej sprawie dobiera argumenty do kryteriów decyzji, odpowiada na kontrargumenty i objaśnia ograniczenia przywołanych danych." },
                { 6, 5, "Przy sprzecznych stanowiskach sprowadza dyskusję do wspólnych kryteriów, waży dowody i modyfikuje argumentację po uzyskaniu nowych informacji." },
                { 7, 1, "Stosuje metody nieadekwatne do zadania mimo dostępnych wskazówek, przez co wynik wymaga ponownego opracowania." },
                { 7, 2, "Stosuje podstawową wiedzę w zadaniach powtarzalnych, ale przy zmianie warunków potrzebuje wskazania właściwej metody." },
                { 7, 3, "Dobiera i stosuje wiedzę merytoryczną odpowiednią do zadania. W razie wątpliwości potwierdza przyjęty sposób działania." },
                { 7, 4, "Łączy wiedzę z kilku obszarów, by rozwiązać nietypową sprawę, i uzasadnia dobór metody oraz granice jej stosowania." },
                { 7, 5, "Sprawdza przydatność wiedzy i metod w nowych typach spraw, a sprawdzone rozwiązania przekłada na wskazówki do ponownego użycia." },
                { 8, 1, "Po zmianie wymagań lub po informacji o błędach nadal wykonuje zadanie według nieaktualnych zasad lub metod." },
                { 8, 2, "Uzupełnia wiedzę po wskazaniu konkretnej luki, lecz potrzebuje wsparcia, by zastosować nowe umiejętności." },
                { 8, 3, "Rozpoznaje luki istotne dla swoich zadań, korzysta z aktualnych materiałów i doświadczeń innych oraz stosuje zdobytą wiedzę." },
                { 8, 4, "Planuje rozwój pod kątem nadchodzących zadań, testuje wybrane praktyki i sprawdza ich wpływ na własną pracę." },
                { 8, 5, "Z wyprzedzeniem rozpoznaje nowe wymagania wobec swojej roli, rozwija potrzebne umiejętności i na podstawie efektów aktualizuje sposób pracy." },
                { 9, 1, "Pomija dostępne źródła lub używa danych i narzędzi niedostosowanych do zadania." },
                { 9, 2, "Korzysta ze znanego źródła lub narzędzia, lecz potrzebuje wsparcia w ocenie jego aktualności albo przydatności." },
                { 9, 3, "Dobiera dopuszczone źródła i narzędzia do celu oraz sprawdza aktualność informacji i poprawność wyniku." },
                { 9, 4, "Porównuje dostępne źródła i rozwiązania, rozstrzyga rozbieżności i dobiera narzędzie z uwzględnieniem jakości, czasu oraz zasad ochrony danych." },
                { 9, 5, "Testuje dopuszczone rozwiązania wobec konkretnych wymagań, dokumentuje ich ograniczenia i wprowadza sprawdzony sposób weryfikacji wyników." },
                { 10, 1, "Przystępuje do pracy bez ustalenia rezultatu ani kolejności działań, przez co pomija istotne etapy." },
                { 10, 2, "Ustala podstawowe kroki, lecz plan wymaga uzupełnienia o zależności, terminy lub oczekiwany rezultat." },
                { 10, 3, "Określa rezultat, główne kroki, ich kolejność i terminy odpowiednie do zadania." },
                { 10, 4, "Dla złożonego zadania uwzględnia zależności, punkty kontroli i sposób postępowania przy przewidywalnych odchyleniach." },
                { 10, 5, "Przy wielu współzależnych działaniach uzgadnia wspólny plan, wskazuje etapy krytyczne i przygotowuje warianty realizacji." },
                { 11, 1, "Wykonuje zadania według kolejności wpływu mimo znanych różnic w terminach i konsekwencjach opóźnień." },
                { 11, 2, "Uwzględnia pilność, lecz potrzebuje wsparcia, gdy zadania o podobnych terminach mają różną wagę." },
                { 11, 3, "Ustala kolejność zadań, uwzględniając ich wagę, pilność, terminy i wpływ na inne działania." },
                { 11, 4, "Przy zbiegu ważnych zadań porównuje skutki przesunięć, aktualizuje priorytety i uzgadnia wpływ na zobowiązania." },
                { 11, 5, "W sytuacji konkurujących priorytetów wielu stron przedstawia kryteria wyboru, uzgadnia kolejność i wskazuje warunki jej ponownej oceny." },
                { 12, 1, "Rozpoczyna zadanie bez ustalenia podstawowych zasobów i zgłasza ich brak po wstrzymaniu pracy." },
                { 12, 2, "Wskazuje część zasobów, lecz potrzebuje pomocy w oszacowaniu dostępności, czasu lub nakładu pracy." },
                { 12, 3, "Określa potrzebny czas, informacje, narzędzia i wsparcie. Sprawdza dostępność zasobów przed realizacją." },
                { 12, 4, "Dla złożonych zadań porównuje warianty wykorzystania zasobów i uzgadnia zastępstwa lub rezerwy." },
                { 12, 5, "Przy ograniczonych zasobach i współzależnych zadaniach przygotowuje warianty ich wykorzystania, wskazuje skutki wyboru i uzgadnia przesunięcia." },
                { 13, 1, "Kontynuuje pracę według pierwotnego planu mimo zmiany, która uniemożliwia osiągnięcie ustalonego rezultatu." },
                { 13, 2, "Zmienia sposób pracy po wskazaniu problemu, lecz wymaga wsparcia w ocenie konsekwencji dla dalszych działań." },
                { 13, 3, "Po zmianie warunków aktualizuje sposób pracy i informuje zainteresowanych o skutkach dla uzgodnionego planu." },
                { 13, 4, "Przy nagłej zmianie porównuje możliwe reakcje, wdraża uzgodnione rozwiązanie i zabezpiecza najważniejsze zobowiązania." },
                { 13, 5, "Na podstawie wczesnych sygnałów przygotowuje warianty działania i koordynuje przejście na wybrany wariant, gdy warunki się zmienią." },
                { 14, 1, "Po napotkaniu przeszkody przerywa realizację i pozostawia zadanie bez zgłoszenia lub dalszych ustaleń." },
                { 14, 2, "Po trudnościach wraca do zadania po przypomnieniu lub pozostawia uzgodnione działania bez domknięcia." },
                { 14, 3, "Realizuje uzgodnione etapy, reaguje na przeszkody i doprowadza zadanie do zakończenia lub uzgodnionego przekazania." },
                { 14, 4, "Przy powtarzających się przeszkodach podejmuje kolejne uzasadnione działania i pozyskuje wsparcie potrzebne do zakończenia zadania." },
                { 14, 5, "W długotrwałym zadaniu podtrzymuje postęp mimo kolejnych trudności i uzgadnia zmianę zakresu lub sposobu realizacji, gdy wymaga tego cel." },
                { 15, 1, "Powiela dotychczasowe rozwiązanie mimo wskazanego problemu i możliwości zaproponowania zmiany." },
                { 15, 2, "Zgłasza ogólny pomysł, który wymaga doprecyzowania, aby odpowiedzieć na rozpoznaną potrzebę." },
                { 15, 3, "Proponuje konkretne nowe rozwiązanie, wyjaśnia, na jaką potrzebę odpowiada, i wskazuje sposób jego wypróbowania." },
                { 15, 4, "Opracowuje warianty, testuje wybrane rozwiązanie w uzgodnionym zakresie i poprawia je na podstawie wyników." },
                { 15, 5, "Weryfikuje rozwiązanie według uzgodnionych kryteriów, dostosowuje je po testach i określa warunki szerszego wykorzystania." },
                { 16, 1, "Powtarza nieskuteczny sposób pracy mimo dostępnych przesłanek do zmiany podejścia." },
                { 16, 2, "Dostrzega ograniczenia utrwalonego sposobu pracy, lecz potrzebuje wskazania alternatywnego podejścia." },
                { 16, 3, "Kwestionuje założenie, które ogranicza rozwiązanie zadania, i wypróbowuje uzasadnioną alternatywę." },
                { 16, 4, "Przenosi użyteczną metodę z innego obszaru lub łączy znane metody w sposób dopasowany do nietypowego zadania." },
                { 16, 5, "Przeformułowuje złożony problem przez sprawdzenie jego założeń i porównuje skutki odmiennych podejść w praktyce." },
                { 17, 1, "Pomija uzgodnioną część wspólnego zadania lub działa bez uwzględnienia uzgodnień zespołu." },
                { 17, 2, "Włącza się po ponownej prośbie lub wykonuje swoją część bez uzgodnienia zależności z innymi osobami." },
                { 17, 3, "Podejmuje uzgodnioną część wspólnego zadania, koordynuje ją z innymi i przekazuje rezultaty zgodnie z ustaleniami." },
                { 17, 4, "Samodzielnie rozpoznaje, gdzie potrzebna jest współpraca, i uzgadnia podział działań lub oferuje konkretne wsparcie." },
                { 17, 5, "W zadaniu angażującym kilka zespołów łączy ich wkład, doprecyzowuje role i pomaga usunąć przeszkody we współdziałaniu." },
                { 18, 1, "Nie przekazuje uprawnionym osobom informacji potrzebnych do wykonania wspólnego zadania lub przekazuje je po czasie." },
                { 18, 2, "Przekazuje informacje po przypomnieniu albo pomija kontekst i odbiorcy muszą dopytywać o ustalenia." },
                { 18, 3, "Przekazuje uprawnionym odbiorcom aktualne informacje, ustalenia i zmiany w czasie pozwalającym wykorzystać je w pracy." },
                { 18, 4, "Z wyprzedzeniem określa, komu i kiedy potrzebna jest informacja, oraz potwierdza dotarcie komunikatów istotnych dla dalszych działań." },
                { 18, 5, "Uzgadnia i usprawnia przepływ informacji między zespołami, dbając o aktualność ustaleń, dostęp uprawnionych osób i poufność." },
                { 19, 1, "Używa deprecjonujących komentarzy, przerywa lub przenosi rozbieżność dotyczącą zadania na ocenę osób." },
                { 19, 2, "Utrzymuje rzeczowy ton w typowych sytuacjach, lecz przy napięciu pomija głos innych lub wycofuje się z potrzebnego uzgodnienia." },
                { 19, 3, "Odnosi się z szacunkiem do innych osób, zaprasza do wypowiedzi i omawia rozbieżności w odniesieniu do zadania." },
                { 19, 4, "Reaguje na sygnały napięcia, nazywa przedmiot rozbieżności i umożliwia stronom przedstawienie perspektyw." },
                { 19, 5, "W trudnej współpracy uzgadnia zasady rozmowy i sprawdza, czy pozwalają zgłaszać zastrzeżenia oraz podejmować wspólne działania." },
                { 20, 1, "Pomija udział innych osób, gdy przedstawia wspólny rezultat, lub nieprawidłowo przypisuje autorstwo." },
                { 20, 2, "Wymienia osoby uczestniczące w zadaniu po przypomnieniu albo dziękuje ogólnie, bez wskazania wkładu." },
                { 20, 3, "Wskazuje konkretny wkład współpracowników i przypisuje autorstwo zgodnie z przebiegiem prac." },
                { 20, 4, "Zauważa także mniej widoczny wkład i wyjaśnia jego znaczenie dla wyniku, dobierając formę uznania do sytuacji." },
                { 20, 5, "Przy zadaniach z udziałem wielu osób dba o rzetelne przypisanie wkładu na różnych etapach i odzwierciedla go w podsumowaniu prac." },
                { 21, 1, "Odrzuca stanowiska innych osób bez zapoznania się z ich argumentami lub bez odniesienia do nich." },
                { 21, 2, "Zbiera opinie, lecz wybiórczo je rozważa lub pozostawia bez odpowiedzi uwagi istotne dla zadania." },
                { 21, 3, "Zasięga opinii osób związanych z zadaniem, rozważa ich argumenty i wyjaśnia sposób uwzględnienia istotnych uwag." },
                { 21, 4, "Aktywnie zbiera także odmienne stanowiska i porównuje je według kryteriów dotyczących wspólnego celu." },
                { 21, 5, "W sytuacji rozbieżnych stanowisk pokazuje, jak ich rozważenie wpłynęło na rozwiązanie, i wyjaśnia odrzucenie kluczowych propozycji." },
                { 22, 1, "Pomija prośby o przekazanie wiedzy potrzebnej współpracownikom mimo możliwości i uprawnienia do jej udostępnienia." },
                { 22, 2, "Udostępnia informacje po ponownej prośbie lub w formie wymagającej dodatkowych wyjaśnień przed użyciem." },
                { 22, 3, "Przekazuje uprawnionym osobom potrzebne informacje, materiały i przykłady w formie pozwalającej wykonać zadanie." },
                { 22, 4, "Z własnej inicjatywy dzieli się wskazówkami z ukończonych spraw, objaśnia ich zastosowanie i odpowiada na pytania." },
                { 22, 5, "Przekształca powtarzające się doświadczenia w aktualizowane materiały lub instruktaż i usprawnia je na podstawie pytań oraz wyników pracy." },
                { 23, 1, "Uwzględnia informacje poboczne jako kluczowe albo pomija dane konieczne do rozstrzygnięcia sprawy." },
                { 23, 2, "Wybiera podstawowe informacje, lecz potrzebuje pomocy w usunięciu zbędnych danych lub ustaleniu, czego brakuje." },
                { 23, 3, "Selekcjonuje informacje według celu analizy i wskazuje braki mające znaczenie dla wyniku." },
                { 23, 4, "Przy obszernych lub niejednoznacznych danych stosuje jawne kryteria selekcji i sprawdza, które braki zmieniają obraz sprawy." },
                { 23, 5, "Przy wielu źródłach i konkurujących kryteriach uzasadnia zakres analizy oraz pokazuje, jak wybór danych wpływa na wynik." },
                { 24, 1, "Traktuje powiązane informacje oddzielnie albo przyjmuje związek na podstawie samej zbieżności." },
                { 24, 2, "Zauważa oczywiste powiązania, lecz pomija zależności istotne dla sprawy lub wymaga wsparcia przy ich sprawdzeniu." },
                { 24, 3, "Identyfikuje zależności istotne dla zadania, sprawdza dostępne przesłanki i odróżnia ustalenia od przypuszczeń." },
                { 24, 4, "Porównuje alternatywne wyjaśnienia, uwzględnia dodatkowe czynniki i sprawdza, które dane potwierdzają lub podważają zależność." },
                { 24, 5, "Opisuje współwystępujące zależności, wskazuje ich wpływ na przebieg sprawy i określa dane potrzebne do rozstrzygnięcia niepewności." },
                { 25, 1, "Przedstawia wnioski bez związku z danymi lub wbrew istotnym ustaleniom." },
                { 25, 2, "Formułuje częściowe wnioski, lecz pomija istotne dane lub przedstawia niezweryfikowane założenia jako ustalenia." },
                { 25, 3, "Formułuje logiczne wnioski, wskazuje ich podstawę i zaznacza ograniczenia zgromadzonych informacji." },
                { 25, 4, "W złożonej sprawie zestawia możliwe wnioski, ocenia ich oparcie w dowodach i wskazuje konsekwencje dla działania." },
                { 25, 5, "Sprawdza, czy wnioski utrzymują się po zmianie istotnych założeń, i określa, w jakich warunkach wymagają aktualizacji." },
                { 26, 1, "Przystępuje do rozmów bez określenia stanowiska, argumentów lub granic upoważnienia." },
                { 26, 2, "Przygotowuje stanowisko, lecz wymaga uzupełnienia argumentacji, granic ustępstw lub wariantu na wypadek braku porozumienia." },
                { 26, 3, "Przygotowuje projekt stanowiska, argumenty i zakres ustępstw. Potwierdza mandat oraz wariant na wypadek braku porozumienia." },
                { 26, 4, "Rozpoznaje interesy stron na podstawie dostępnych informacji i przygotowuje warianty warunkowych ustępstw wraz z ich konsekwencjami." },
                { 26, 5, "Dla wielostronnych negocjacji opracowuje scenariusze, warunki przejścia między nimi oraz sposób uzyskania decyzji wykraczającej poza mandat." },
                { 27, 1, "Powtarza stanowisko bez ustalenia interesów stron lub składa jednostronne ustępstwa bez jasnego celu." },
                { 27, 2, "Stosuje podstawowe pytania i propozycje, lecz przy zmianie przebiegu rozmów potrzebuje wskazania dalszej strategii." },
                { 27, 3, "Zadaje pytania o interesy stron, stosuje obiektywne kryteria i przedstawia warunkowe propozycje w granicach mandatu." },
                { 27, 4, "Dostosowuje strategię do sygnałów drugiej strony, przedstawia pakiety rozwiązań i sprawdza warunki ich przyjęcia." },
                { 27, 5, "W impasie lub rozmowach wielostronnych uzgadnia sposób dalszych rozmów i buduje wymianę ustępstw opartą na priorytetach stron." },
                { 28, 1, "Przedstawia wynik, którego kluczowe warunki wykraczają poza uzgodniony mandat, lub pomija istotne zobowiązania w podsumowaniu." },
                { 28, 2, "Dokumentuje część ustaleń, lecz przełożony potrzebuje dodatkowych wyjaśnień o warunkach, rozbieżnościach lub skutkach." },
                { 28, 3, "Przedstawia przełożonemu do akceptacji wynik zgodny z mandatem wraz z warunkami i kwestiami otwartymi. Przy braku porozumienia dokumentuje jego przyczyny." },
                { 28, 4, "Przedstawia do akceptacji wynik złożonych negocjacji wraz z oceną ustępstw, ryzyk i warunków wdrożenia." },
                { 28, 5, "W negocjacjach o dużych konsekwencjach przedstawia do akceptacji wykonalny wynik, chroniący kluczowe interesy, z jasno opisanymi zobowiązaniami i warunkami realizacji." },
                { 29, 1, "Używa materiałów nieczytelnych lub niedopasowanych do wystąpienia, co utrudnia śledzenie przekazu." },
                { 29, 2, "Materiały wspierają część przekazu, lecz nadmiar treści lub sposób ich prezentowania zaciera główną myśl." },
                { 29, 3, "Dobiera formę i czytelne materiały do celu, odbiorców i warunków. Sprawdza działanie narzędzi i przedstawia treść w uporządkowany sposób." },
                { 29, 4, "W złożonym wystąpieniu łączy środki przekazu tak, by objaśniały kluczowe treści, i w razie problemu technicznego płynnie zmienia formę." },
                { 29, 5, "Przy zróżnicowanej publiczności lub trudnych warunkach dopasowuje formę na bieżąco, zachowując dostępność, spójność i precyzję przekazu." },
                { 30, 1, "Prowadzi wystąpienie bez reagowania na pytania lub widoczne sygnały, że odbiorcy nie nadążają za treścią." },
                { 30, 2, "Nawiązuje kontakt na początku, lecz później reaguje na sygnały audytorium dopiero po wyraźnej interwencji." },
                { 30, 3, "Nawiązuje kontakt z odbiorcami, obserwuje ich reakcje i odnosi się do pytań lub sygnałów niezrozumienia." },
                { 30, 4, "Sprawdza rozumienie kluczowych treści i w razie potrzeby zmienia tempo, przykłady lub sposób angażowania audytorium." },
                { 30, 5, "W zróżnicowanym audytorium angażuje różne grupy, porządkuje rozbieżne pytania i dostosowuje interakcję do celu wystąpienia." },
                { 31, 1, "Przekracza ustalony czas lub kończy bez kluczowych treści mimo sygnałów o upływie czasu." },
                { 31, 2, "Kontroluje czas sporadycznie i po wydłużeniu jednej części skraca pozostałe bez uwzględnienia ich znaczenia." },
                { 31, 3, "Planuje czas części, monitoruje go i w razie potrzeby skraca wątki poboczne, zachowując kluczowe treści i podsumowanie." },
                { 31, 4, "Przy zmianie limitu lub większej liczbie pytań świadomie przesuwa akcenty i uzgadnia przeniesienie tematów pobocznych." },
                { 31, 5, "W wydarzeniu z wieloma zależnościami koordynuje zmianę harmonogramu z prowadzącym, zachowując cel wystąpienia i uzgodnione ramy." },
                { 32, 1, "W odpowiedzi na krytykę przechodzi do uwag osobistych, przerywa rozmowę lub traci ciągłość prezentacji." },
                { 32, 2, "Odpowiada reaktywnie lub odchodzi od pytania i potrzebuje wsparcia, by wrócić do rzeczowego przekazu." },
                { 32, 3, "Przyjmuje trudne pytanie lub krytykę, odpowiada rzeczowo, rozpoznaje granice swojej wiedzy i wraca do głównego wątku." },
                { 32, 4, "Przy ostrym pytaniu oddziela istotę sprawy od tonu wypowiedzi, doprecyzowuje zarzut i utrzymuje rzeczowy przebieg rozmowy." },
                { 32, 5, "W sytuacji silnego napięcia porządkuje wymianę, odnosi się do faktów i ustala sposób odpowiedzi na kwestie wymagające weryfikacji." },
                { 33, 1, "Używa lekceważących sformułowań, przerywa lub ignoruje uzasadnioną prośbę o wyjaśnienie." },
                { 33, 2, "Zachowuje poprawny ton w typowych sprawach, lecz przy napięciu odpowiada zdawkowo lub pomija zgłoszoną trudność." },
                { 33, 3, "Wysłuchuje rozmówcy, odnosi się z szacunkiem do jego sytuacji i taktownie wyjaśnia także ograniczenia." },
                { 33, 4, "W sytuacji napięcia uznaje trudność zgłaszaną przez rozmówcę i dostosowuje język lub tempo, zachowując jasność i równe traktowanie." },
                { 33, 5, "W rozmowie o dużym napięciu obniża temperaturę wymiany, jasno stawia granice i prowadzi rozmowę w sposób respektujący godność stron." },
                { 34, 1, "Pomija istotę zgłoszenia lub wskazuje działanie nieadekwatne do sytuacji i dostępnej ścieżki." },
                { 34, 2, "Rozpoznaje typowe zgłoszenie, lecz przy niejednoznacznej sprawie pomija pytania potrzebne do dobrania rozwiązania." },
                { 34, 3, "Dopytuje o potrzebę i proponuje rozwiązanie dostępne w ramach procedur albo wskazuje właściwą ścieżkę i dalsze kroki." },
                { 34, 4, "W złożonej sprawie ustala priorytety i bariery, porównuje dopuszczalne opcje oraz koordynuje przekazanie sprawy." },
                { 34, 5, "Przy nietypowych potrzebach angażuje właściwe osoby, uzgadnia wykonalną ścieżkę oraz sprawdza, czy przekazanie lub rozwiązanie odpowiada ustalonej potrzebie." },
                { 35, 1, "Pomija wyraźne sygnały kryzysu lub nie przekazuje informacji właściwym osobom w wymaganym czasie." },
                { 35, 2, "Rozpoznaje sytuację po jej eskalacji lub przekazuje niepełny komunikat, który wymaga dodatkowego ustalania faktów." },
                { 35, 3, "Rozpoznaje sytuację według kryteriów eskalacji i informuje właściwe osoby o faktach, skali zagrożenia i potrzebnych działaniach." },
                { 35, 4, "Łączy wczesne sygnały, weryfikuje skalę zagrożenia i przekazuje aktualizacje wraz z wyraźnym wskazaniem niepewności." },
                { 35, 5, "Przy rozproszonych sygnałach ustala, co wymaga pilnej weryfikacji, uruchamia właściwą eskalację i dba o spójność komunikatów." },
                { 36, 1, "Zwleka z działaniami pozostającymi w jego zakresie lub podejmuje działania pogłębiające zakłócenie." },
                { 36, 2, "Wykonuje pojedyncze czynności po instrukcji, lecz potrzebuje wsparcia w ustaleniu ich kolejności lub zasięgu." },
                { 36, 3, "Uruchamia właściwe działania w granicach uprawnień, zabezpiecza priorytetowe sprawy i przekazuje kwestie wykraczające poza swój zakres." },
                { 36, 4, "Dostosowuje działania do rozwoju sytuacji, uzgadnia kolejność i zasoby oraz weryfikuje skuteczność podjętej reakcji." },
                { 36, 5, "W złożonym kryzysie koordynuje działania w swoim zakresie, rozstrzyga kolizje priorytetów i uzgadnia warunki przejścia do stabilizacji." },
                { 37, 1, "Zamyka sprawę bez ustalenia przyczyn i bez reakcji na powtórzenie się problemu." },
                { 37, 2, "Wskazuje bezpośredni problem, lecz pozostawia zalecenia bez konkretnego działania lub terminu." },
                { 37, 3, "Odtwarza przebieg zdarzenia, rozpoznaje przyczyny i uzgadnia działania, odpowiedzialność oraz terminy." },
                { 37, 4, "Weryfikuje przyczyny z uczestnikami, wdraża zmiany w swoim zakresie i sprawdza ich skuteczność." },
                { 37, 5, "Przekłada wnioski z kryzysu na zmiany procedur lub ćwiczeń, uzgadnia ich zastosowanie i po wdrożeniu ponownie ocenia ryzyko." },
                { 38, 1, "Nie określa kierunku działania albo formułuje cele oderwane od misji i uwarunkowań organizacji." },
                { 38, 2, "Formułuje ogólny kierunek, lecz pomija część istotnych uwarunkowań lub nie wskazuje priorytetów." },
                { 38, 3, "Wyznacza spójny kierunek i cele strategiczne na podstawie misji, danych o organizacji i analizy otoczenia." },
                { 38, 4, "Porównuje scenariusze rozwoju, uzasadnia wybór priorytetów i określa założenia wymagające okresowej weryfikacji." },
                { 38, 5, "Uzgadnia kierunek wobec rozbieżnych interesów, rozstrzyga długofalowe kompromisy i wyznacza warunki aktualizacji strategii." },
                { 39, 1, "Wyznacza zadania bez powiązania z celami strategicznymi lub bez określenia oczekiwanych rezultatów." },
                { 39, 2, "Wskazuje powiązanie z kierunkiem strategicznym, lecz cele operacyjne pozostawia bez miernika, terminu lub odpowiedzialności." },
                { 39, 3, "Przekłada cele strategiczne na konkretne rezultaty operacyjne, określając mierniki, terminy i odpowiedzialność." },
                { 39, 4, "Uzgadnia cele operacyjne między obszarami, uwzględnia zależności i rozstrzyga konflikty priorytetów oraz zasobów." },
                { 39, 5, "Sprawdza, czy wyniki operacyjne przekładają się na efekty strategiczne, i na tej podstawie aktualizuje uzgodnione cele." },
                { 40, 1, "Podejmuje planowanie strategiczne bez rozpoznania istotnych zagrożeń lub ignoruje udokumentowane ryzyka." },
                { 40, 2, "Wymienia główne ryzyka, lecz pomija ich skalę, działania ograniczające lub odpowiedzialność." },
                { 40, 3, "Ocenia prawdopodobieństwo i skutki istotnych ryzyk oraz określa działania, odpowiedzialność i sposób monitorowania." },
                { 40, 4, "Analizuje zależności między ryzykami, porównuje scenariusze i ustala progi uruchomienia działań ograniczających." },
                { 40, 5, "Sprawdza odporność wybranej strategii na zmianę warunków, ustala sygnały ostrzegawcze i aktualizuje reakcje w cyklu przeglądów." },
                { 41, 1, "Przydziela zadanie bez określenia rezultatu, odpowiedzialności lub uprawnień koniecznych do wykonania." },
                { 41, 2, "Określa część oczekiwań, lecz pozostawia niejasność co do granic decyzji, terminu lub sposobu przekazania wyniku." },
                { 41, 3, "Ustala z pracownikiem rezultat, termin, zakres odpowiedzialności, uprawnienia i sposób uzgadniania wyjątków." },
                { 41, 4, "Dopasowuje zakres samodzielności i wsparcia do złożoności zadania oraz przygotowania pracownika, a przy zależnościach uzgadnia role." },
                { 41, 5, "W zadaniach przekrojowych porządkuje granice decyzji między rolami i na podstawie efektów koryguje podział odpowiedzialności." },
                { 42, 1, "Sprawdza stan zadania dopiero po terminie lub po ujawnieniu problemu mimo dostępnych sygnałów odchylenia." },
                { 42, 2, "Monitoruje zadania doraźnie, ale nie uzgadnia działań po rozpoznaniu odchylenia lub sprawdza je z opóźnieniem." },
                { 42, 3, "Ustala punkty kontroli, sprawdza postęp względem oczekiwań i uzgadnia korektę wraz z terminem sprawdzenia efektu." },
                { 42, 4, "Dostosowuje monitoring do ryzyka i samodzielności pracowników oraz usuwa przeszkody, zanim wpłyną na wynik." },
                { 42, 5, "Wprowadza spójny sposób monitorowania, identyfikuje powtarzalne przyczyny odchyleń i sprawdza skuteczność działań zapobiegawczych." },
                { 43, 1, "Pomija zgłaszane bariery i wkład pracowników lub reaguje w sposób zniechęcający do podejmowania inicjatywy." },
                { 43, 2, "Stosuje ogólne zachęty i sporadyczne pochwały, bez powiązania ich z konkretnym wkładem albo potrzebami pracownika." },
                { 43, 3, "Wyjaśnia znaczenie zadań, docenia konkretny wkład i reaguje na zgłaszane bariery w zakresie swoich możliwości." },
                { 43, 4, "Dobiera formy uznania, samodzielności i wsparcia na podstawie rozmów z pracownikami oraz sprawdza ich efekty." },
                { 43, 5, "Diagnozuje powtarzające się organizacyjne przyczyny spadku zaangażowania i w uzgodnionym zakresie wprowadza oraz ocenia zmiany." },
                { 44, 1, "Pomija potrzebną informację zwrotną lub zastępuje ją ogólną oceną osoby bez wskazania zachowania." },
                { 44, 2, "Przekazuje feedback z opóźnieniem lub bez konkretnego przykładu i informacji o oczekiwanym działaniu." },
                { 44, 3, "Omawia konkretne zachowanie i jego wpływ, wysłuchuje perspektywy pracownika oraz uzgadnia, co utrzymać lub zmienić." },
                { 44, 4, "W trudnych rozmowach odwołuje się do sprawdzonych przykładów, uzgadnia wykonalne działania i wraca do ich efektów." },
                { 44, 5, "Dopasowuje cykl rozmów do sytuacji pracowników, sprawdza zmianę na podstawie obserwacji i aktualizuje uzgodnienia." },
                { 45, 1, "Pozostawia zgłoszony konflikt bez reakcji albo odnosi się do niego bez wysłuchania stron i ustalenia faktów." },
                { 45, 2, "Podejmuje interwencję po nasileniu sporu, ale nie doprowadza do jasnego uzgodnienia działań lub dalszej ścieżki." },
                { 45, 3, "Wysłuchuje stron, ustala przedmiot sporu, uzgadnia działania i sprawdza ich wykonanie albo kieruje sprawę we właściwym trybie." },
                { 45, 4, "Reaguje na wczesne oznaki konfliktu, porządkuje interesy stron i pomaga uzgodnić trwałe zasady współpracy." },
                { 45, 5, "Rozpoznaje powtarzalne źródła sporów, wprowadza zmiany w organizacji współpracy i sprawdza ich wpływ na przebieg konfliktów." },
                { 46, 1, "Pomija rozpoznane potrzeby rozwojowe lub kieruje na działania bez powiązania z zadaniami i potrzebami pracownika." },
                { 46, 2, "Ustala pojedyncze działania rozwojowe, lecz nie określa celu, terminu lub sposobu sprawdzenia efektu." },
                { 46, 3, "Rozpoznaje potrzeby z pracownikiem, uzgadnia cel, działania i terminy oraz omawia postęp i zastosowanie w pracy." },
                { 46, 4, "Dobiera rozwój do luki i przyszłych zadań, łączy różne formy uczenia oraz modyfikuje plan na podstawie efektów." },
                { 46, 5, "Z wyprzedzeniem planuje rozwój pod kątem przyszłych potrzeb zespołu i sprawdza, jak działania przekładają się na samodzielność oraz jakość pracy." },
                { 47, 1, "Przedstawia jedną opcję bez analizy dostępnych informacji i pomija istotne ryzyko." },
                { 47, 2, "Wskazuje alternatywy, lecz porównuje je wybiórczo albo pomija ważne konsekwencje i ograniczenia." },
                { 47, 3, "Porównuje wykonalne warianty według istotnych kryteriów i wskazuje główne ryzyka oraz ograniczenia." },
                { 47, 4, "W złożonej sprawie sprawdza kluczowe założenia, porównuje scenariusze i wskazuje działania ograniczające ryzyko." },
                { 47, 5, "Przy dużej niepewności ocenia odporność wariantów na zmianę założeń oraz określa warunki wyboru, korekty lub wycofania." },
                { 48, 1, "Zwleka z rozstrzygnięciem pomimo dostępnych przesłanek albo podejmuje decyzję bez niezbędnych ustaleń lub poza zakresem uprawnień." },
                { 48, 2, "Podejmuje decyzje w typowych sprawach po przypomnieniu lub odkłada złożone rozstrzygnięcie bez wskazania brakujących przesłanek i terminu." },
                { 48, 3, "Podejmuje i komunikuje uzasadnioną decyzję w czasie adekwatnym do sprawy. W razie potrzeby uzyskuje konsultację lub przekazuje sprawę zgodnie z uprawnieniami." },
                { 48, 4, "Terminowo rozstrzyga sprawy złożone na podstawie kluczowych informacji i określa przesłanki ponownej oceny decyzji." },
                { 48, 5, "W warunkach presji i niepewności dobiera tryb decyzji do jej skutków i odwracalności oraz określa warunki dalszych rozstrzygnięć." },
                { 49, 1, "Nie wyjaśnia skutków swojej decyzji lub pozostawia ujawnione konsekwencje bez reakcji w swoim zakresie." },
                { 49, 2, "Odnosi się do skutków po interwencji, lecz nie ustala dalszych działań albo nie sprawdza ich wykonania." },
                { 49, 3, "Wyjaśnia przesłanki decyzji, monitoruje jej skutki i podejmuje potrzebną korektę w swoim zakresie." },
                { 49, 4, "W razie niekorzystnych skutków informuje zainteresowanych, inicjuje działania naprawcze i potwierdza ich wykonanie." },
                { 49, 5, "Przy decyzjach o szerokich konsekwencjach monitoruje skutki w dłuższym okresie i przekłada wnioski oraz rezultaty korekt na kolejne decyzje." }
            });

        migrationBuilder.CreateIndex(
            name: "IX_CompetencyIndicators_Criterion_Number",
            table: "CompetencyIndicators",
            columns: new[] { "Criterion", "Number" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_IndicatorRatings_IndicatorId",
            table: "IndicatorRatings",
            column: "IndicatorId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CriterionDecisions");

        migrationBuilder.DropTable(
            name: "IndicatorBehaviors");

        migrationBuilder.DropTable(
            name: "IndicatorRatings");

        migrationBuilder.DropTable(
            name: "CompetencyIndicators");
    }
}
