# Tickety do implementacji: Etap I oceny (rozmowa rozpoczynająca i część I arkusza)

Cel: przełożony przygotowuje się do rozmowy rozpoczynającej (propozycja kryteriów dodatkowych z OSP, oczekiwania, cele), wyznacza i odbywa rozmowę, a po niej zatwierdza część I arkusza i potwierdza zapoznanie pracownika. Kryteria nie są ostatecznie ustalone przed rozmową ani wymyślane od zera po niej: ta sama wersja robocza przechodzi przez cały etap i zostaje zatwierdzona dopiero po rozmowie.

Proces i etapy: `CLAUDE.md`, sekcja „Proces oceny okresowej”. Wymagania z treści zadania: „pomagać w doborze właściwych kryteriów”, „lepiej przygotować się do rozmowy z pracownikiem”, „umożliwiać wypełnienie właściwej części formularza”.

## Podstawa w przepisach (`tresci_zadania/pliki_dodatkowe/`)

| Źródło | Co mówi |
|---|---|
| Rozporządzenie o ocenach okresowych, § 5 ust. 1 | Na rozmowie omawia się: główne obowiązki z OSP, kryteria obowiązkowe i dodatkowe, oczekiwania co do spełniania kryteriów, cele (jeśli uzasadnione charakterem stanowiska) i sposób ich realizacji |
| § 5 ust. 2 | Najwyżej 3 kryteria dodatkowe z wykazu 14, najistotniejsze ze względu na obowiązki z OSP |
| § 5 ust. 3 | **Po rozmowie** oceniający wpisuje do części I kryteria dodatkowe i termin sporządzenia oceny |
| § 6 | Zapoznanie ocenianego z kryteriami i terminem; podpis ocenianego potwierdza rozmowę i zapoznanie; kopia części I dla pracownika |
| § 7 | Czynności z § 5 i § 6 w ciągu 30 dni od sporządzenia poprzedniej oceny albo od zatrudnienia |
| Poradnik KPRM, rozdz. 6.2.1–6.2.2 | Przed rozmową zadbać o aktualny OSP; w trakcie rozmowy zastanowić się nad kryteriami dodatkowymi, korzystając z wymaganych kompetencji z OSP; oczekiwania warto zapisać, żeby odwołać się do nich na rozmowie oceniającej |
| Arkusze B i C, część I | Pola: kryteria obowiązkowe, kryteria dodatkowe (do 3), termin sporządzenia oceny; wspólne potwierdzenie „przeprowadzenie rozmowy (…)” i „zapoznanie ocenianego z kryteriami oceny oraz terminem sporządzenia oceny”, podpisy obu stron z datą |
| Arkusze B i C, część II, przypis do „OD” | Okres oceny zaczyna się w dniu **zapoznania ocenianego z kryteriami** (z części I) |

## Stan wyjściowy (2026-11-21)

- Etap 0 gotowy (`tickety_do_implementacji/ocena_etap_0/`): `Evaluation.SheetConfirmedAt`, strona pracownika `/przelozony/pracownicy/{id}` z `RadzenSteps`, komponent `Przelozony/Stages/JobDescriptionStage.razor`.
- `EvaluationStages.Current` zna tylko dwa etapy: bez arkusza `JobDescriptionReview`, z arkuszem `StartingConversation`. Krok „Rozmowa rozpoczynająca” pokazuje sam opis z `EvaluationText.StageDescription`.
- `Evaluation` ma jedną rozmowę (`ConversationAt`, `ConversationStatus`): rozmowę oceniającą (Etap III). Zasady w `Evaluations/ConversationRules.cs`, okno `Przelozony/ConversationDialog.razor`, kalendarz i lista „Do zrobienia” w `Przelozony/Dashboard.razor`.
- `PeriodStart` = termin oceny minus rok, ustawiane w `EvaluationService.CreateOpenEvaluation`.
- W kodzie nie ma wykazu 14 kryteriów oceny. Wymagane kompetencje w OSP (`JobDescription.RequiredCompetencies`) mają własne nazwy („Orientacja na cele”, „Komunikacja pisemna”, „Zarządzanie zespołem”), niezgodne z nazwami kryteriów.
- Brak biblioteki PDF.

## Podjęte decyzje

| # | Temat | Decyzja |
|---|---|---|
| D1 | Przebieg Etapu I | Trzy kroki w jednym etapie: **przygotowanie** (wersja robocza części I) → **rozmowa rozpoczynająca** (termin i status) → **zatwierdzenie części I** (po rozmowie odbytej, z datą zapoznania pracownika). Wersja robocza jest edytowalna przez cały etap, także po rozmowie; po zatwierdzeniu tylko do odczytu |
| D2 | Jedna wersja robocza, nie dwie | Propozycja przed rozmową i ostateczna treść po niej to te same pola. Zatwierdzenie tylko je zamyka (`PartIConfirmedAt`). Dzięki temu kryteria nie są wymyślane od zera po rozmowie |
| D3 | Rozmowy | Osobna encja `Conversation` z typem (`Starting`, `Assessment`), zgodnie z CLAUDE.md. Obecna rozmowa oceniająca przechodzi z `Evaluation` do tej encji bez zmiany zachowania. Statusy bez zmian: Zaplanowana → Odbyta, „termin minął” wyliczany |
| D4 | Termin rozmowy rozpoczynającej | W ciągu 30 dni od początku cyklu (zatrudnienie albo `CompletedAt` poprzedniej oceny). Po upływie 30 dni: jak najszybciej, bez górnej granicy (tak jak rozmowa oceniająca po terminie oceny), w UI informacja „po terminie”. Nie blokujemy |
| D5 | Kryteria dodatkowe | 0–3 z wykazu 14 (bez obowiązkowych dla arkusza). Podpowiedź liczy jawna reguła: wymagane kompetencje z OSP przypisane do kryteriów stałą tabelą nazw. Przełożony wybiera dowolnie, bez uzasadnienia. AI nie wybiera kryteriów |
| D6 | Oczekiwania i cele | Dwa pola tekstowe na całą ocenę: „Oczekiwania” (wymagane przy zatwierdzeniu) i „Cele” (opcjonalne, § 5 ust. 1 pkt 4: „jeżeli ich określenie jest uzasadnione”). Bez oczekiwań per kryterium, żeby formularz był krótki |
| D7 | Potwierdzenie zapoznania | Przełożony wpisuje datę, w której pracownik zapoznał się z częścią I (podpis na papierze). Data: nie wcześniej niż data rozmowy rozpoczynającej, nie później niż dziś. Ta data to `PeriodStart` (przypis do części II arkusza). CLAUDE.md do poprawy: „data rozmowy rozpoczynającej to początek okresu oceny” → „data zapoznania z częścią I” |
| D8 | Termin oceny w części I | Tylko do odczytu, z `DueDate` (dziś z harmonogramu, docelowo wpisuje HR). Etap I go nie zmienia |
| D9 | Rozmowa oceniająca a część I | Wyznaczenie rozmowy oceniającej wymaga zatwierdzonej części I (bez kryteriów nie ma czego oceniać). Rozmowa rozpoczynająca wymaga tylko Etapu 0 |
| D10 | Asystent AI | Pomaga przygotować rozmowę (tematy, pytania, sformułowanie oczekiwań i celów) w osobnym, ostatnim tickecie. Na stronie etapu stała lista tematów z § 5 działa bez AI |
| D11 | PDF części I | Poza zakresem. Powstanie razem z generowaniem dokumentu w Etapie V |

## Kolejność ticketów

1. [01_model_danych.md](01_model_danych.md): encja `Conversation`, pola części I, kryteria dodatkowe, wykaz kryteriów, migracja, dane demo
2. [02_reguly_i_serwis.md](02_reguly_i_serwis.md): początek cyklu, okno 30 dni, bieżący etap, podpowiedź kryteriów, metody serwisu
3. [03_przygotowanie_do_rozmowy.md](03_przygotowanie_do_rozmowy.md): formularz wersji roboczej części I na stronie pracownika
4. [04_rozmowa_rozpoczynajaca.md](04_rozmowa_rozpoczynajaca.md): wyznaczanie, zmiana i status rozmowy rozpoczynającej (strona pracownika, okno rozmowy, kalendarz)
5. [05_zatwierdzenie_czesci_I.md](05_zatwierdzenie_czesci_I.md): zatwierdzenie części I i potwierdzenie zapoznania, tryb tylko do odczytu
6. [06_pulpit_poczatek_cyklu.md](06_pulpit_poczatek_cyklu.md): pulpit przełożonego na początku cyklu i blokada rozmowy oceniającej bez części I
7. [07_asystent_przygotowanie_rozmowy.md](07_asystent_przygotowanie_rozmowy.md): asystent AI w przygotowaniu do rozmowy

Tickety 01 i 02 są podstawą reszty. 03, 04 i 05 dotyczą tego samego komponentu etapu i najlepiej robić je po kolei. 06 i 07 są niezależne od siebie.

## Stan realizacji

| Ticket | Stan | Uwagi |
|---|---|---|
| 01 | zrobione | Encje `Conversation` i `EvaluationAdditionalCriterion`, pola części I, `EvaluationCriteria`, jedna migracja `Initial`, dane demo (Paweł: początek cyklu, Agnieszka: w trakcie Etapu I). Serwis i widoki czytają tylko rozmowę `Assessment` |
| 02 | zrobione | `EvaluationSchedule.CycleStart`, `StartingConversationRules` (z krokiem Etapu I `Step`, dodatkowa wartość `Confirmed`), `CriteriaSuggestion`, `EvaluationCriteria.ValidateAdditional`, `ConversationRules.ValidateStatus` wspólne dla obu typów. Serwis: `SaveConversationAsync` z typem rozmowy (oceniająca wymaga części I), `SavePartIDraftAsync`, `ConfirmPartIAsync`, rozszerzony `EmployeeEvaluationView`. Wywołania w UI przekazują `ConversationType.Assessment`. Michał (OSP same kryteria obowiązkowe C) nie ma podpowiedzi kryteriów |
| 03 | zrobione | `Przelozony/Stages/StartingConversationStage` (karta „Część I arkusza”: kryteria obowiązkowe, `RadzenCheckBoxList` kryteriów dodatkowych z limitem 3 i podpowiedzią z OSP, oczekiwania, cele, termin oceny, „Zapisz propozycję”; tryb `ReadOnly` z listami i tekstami) i `StartingConversationTopics` (lewa kolumna „Do omówienia na rozmowie”, ukryta w `ReadOnly`). `EvaluationService.MaxPartITextLength` publiczne (limit pól tekstowych w UI), klasa `.pre-line` w `app.css` dla wielowierszowych tekstów w trybie tylko do odczytu |
| 04 | zrobione | Karta „Rozmowa rozpoczynająca” nad kolumnami Etapu I (termin z 30 dni, „Wyznacz/Zmień termin”, „Oznacz jako odbytą”, alert „termin minął”, badge „Odbyta”; termin z 30 dni ukryty po odbytej rozmowie, przyciski ukryte w `ReadOnly`). `ConversationDialog` z parametrem `Type` i statycznym `OpenAsync` (`Min` bez zmian, bo data z przeszłości zapisuje odbytą rozmowę). `EvaluationTask` ma już dane rozmowy rozpoczynającej, `CycleStart`, `IsPartIConfirmed` i `PastDueConversation(now)`; kalendarz i „Do zrobienia” obsługują oba typy. W 06 zostaje: `NeedsStart`, `CanScheduleConversation` z `IsPartIConfirmed` (dziś osoba po Etapie 0 bez części I z terminem oceny w oknie 2 miesięcy, np. Jan Nowak po zatwierdzeniu arkusza, dostaje „Wyznacz termin” rozmowy oceniającej, który serwis odrzuca) |
| 05 | zrobione | W `StartingConversationStage` krok `ReadyToConfirm`: podpis karty „Uzupełnij ustalenia z rozmowy i zatwierdź”, alert Info z datą rozmowy na górze karty, pole „Data zapoznania pracownika z częścią I” (`DateOnly?`, domyślnie dzień rozmowy, `Min`/`Max`), „Zatwierdź część I” z `DialogService.Confirm` i osobną flagą zajętości; walidacja oczekiwań i daty także w formularzu (pusta lub odrzucona przez kontrolkę data daje komunikat z zakresem). W `Preparation` podpis „Część I zatwierdzisz po rozmowie rozpoczynającej.”. Po zatwierdzeniu `OnChanged` przeładowuje stronę, która sama przechodzi na „Ocenianie” (`selectedIndex` = bieżący etap). Tryb tylko do odczytu dopisuje arkusz, „Zapoznanie pracownika” i „Okres oceny od” |
| 06 | zrobione | `EvaluationTask.NeedsStart` (część I niezatwierdzona i brak zaplanowanej rozmowy rozpoczynającej) zastąpił `NeedsSheet`; `CanScheduleConversation` wymaga `IsPartIConfirmed`. „Do zrobienia”: rozmowy z minionym terminem, potem `NeedsStart` (sortowane po terminie rozmowy rozpoczynającej, po prawej ten termin; „Zatwierdź arkusz” / „Wyznacz termin” otwierający okno rozmowy rozpoczynającej / „Zatwierdź część I”), potem rozmowy oceniające do wyznaczenia (zdanie mówi teraz „rozmowy oceniającej”). Podpis karty i pusty stan („Nie ma zadań do wykonania.”) zaktualizowane. Jan Nowak i Karolina Mazur mają termin rozmowy rozpoczynającej liczony od `CompletedAt` poprzedniej oceny (2025), więc widać go jako dawno po terminie. Kompilacja bez błędów; pełny build, `dotnet test` i sprawdzenie w aplikacji czekały na zwolnienie `bin` przez sesję debugowania Visual Studio |
| 07 | do zrobienia | |

## Zasady obowiązujące we wszystkich ticketach

- Bez komentarzy w kodzie (C#, Razor, CSS). Kod komponentu w `Nazwa.razor.cs`.
- Bez nowych testów automatycznych. Sprawdzenie ręczne w aplikacji według sekcji „Gotowe, gdy”.
- Kod i nazwy po angielsku, teksty w UI po polsku. Dane pracowników i OSP fikcyjne.
- UI według `design.md`: najpierw komponenty Radzen, wzorce z istniejących widoków, motyw `standard-dark` bez nadpisywania.
- W UI zawsze nazwany termin: „termin oceny”, „termin rozmowy rozpoczynającej”, „termin rozmowy oceniającej”. Bez informacji o prototypie i brakujących funkcjach.
- Bez powiadomień i przypomnień (CLAUDE.md). Informacje przekazują pulpit i kalendarz.
- Baza jest tworzona od nowa przy starcie, a migracje można w każdej chwili usunąć i wygenerować od nowa. Bez migracji przenoszących dane i bez dbania o historię migracji; liczy się aktualny model i `HasData`.
- Po każdym uruchomieniu aplikacji do testów: zatrzymać ją (aplikacja blokuje pliki w `bin`).
- Po zakończeniu ticketu zaktualizować `CLAUDE.md` (architektura, lista różnic w „Proces oceny okresowej”) i `design.md` (nowe wzorce), jeśli ticket je zmienia, oraz tabelę „Stan realizacji” powyżej.
