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

## Ustalenia od organizatorów

- **Ocena okresowa odbywa się co roku.** Pierwsza ocena okresowa jest po roku pracy, a nie na początku zatrudnienia. Cykl roczny służy jako domyślna podpowiedź terminu; sam termin oceny wpisuje HR (zob. proces niżej).
- Dotyczy to członków korpusu służby cywilnej. Ustawa o służbie cywilnej, rozporządzenia i poradniki KPRM mogą podawać inny okres (np. co 24 miesiące). **Na potrzeby tego projektu domyślnie przyjmujemy cykl roczny**, a każdą informację o innym okresie, z dowolnego źródła, traktujemy jako mniej istotną od tego ustalenia.

## Proces oceny okresowej (ustalenia z rozmów o wymaganiach)

Ten proces jest nadrzędny przy budowie kolejnych funkcji. Gdy obecny kod się z nim nie zgadza, kierujemy się procesem.

1. **HR wpisuje termin oceny ręcznie.** Po zatrudnieniu pracownika HR wprowadza datę, kiedy ma się odbyć ocena okresowa. Termin nie jest wyliczany automatycznie; cykl roczny (pierwsza po roku pracy, potem co rok) może jedynie podpowiadać datę w formularzu HR.
2. **Na miesiąc lub dwa przed terminem przełożony dowiaduje się, że ma przeprowadzić rozmowę.** HR kontaktuje się z przełożonym bezpośrednio, poza aplikacją; w aplikacji **nie ma przycisku ani akcji HR „przekaż przełożonemu”**. Pracownik pojawia się na **pulpicie przełożonego** sam, gdy do terminu oceny zostaje mniej niż ok. 2 miesiące, z jasną informacją, z kim przełożony ma odbyć rozmowę i że powinien wyznaczyć jej termin.
3. **Przełożony wyznacza termin rozmowy.** Termin oceny zna z kalendarza i dopasowuje do niego rozmowę: **rozmowa odbywa się najpóźniej w dniu oceny** (walidacja: data rozmowy ≤ termin oceny). Przełożony może później **zmienić termin rozmowy** (z tą samą walidacją).
4. **Rozmowa ma status wybierany przez przełożonego**, dokładnie dwa statusy: **Zaplanowana** (przełożony wyznaczył termin; obejmuje też termin uzgodniony z pracownikiem, osobnego statusu akceptacji nie ma) → **Odbyta**. Zmiana terminu rozmowy wraca status do „Zaplanowana”. **„Termin minął” nie jest statusem**, tylko dodatkową informacją wyliczaną automatycznie, gdy data rozmowy minęła, a rozmowa nie jest oznaczona jako odbyta (podpowiada: oznacz jako odbytą albo zmień termin). Zasada nadrzędna: aplikacja ma być jak najprostsza w użyciu, więc nie dokładamy statusów, które aplikacja może wyliczyć sama.
5. **Ocenę sporządza przełożony z pomocą asystenta AI.** AI tylko wspiera, decyzja o poziomie zawsze należy do przełożonego (zob. Konwencje).
6. **Po ocenie informacja trafia do HR**: że rozmowa się odbyła i ocena została sporządzona.
7. **Bez powiadomień.** Ani HR, ani przełożony nie dostają powiadomień, maili ani przypomnień w aplikacji. Cała komunikacja odbywa się przez **kalendarze**: HR i przełożeni widzą w swoich kalendarzach terminy ocen, terminy rozmów i ich statusy oraz informację o sporządzonej ocenie.

Wynikające z tego zasady dla aplikacji:
- Dwa różne terminy: **termin oceny** (ustala HR) i **termin rozmowy** (ustala przełożony, nie później niż termin oceny). W UI zawsze rozróżniamy je nazwą.
- Kalendarz jest głównym miejscem informacji: przełożony widzi terminy ocen swoich podwładnych i swoje rozmowy, HR widzi terminy ocen, rozmowy i sporządzone oceny wszystkich pracowników.
- Lista „Do zrobienia” na pulpicie przełożonego (`SupervisorOverview.ToDo`) pokazuje: **najpierw rozmowy z minionym terminem, nieoznaczone jako odbyte** (przyciski „Oznacz jako odbytą” jednym kliknięciem i „Zmień termin”), **potem osoby z terminem oceny w ciągu 2 miesięcy (albo już po terminie) bez wyznaczonej rozmowy** (przycisk „Wyznacz termin”). Dalsze terminy ocen i rozmowy zaplanowane na przyszłość albo odbyte nie trafiają na listę; są w kalendarzu (tam też „termin minął” jako pomarańczowa ramka, a kliknięcie otwiera edycję).

Różnice względem obecnej implementacji (do dostosowania przy kolejnych zmianach):
- Termin oceny jest dziś liczony automatycznie z `EmployedSince` w `EvaluationSchedule`; ma być wpisywany przez HR (np. pole na ocenie lub osobny rekord tworzony przez HR), a wyliczenie roczne zostaje tylko jako podpowiedź.
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
- Zakładki przełożonego: Pulpit (`/przelozony`, strona startowa), Moi pracownicy (`/przelozony/pracownicy`), Oceny (`/przelozony/oceny`), Model kompetencji (`/przelozony/model-kompetencji`), Zasady i materiały (`/przelozony/zasady`). Oceny, Model kompetencji i Zasady są na razie puste.
- Pozycja menu, której adres jest prefiksem innych (`/przelozony`, `/hr`), ma `NavLinkMatch.All`, inaczej podświetla się razem z podstronami.

### Dane
- `Data/AppDbContext.cs`, encje w `Data/`, migracje w `Data/Migrations/`. Do bazy z komponentów przez `IDbContextFactory<AppDbContext>` (krótko żyjący kontekst na operację) albo przez serwis.
- Model organizacji:
  - `Employee`: każda osoba w urzędzie (imię, nazwisko, stanowisko, komórka organizacyjna, data zatrudnienia `EmployedSince`). Flaga `IsSupervisor` oznacza, że może być czyimś przełożonym. Przełożony też jest pracownikiem i ma własnych przełożonych.
  - `EmployeeSupervisor`: powiązanie wiele-do-wielu pracownik → przełożony (klucz złożony, check constraint „nie można być swoim przełożonym”). Pracownik może mieć kilku przełożonych.
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
  - `Evaluation`: ocena okresowa pracownika (oceniający, arkusz B/C, okres, termin oceny `DueDate`, status oceny: `NotStarted` → `CriteriaSet` → `InProgress` → `ReadyForConversation` → `Completed`) oraz rozmowa: `ConversationAt` i `ConversationStatus` (`Scheduled` → `Held`). „Termin minął” wylicza `ConversationRules.IsPastDue`. Rekord powstaje najpóźniej przy wyznaczeniu rozmowy (wtedy ze statusem `NotStarted`); bez rekordu ocena jest „nierozpoczęta”, a termin wynika z harmonogramu.
  - Wyznaczanie i zmiana rozmowy: `EvaluationService.SaveConversationAsync` (sprawdza, że pracownik jest w zespole przełożonego, i zakłada ocenę `NotStarted`, jeśli jej nie ma). Zasady w `Evaluations/ConversationRules.cs`: rozmowa najpóźniej w dniu oceny (po terminie oceny: jak najszybciej, bez górnej granicy), status wynika z daty: data przyszła = tylko „Zaplanowana”, data z przeszłości = „Odbyta” (można od razu zapisać rozmowę, która już się odbyła; przy zmianie daty okno samo ustawia odpowiedni status), niezmieniona data z minionym terminem = do wyboru „Zaplanowana” albo „Odbyta”. Rozmowę można wyznaczyć tylko dla oceny w oknie 2 miesięcy, w toku lub po terminie (`EvaluationTask.CanScheduleConversation`).
  - Przypomnień i powiadomień w aplikacji nie ma i nie dodajemy ich (zob. „Proces oceny okresowej”). Informacje przekazują kalendarze.
  - Terminy liczy obecnie `Evaluations/EvaluationSchedule.cs` według cyklu rocznego (do zmiany: termin ma wpisywać HR, zob. proces): pierwsza ocena rok po `EmployedSince`, kolejna rok po terminie ostatniej zakończonej. Tam też podział na: po terminie, w toku, do rozpoczęcia (w ciągu 60 dni), zaplanowane. Arkusz domyślny: B dla przełożonych, C dla pozostałych.
  - Dane pulpitu (zadania i rozmowy do kalendarza): `Evaluations/EvaluationService.cs`. Teksty statusów i daty po polsku: `Evaluations/EvaluationText.cs`.
- Dane demo (13 osób, 13 powiązań, 8 ocen) są w `HasData` w `AppDbContext`. Podwładni Ewy Wiśniewskiej (konto `przelozony`) pokrywają przypadki pulpitu: Łukasz Grabowski ma rozmowę z minionym terminem (14.11.2026, nieoznaczona jako odbyta), Jan Nowak ocenę po terminie bez rozmowy, Karolina Mazur termin oceny za mniej niż 2 miesiące bez rozmowy, Anna (27.11.2026) i Michał (18.12.2026) zaplanowane rozmowy, Agnieszka i Paweł dalsze terminy (Paweł: pierwsza ocena). Zakończone oceny mają rozmowy w historii kalendarza. Daty demo są ustawione pod 21.11.2026.
- Kultura aplikacji to `pl-PL` (`UseRequestLocalization` w `Program.cs`), więc komponenty Radzen (kalendarz, wybór daty) mają polskie nazwy miesięcy i tydzień od poniedziałku.
