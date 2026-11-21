# Kompas kompetencji

Prototyp aplikacji wspierającej przełożonego w ocenie pracownika służby cywilnej (hackathon, wyzwanie Ministerstwa Cyfryzacji). Obecny priorytet: **ścieżka oceny okresowej** (arkusz C, potem B).

**To projekt czysto demonstracyjny i nigdy nie zostanie wdrożony.** Ewentualne wdrożenie to osobna wersja budowana od zera w innych technologiach. Wynika z tego:
- Nie kieruj się wymaganiami produkcyjnymi (utwardzanie bezpieczeństwa, skalowanie, konfiguracja wdrożenia, kompatybilność wstecz). Wystarczy, że działa poprawnie na demo.
- Nie dodawaj w UI informacji o prototypie, ograniczeniach czy przyszłym wdrożeniu.
- **Nie przejmuj się zachowaniem istniejących danych w bazie.** Baza może być w każdej chwili usunięta i utworzona od nowa z migracji i danych demo (`HasData`, konta testowe). Przy zmianach modelu nie pisz migracji przenoszących ani uzupełniających dane (np. `migrationBuilder.Sql` z `UPDATE`); wystarczy migracja schematu i aktualne dane demo. W Development baza i tak jest usuwana i tworzona od nowa przy każdym starcie aplikacji.

- Treść zadania i wymagania: `podsumowanie_rozwiazania.md`, `tresci_zadania/Pełna treść zadania konkursowego.pdf`
- Zakres MVP i kolejność prac: `mvp.md` (stack z sekcji 7 jest nieaktualny, obowiązuje ten poniżej)
- Uruchomienie krok po kroku: `README.md`
- Otwarte decyzje i ryzyka (m.in. wybór dostawcy AI a wymóg „darmowych narzędzi AI”): `do_rozwazenia.md`. Przed zmianami w asystencie AI sprawdź, czy dana kwestia nie jest tam opisana.
- Plan wdrożenia asystenta AI z bazą wiedzy (RAG, BM25), podjęte decyzje i tickety w kolejności realizacji: `tickety_do_implementacji/` (zacznij od `README.md` w tym folderze). Cała baza wiedzy (źródła, skrypt, `knowledge-base.json`) ma być w jednym folderze `knowledge-base/`.
- Plan Etapu 0 oceny (opis stanowiska, wybór arkusza) i strony pracownika z etapami: `tickety_do_implementacji/ocena_etap_0/` (zacznij od `README.md` w tym folderze).

## Ustalenia od organizatorów

- **Ocena okresowa odbywa się co roku.** Pierwsza ocena okresowa jest po roku pracy, a nie na początku zatrudnienia. Cykl roczny służy jako domyślna podpowiedź terminu; sam termin oceny wpisuje HR (zob. proces niżej).
- Dotyczy to członków korpusu służby cywilnej. Ustawa o służbie cywilnej, rozporządzenia i poradniki KPRM mogą podawać inny okres (np. co 24 miesiące). **Na potrzeby tego projektu domyślnie przyjmujemy cykl roczny**, a każdą informację o innym okresie, z dowolnego źródła, traktujemy jako mniej istotną od tego ustalenia.

## Proces oceny okresowej (ustalenia z rozmów o wymaganiach)

Ten proces jest nadrzędny przy budowie kolejnych funkcji. Gdy obecny kod się z nim nie zgadza, kierujemy się procesem.

### Etapy cyklu oceny

Przebieg etapów jest taki jak w artefakcie „Luki w ścieżce oceny okresowej” (https://claude.ai/artifact/ULYd2HrnbtFT2dyK5vKiMt). Długość cyklu, terminy i brak powiadomień ustalamy według tego dokumentu, a nie artefaktu (artefakt zakłada cykl 24 miesiące i przypomnienia). Cykl zaczyna się w dniu zatrudnienia (pierwszy cykl) albo w dniu sporządzenia poprzedniej oceny.

- **Etap 0: opis stanowiska i wybór arkusza.** Przełożony przegląda opis stanowiska pracy (OSP) i zatwierdza arkusz. System podpowiada B (pracownik kieruje zespołem, `IsSupervisor`) albo C, a przełożony może zmienić wybór bez uzasadnienia.
- **Etap 1: rozmowa rozpoczynająca i część I arkusza** (w ciągu 30 dni od początku cyklu). Kryteria: 5 obowiązkowych i 0–3 dodatkowe, do tego oczekiwania, cele i potwierdzenie zapoznania pracownika. Data rozmowy rozpoczynającej to początek okresu oceny.
- **Etap 2: ocena pracownika**, jeden etap od końca Etapu 1 do terminu oceny. Obejmuje:
  - ocenianie przez cały okres (model kompetencji, notatki do kryteriów),
  - rozmowę oceniającą, około 2 miesiące przed terminem oceny, najpóźniej w dniu oceny,
  - ocenę na piśmie (część II arkusza): wskaźniki, sugerowany poziom z jawnej reguły, decyzja przełożonego, średnia, wynik, uzasadnienie i wnioski do IPRZ.
- **Etap 3: zakończenie i dokument.** Data sporządzenia oceny (koniec okresu oceny), potwierdzenie zapoznania, PDF i początek nowego cyklu.

W artefakcie i w `flow_oceny_okresowej.md` ocenianie, rozmowa oceniająca i ocena na piśmie to osobne etapy (II, III, IV), a zakończenie to Etap V. W aplikacji są połączone w jeden Etap 2 „Ocena pracownika”, a zakończenie to Etap 3. Etapy w aplikacji numerujemy cyframi arabskimi (0, 1, 2, 3).

Cały przebieg oceny odbywa się na **stronie konkretnego pracownika**: pasek kroków ze wszystkimi etapami, pod nim formularz bieżącego etapu, zakończone etapy tylko do odczytu.

### Przebieg w aplikacji

1. **HR wpisuje termin oceny ręcznie.** Po zatrudnieniu pracownika HR wprowadza datę, kiedy ma się odbyć ocena okresowa. Termin nie jest wyliczany automatycznie; cykl roczny (pierwsza po roku pracy, potem co rok) może jedynie podpowiadać datę w formularzu HR.
2. **Pracownik pojawia się na pulpicie przełożonego sam, dwa razy w cyklu.** Na początku cyklu pulpit pokazuje zadanie ustalenia arkusza i kryteriów (Etapy 0 i 1, w ciągu 30 dni). Gdy do terminu oceny zostaje mniej niż ok. 2 miesiące, pulpit pokazuje, z kim przełożony ma odbyć rozmowę oceniającą i że powinien wyznaczyć jej termin. HR kontaktuje się z przełożonym bezpośrednio, poza aplikacją; w aplikacji **nie ma przycisku ani akcji HR „przekaż przełożonemu”**.
3. **Przełożony wyznacza terminy rozmów.** **Wyznaczenie którejkolwiek rozmowy wymaga zakończonego Etapu 0.** Rozmowa rozpoczynająca odbywa się w ciągu 30 dni od początku cyklu. **Rozmowa oceniająca odbywa się najpóźniej w dniu oceny** (walidacja: data rozmowy ≤ termin oceny). Przełożony może później **zmienić termin rozmowy** (z tą samą walidacją).
4. **Każda rozmowa ma status wybierany przez przełożonego**, dokładnie dwa statusy: **Zaplanowana** (przełożony wyznaczył termin; obejmuje też termin uzgodniony z pracownikiem, osobnego statusu akceptacji nie ma) → **Odbyta**. Zmiana terminu rozmowy wraca status do „Zaplanowana”. **„Termin minął” nie jest statusem**, tylko dodatkową informacją wyliczaną automatycznie, gdy data rozmowy minęła, a rozmowa nie jest oznaczona jako odbyta (podpowiada: oznacz jako odbytą albo zmień termin). Zasada nadrzędna: aplikacja ma być jak najprostsza w użyciu, więc nie dokładamy statusów, które aplikacja może wyliczyć sama.
5. **Ocenę sporządza przełożony z pomocą asystenta AI.** AI tylko wspiera, decyzja o poziomie zawsze należy do przełożonego (zob. Konwencje).
6. **Po ocenie informacja trafia do HR**: że rozmowa się odbyła i ocena została sporządzona.
7. **Bez powiadomień.** Ani HR, ani przełożony nie dostają powiadomień, maili ani przypomnień w aplikacji. Cała komunikacja odbywa się przez **kalendarze**: HR i przełożeni widzą w swoich kalendarzach terminy ocen, terminy rozmów i ich statusy oraz informację o sporządzonej ocenie.

Wynikające z tego zasady dla aplikacji:
- Trzy różne terminy: **termin oceny** (ustala HR), **termin rozmowy rozpoczynającej** i **termin rozmowy oceniającej** (ustala przełożony, rozmowa oceniająca nie później niż termin oceny). W UI zawsze rozróżniamy je nazwą.
- Kalendarz jest głównym miejscem informacji: przełożony widzi terminy ocen swoich podwładnych i swoje rozmowy obu rodzajów, HR widzi terminy ocen, rozmowy i sporządzone oceny wszystkich pracowników.
- Pulpit przełożonego wyróżnia pracowników na początku cyklu bez ustalonego arkusza i kryteriów oraz pracowników, którym zbliża się termin oceny, a nie mają jeszcze wyznaczonej rozmowy oceniającej.
- Lista „Do zrobienia” na pulpicie przełożonego (`SupervisorOverview.ToDo`) pokazuje dziś: **najpierw rozmowy z minionym terminem, nieoznaczone jako odbyte** (przyciski „Oznacz jako odbytą” jednym kliknięciem i „Zmień termin”), **potem osoby z terminem oceny w ciągu 2 miesięcy (albo już po terminie)**: bez zatwierdzonego arkusza przycisk „Zatwierdź arkusz” (prowadzi na stronę pracownika), z arkuszem i bez rozmowy „Wyznacz termin”. Dalsze terminy ocen i rozmowy zaplanowane na przyszłość albo odbyte nie trafiają na listę; są w kalendarzu (tam też „termin minął” jako pomarańczowa ramka, a kliknięcie otwiera edycję).

Różnice względem obecnej implementacji (do dostosowania przy kolejnych zmianach):
- Termin oceny jest dziś liczony automatycznie z `EmployedSince` w `EvaluationSchedule`; ma być wpisywany przez HR (np. pole na ocenie lub osobny rekord tworzony przez HR), a wyliczenie roczne zostaje tylko jako podpowiedź.
- Aplikacja ma jedną rozmowę na ocenę (`ConversationAt`, `ConversationStatus`), która odpowiada rozmowie oceniającej (część Etapu 2). Brakuje rozmowy rozpoczynającej (najlepiej osobna encja rozmowy z typem), formularzy Etapów 1 i 3 oraz oceny na piśmie w Etapie 2 (Etap 0 jest gotowy, Etap 2 ma notatki bez modelu kompetencji i bez powiązania z kryteriami oraz wyznaczanie rozmowy oceniającej).
- Pulpit pokazuje pracownika tylko w oknie 2 miesięcy przed terminem oceny, a nie na początku cyklu.
- Okres oceny (`PeriodStart`, `PeriodEnd`) to dziś „termin oceny minus rok”; ma trwać od rozmowy rozpoczynającej do sporządzenia oceny.
- Strona pracownika ma formularze dla Etapu 0 i Etapu 2 (notatki); pozostałe etapy mają na razie sam opis, co przełożony na nich robi.
- HR nie ma jeszcze kalendarza z terminami ocen, rozmowami i sporządzonymi ocenami.
- Kalendarz przełożonego pokazuje dziś tylko rozmowy; powinien pokazywać też terminy ocen.

## Zasady UI i stylu

@design.md

## Stack

Blazor Web App (Interactive Server), .NET 11, EF Core 11, SQL Server (LocalDB), Radzen Blazor 13 (motyw `standard-dark`).

## Komendy

```powershell
dotnet build src/KompasKompetencji.Web
dotnet run --project src/KompasKompetencji.Web --launch-profile http     # http://localhost:5166
dotnet test                                                               # testy (bez wywołań API)
python knowledge-base/build_knowledge_base.py                             # przebudowa knowledge-base.json po zmianie źródeł
dotnet tool restore                                                       # raz, instaluje dotnet-ef
dotnet ef migrations add <Nazwa> --project src/KompasKompetencji.Web --output-dir Data/Migrations
```

W Development **przy każdym uruchomieniu baza jest usuwana i tworzona od nowa** (`EnsureDeleted` + `Migrate` w `Program.cs`), razem z danymi demo i kontami testowymi. Wszystko, co zostało zmienione w aplikacji, znika po restarcie.

## Konwencje

- Kod, nazwy plików, klas i tabel **po angielsku**. Teksty widoczne dla użytkownika **po polsku**.
- **Nie dodawaj komentarzy w kodzie** (ani `//`, ani `///`, ani `@* *@`, ani `/* */` w CSS) i nie zastanawiaj się, jakie komentarze dodać. Kod ma się tłumaczyć nazwami.
- **Kod C# komponentu Razor trzymaj w osobnym pliku `Nazwa.razor.cs`** obok `Nazwa.razor` (np. `AddEmployeeDialog.razor` + `AddEmployeeDialog.razor.cs`), jako `public partial class Nazwa` w przestrzeni nazw odpowiadającej folderowi (np. `KompasKompetencji.Web.Components.Pages.Hr`). W `.razor` nie ma bloków `@code` ani `@inject`: zostają tylko znaczniki i dyrektywy `@page`, `@using`, `@attribute`, `@layout`, `@inherits`. Serwisy wstrzykujemy w `.razor.cs` jako `[Inject] private DialogService DialogService { get; set; } = default!;` (na początku klasy). Plik `.razor.cs` nie widzi `@using` z `_Imports.razor`, więc potrzebne `using` (m.in. `Microsoft.AspNetCore.Components` dla `[Parameter]`) dopisz w nim jawnie. Dotyczy to też nowych komponentów.
- Dane pracowników są fikcyjne. Nie dodawaj prawdziwych danych osobowych.
- AI nigdy nie ustala poziomu oceny. Sugerowany poziom liczy jawna, deterministyczna reguła, decyzję podejmuje przełożony.

## Architektura

### Logowanie i role
- Role: `Auth/AppRoles.cs` (`Przelozony`, `HR`, `Admin`). Tam też nazwa wyświetlana, ikona i strona startowa roli.
- Logowanie cookie. Komponent interaktywny nie może ustawić cookie, więc `Login.razor` sprawdza hasło, wydaje jednorazowy bilet (`Auth/LoginTicketStore.cs`) i robi pełne przekierowanie na `/auth/signin` (`Auth/AuthEndpoints.cs`). Wylogowanie: GET `/auth/logout` (z menu użytkownika w nagłówku).
- Konta i hasła: `Auth/UserService.cs` (hash przez `PasswordHasher`). Konta testowe: `Auth/TestAccounts.cs`, przycisk szybkiego logowania działa tylko w Development.

### Widoki per rola
- Każda rola ma folder w `Components/Pages/`: `Przelozony/`, `Hr/`, `Admin/`. `_Imports.razor` folderu nadaje `[Authorize(Roles = ...)]` wszystkim stronom w środku. **Nowa strona roli trafia do jej folderu**, bez własnego atrybutu.
- Menu boczne dla roli: `Auth/RoleNavigation.cs`. Nowa strona w menu to jedna pozycja tutaj.
- `Routes.razor` używa `AuthorizeRouteView`: niezalogowany → `/login?returnUrl=...`, zła rola → „Brak dostępu”. `/` przekierowuje do panelu roli.
- Layouty: `MainLayout` (RadzenLayout z nagłówkiem i menu) dla zalogowanych, `AuthLayout` (bez menu) dla logowania.
- Wspólne komponenty: `Components/Shared/`.
- Zakładki przełożonego: Kalendarz rozmów (`/przelozony`, strona startowa; w kodzie `Przelozony/Dashboard.razor`), Moi pracownicy (`/przelozony/pracownicy`; nad tabelą wszystkie etapy w poziomie, każdy w swoim kolorze (`StageLegend`, `StageMarker`); tabela z kolumnami: pracownik, etap, termin oceny i „Następny krok”: na Etapie 0 „Zatwierdź arkusz”, na Etapie 1 sama informacja o rozmowie rozpoczynającej, na Etapie 2 rozmowa oceniająca z przyciskiem „Wyznacz termin” (wyszarzony przed oknem 2 miesięcy, szczegół mówi, od kiedy: `EvaluationTask.ConversationWindowOpensOn`) albo „Zmień termin”, które otwiera `ConversationDialog`), Oceny (`/przelozony/oceny`), Model kompetencji (`/przelozony/model-kompetencji`), Zasady i materiały (`/przelozony/zasady`). Oceny, Model kompetencji i Zasady są na razie puste.
- Pozycja menu, której adres jest prefiksem innych (`/przelozony`, `/hr`), ma `NavLinkMatch.All`, inaczej podświetla się razem z podstronami.

### Dane
- `Data/AppDbContext.cs`, encje w `Data/`, migracje w `Data/Migrations/`. Do bazy z komponentów przez `IDbContextFactory<AppDbContext>` (krótko żyjący kontekst na operację) albo przez serwis.
- Model organizacji:
  - `Employee`: każda osoba w urzędzie (imię, nazwisko, stanowisko, komórka organizacyjna, data zatrudnienia `EmployedSince`). Flaga `IsSupervisor` oznacza, że może być czyimś przełożonym. Przełożony też jest pracownikiem i ma własnych przełożonych.
  - `EmployeeSupervisor`: powiązanie wiele-do-wielu pracownik → przełożony (klucz złożony, check constraint „nie można być swoim przełożonym”). Pracownik może mieć kilku przełożonych.
  - `JobDescription` (tabela `JobDescriptions`, klucz `EmployeeId`): skrócony opis stanowiska pracy (OSP): cel, zadania i wymagane kompetencje (tekst, jedna pozycja w linii, listy przez `TaskList` i `RequiredCompetencyList`), liczba podległych osób. Tylko do odczytu w aplikacji.
  - `AppUser.EmployeeId` (opcjonalne, unikalne): konto w systemie powiązane z osobą. Konto testowe `przelozony` jest powiązane z Ewą Wiśniewską.
- Logika struktury: `Organization/OrganizationService.cs` (odczyt i zmiany) i `Organization/OrganizationChart.cs` (migawka w pamięci: przełożeni i podwładni danej osoby, lista przełożonych możliwych do przypisania bez tworzenia cyklu).

### Asystent AI
- Czat: `Components/Assistant/AssistantChat.razor` (`RadzenAIChat`, prompt systemowy, szablon źródeł z numerem fragmentu). Montowany w `MainLayout` dla zalogowanych. Polskie teksty Radzena: `Assistant/PolishRadzenLocalizer.cs`.
- Łańcuch `IChatClient` (`Program.cs`): `KnowledgeBaseChatClient` → `PseudonymizingChatClient` → logowanie → Claude (`AssistantClientFactory`) albo `MockChatClient` bez klucza.
  - `KnowledgeBaseChatClient`: odmowa bez modelu dla pytań o poziom oceny (`EvaluationLevelGuard`), wyszukanie 5 fragmentów, doklejenie ich do `ChatOptions.Instructions` (`KnowledgePrompt`), usunięcie bloków rozumowania z historii (API odrzuca je po zmianie promptu systemowego), źródła jako `CitationAnnotation` tylko dla przywołanych [n] (numer fragmentu w `FileId`).
  - `Assistant/Privacy/`: imiona i nazwiska z tabeli `Employees` zamieniane na „Pracownik A”… przed modelem i z powrotem w strumieniu odpowiedzi.
- Wyszukiwarka: `Assistant/KnowledgeBase/` (BM25, normalizacja `Text/TextNormalizer.cs`, słowa pomijalne, obcinanie słów do 7 znaków, synonimy). Baza wiedzy: `knowledge-base/` (źródła, skrypt Python, `knowledge-base.json` kopiowany do `bin/Assistant/`).
- Nowe pytanie, na które asystent źle trafia: dopisz je do `tests/KompasKompetencji.Tests/retrieval-questions.json` i popraw synonimy albo bazę, aż test przejdzie.
- Oceny:
  - `Evaluation`: ocena okresowa pracownika (oceniający, arkusz B/C, data zatwierdzenia arkusza `SheetConfirmedAt` (koniec Etapu 0), data potwierdzenia kryteriów `CriteriaConfirmedAt` (koniec Etapu 1; formularza Etapu 1 jeszcze nie ma, ustawiają ją tylko dane demo), data sporządzenia oceny na piśmie `WrittenEvaluationAt` (koniec Etapu 2; formularza oceny na piśmie jeszcze nie ma, ustawiają ją tylko dane demo), okres, termin oceny `DueDate`, status oceny: `NotStarted` → `CriteriaSet` → `InProgress` → `ReadyForConversation` → `Completed`) oraz rozmowa oceniająca (część Etapu 2): `ConversationAt` i `ConversationStatus` (`Scheduled` → `Held`). „Termin minął” wylicza `ConversationRules.IsPastDue`. Rekord powstaje przy zatwierdzeniu arkusza w Etapie 0 (ze statusem `NotStarted`); bez rekordu ocena jest „nierozpoczęta”, a termin wynika z harmonogramu.
  - Etapy: `Evaluations/EvaluationStage.cs` (`EvaluationStages.Current` wylicza bieżący etap z otwartej oceny: bez `SheetConfirmedAt` Etap 0, bez `CriteriaConfirmedAt` Etap 1, bez `WrittenEvaluationAt` Etap 2 „Ocena pracownika”, w przeciwnym razie Etap 3 „Zakończenie”; kolejne etapy dopisują tam swoje warunki). Dane strony pracownika: `EvaluationService.GetEmployeeEvaluationAsync` (pracownik, OSP, zadanie, otwarta ocena, etap, podpowiedź arkusza). Zatwierdzenie arkusza: `EvaluationService.ConfirmSheetAsync` (zakłada ocenę, jeśli jej nie ma; ponowne zatwierdzenie odrzucone).
  - Etap 2: notatki przełożonego `EvaluationNote` (tabela `EvaluationNotes`: otwarta ocena, data dodania, tekst do 2000 znaków; jeszcze bez powiązania z kryteriami, bo kryteriów z Etapu 1 nie ma). Formularz `Przelozony/Stages/AssessmentStage.razor`: nad notatkami karta „Rozmowa oceniająca” z przyciskiem „Wyznacz termin” / „Zmień termin” (przed oknem 2 miesięcy przycisk wyłączony, a podpis mówi, od kiedy można wyznaczyć termin), pod nią notatki: dodawanie i usuwanie (z potwierdzeniem), lista od najnowszej; na późniejszych etapach tylko do odczytu. Serwis: `EvaluationService.AddNoteAsync` / `DeleteNoteAsync` (tylko pracownik z zespołu na etapie „Ocena pracownika”), notatki w `EmployeeEvaluationView.Notes`.
  - Wyznaczanie i zmiana rozmowy: `EvaluationService.SaveConversationAsync` (sprawdza, że pracownik jest w zespole przełożonego i ma zatwierdzony arkusz; oceny nie zakłada). Zasady w `Evaluations/ConversationRules.cs`: rozmowa najpóźniej w dniu oceny (po terminie oceny: jak najszybciej, bez górnej granicy), status wynika z daty: data przyszła = tylko „Zaplanowana”, data z przeszłości = „Odbyta” (można od razu zapisać rozmowę, która już się odbyła; przy zmianie daty okno samo ustawia odpowiedni status), niezmieniona data z minionym terminem = do wyboru „Zaplanowana” albo „Odbyta”. Okno wyznaczania rozmowy otwiera `ConversationDialog.OpenAsync` (pulpit, „Moi pracownicy”, Etap 2). Wiersze list z etapem (przełożony i HR): `EvaluationService.GetEvaluationRowsAsync` (pracownik, bieżący etap, `EvaluationTask`). Rozmowę można wyznaczyć tylko dla oceny w oknie 2 miesięcy, w toku lub po terminie, i z zatwierdzonym arkuszem (`EvaluationTask.CanScheduleConversation`); bez arkusza `NeedsSheet`, a pulpit zamiast „Wyznacz termin” pokazuje „Zatwierdź arkusz” prowadzące na stronę pracownika.
  - Przypomnień i powiadomień w aplikacji nie ma i nie dodajemy ich (zob. „Proces oceny okresowej”). Informacje przekazują kalendarze.
  - Terminy liczy obecnie `Evaluations/EvaluationSchedule.cs` według cyklu rocznego (do zmiany: termin ma wpisywać HR, zob. proces): pierwsza ocena rok po `EmployedSince`, kolejna rok po terminie ostatniej zakończonej. Tam też podział na: po terminie, w toku (status oceny od `InProgress`), do rozpoczęcia (w ciągu 60 dni), zaplanowane. Arkusz domyślny: B dla przełożonych, C dla pozostałych.
  - Dane pulpitu (zadania i rozmowy do kalendarza): `Evaluations/EvaluationService.cs`. Teksty statusów i daty po polsku: `Evaluations/EvaluationText.cs`.
- Dane demo (16 osób, 16 powiązań, 14 ocen, OSP dla Ewy i jej podwładnych poza Pawłem Kozłowskim i Łukaszem Grabowskim) są w `HasData` w `AppDbContext`. Podwładni Ewy Wiśniewskiej (konto `przelozony`) pokrywają przypadki pulpitu: Łukasz Grabowski ma rozmowę z minionym terminem (14.11.2026, nieoznaczona jako odbyta), Jan Nowak ocenę po terminie, a Karolina Mazur termin oceny za mniej niż 2 miesiące, oboje bez Etapu 0 (na pulpicie „Zatwierdź arkusz”), Anna (27.11.2026) i Michał (18.12.2026) zaplanowane rozmowy, Agnieszka i Paweł dalsze terminy (Paweł: pierwsza ocena), na Etapie 2 „Ocena pracownika” są Natalia Krawczyk (termin oceny 3.04.2027, rozmowę oceniającą wyznaczy się od 2.02.2027) i Bartosz Wróbel (termin oceny 17.01.2027, już w oknie rozmowy, bez wyznaczonej rozmowy: „Wyznacz termin” w „Moi pracownicy” i na pulpicie); oboje mają po dwie notatki z Etapu 2; Monika Sikora jest na Etapie 3 „Zakończenie” (rozmowa oceniająca odbyta 16.11.2026, ocena na piśmie sporządzona 19.11.2026, termin oceny 28.11.2026, dwie notatki z Etapu 2 tylko do odczytu). Zakończone oceny mają rozmowy w historii kalendarza. Daty demo są ustawione pod 21.11.2026.
- Kultura aplikacji to `pl-PL` (`UseRequestLocalization` w `Program.cs`), więc komponenty Radzen (kalendarz, wybór daty) mają polskie nazwy miesięcy i tydzień od poniedziałku.
