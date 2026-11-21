# 02. Reguły Etapu I i metody serwisu

Decyzje: D1, D2, D4, D5, D7, D9. Wymaga ticketu 01.

## Cel

Logika Etapu I w jednym miejscu: kiedy zaczyna się cykl, do kiedy trzeba odbyć rozmowę rozpoczynającą, jaki jest bieżący etap, jakie kryteria dodatkowe podpowiada OSP, oraz zapis wersji roboczej, rozmowy i zatwierdzenia części I.

## Zakres

### Początek cyklu i okno 30 dni

- `EvaluationSchedule.CycleStart(employee, evaluations)`: `CompletedAt` ostatniej zakończonej oceny (data) albo `EmployedSince`.
- `StartingConversationRules` (nowy plik `Evaluations/StartingConversationRules.cs`, wzór: `ConversationRules`):
  - `const int DaysFromCycleStart = 30`,
  - `Deadline(cycleStart)` = `cycleStart + 30 dni`,
  - `IsOverdue(cycleStart, now)`,
  - `LatestAllowed(cycleStart, now)`: koniec dnia terminu albo `null` po terminie (jak najszybciej, bez górnej granicy),
  - `Suggest(cycleStart, now)`: najbliższy dzień roboczy za tydzień, nie później niż termin; po terminie jutro o 10:00,
  - `Validate(newAt, currentAt, status, cycleStart, now)`: data najpóźniej w dniu terminu (jeśli jeszcze nie minął), reguły statusu jak w `ConversationRules` (przyszła data = tylko „Zaplanowana”, przeszła = „Odbyta”, niezmieniona miniona data = do wyboru).
- Wspólne reguły statusu (`StatusForNewDate`, `CanBeScheduled`, `CanBeHeld`, `IsPastDue`) zostają w `ConversationRules` i są używane przez oba typy. Bez kopiowania.

### Bieżący etap

`EvaluationStages.Current(openEvaluation)`:
- brak arkusza → `JobDescriptionReview`,
- brak `PartIConfirmedAt` → `StartingConversation`,
- w pozostałych przypadkach → `Assessment` (kolejne etapy dopiszą swoje warunki w kolejnych ticketach).

Dodatkowo `StartingConversationStep` (enum albo właściwość na widoku) dla UI: `Preparation` (brak rozmowy albo rozmowa zaplanowana), `ReadyToConfirm` (rozmowa odbyta, część I niezatwierdzona). Wylicza się z danych, bez zapisu.

### Podpowiedź kryteriów dodatkowych

`Evaluations/CriteriaSuggestion.cs`:
- stała tabela przypisania nazw kompetencji z OSP do kryteriów, porównanie przez `TextNormalizer` (bez wielkości liter i polskich znaków). Minimum dla danych demo:
  - „Myślenie analityczne” → Myślenie analityczne,
  - „Orientacja na cele”, „Organizacja pracy własnej” → Organizacja pracy i orientacja na osiąganie celów,
  - „Komunikacja”, „Komunikacja pisemna” → Komunikacja,
  - „Współpraca” → Współpraca, „Rzetelność” → Rzetelność,
  - „Zarządzanie zespołem”, „Zarządzanie rozwojem pracowników” → Zarządzanie ludźmi,
  - „Podejmowanie decyzji” → Podejmowanie decyzji i odpowiedzialność,
  - „Myślenie strategiczne” → Zarządzanie strategiczne,
  - nazwa identyczna z nazwą kryterium zawsze pasuje;
- `Suggest(sheet, jobDescription)` zwraca do 3 kryteriów spoza obowiązkowych, w kolejności z OSP, każde z kompetencją z OSP, z której wynika (do wyświetlenia „z OSP: Myślenie strategiczne”);
- brak OSP albo brak dopasowań → pusta lista (0 kryteriów dodatkowych jest zgodne z przepisami).

### Serwis (`EvaluationService`)

Każda metoda sprawdza, że pracownik jest w zespole przełożonego (`FindSubordinateAsync`) i ma otwartą ocenę z zatwierdzonym arkuszem; po zatwierdzeniu części I zmiany odrzuca („Część I arkusza jest już zatwierdzona.”).

- `GetEmployeeEvaluationAsync`: widok rozszerzony o: początek cyklu, termin rozmowy rozpoczynającej, rozmowę rozpoczynającą, kryteria obowiązkowe, wybrane kryteria dodatkowe, podpowiedź kryteriów, oczekiwania, cele, datę zapoznania, krok Etapu I. Jeśli ocena nie ma jeszcze żadnej wersji roboczej, wybrane kryteria dodatkowe = podpowiedź (D2: przełożony zaczyna od propozycji, nie od zera).
- `SavePartIDraftAsync(supervisorId, employeeId, additionalCriteria, expectations, goals)`: walidacja: najwyżej 3, bez obowiązkowych, bez powtórzeń; zapis bez zatwierdzania.
- `SaveConversationAsync(..., ConversationType type, ...)`: jedna metoda dla obu typów. `Starting`: wymaga Etapu 0, walidacja `StartingConversationRules`. `Assessment`: dotychczasowe warunki plus zatwierdzona część I (D9, komunikat „Najpierw zatwierdź część I arkusza na stronie pracownika.”).
- `ConfirmPartIAsync(supervisorId, employeeId, additionalCriteria, expectations, goals, acquaintedOn, now)`: zapisuje ostatnią wersję i zatwierdza. Warunki: rozmowa rozpoczynająca odbyta; `Expectations` niepuste; `acquaintedOn` ≥ data rozmowy i ≤ dziś. Ustawia `PartIConfirmedAt = now`, `EmployeeAcquaintedOn`, `PeriodStart = acquaintedOn`, `Status = CriteriaSet`.
- `CreateOpenEvaluation`: `PeriodStart` tymczasowo = początek cyklu (ostateczna wartość przy zatwierdzeniu części I).

### Teksty (`EvaluationText`)

- `StageDescription(StartingConversation)`: „Przygotuj propozycję kryteriów, odbądź rozmowę rozpoczynającą w ciągu 30 dni od początku cyklu, a po rozmowie zatwierdź część I arkusza.”
- Nazwa typu rozmowy: „rozmowa rozpoczynająca”, „rozmowa oceniająca” (do kalendarza i okien).

## Gotowe, gdy

- Build przechodzi; strona pracownika z danymi demo pokazuje etapy zgodnie z nową regułą (Anna, Michał, Łukasz na „Ocenianie”, Paweł na „Opis stanowiska”, osoba w trakcie Etapu I na „Rozmowa rozpoczynająca”).
- Podpowiedź dla każdego OSP z danych demo daje sensowne kryteria (np. Ewa Wiśniewska, arkusz B: Zarządzanie strategiczne; Anna Kowalska, arkusz C: Myślenie analityczne).
- Próba wyznaczenia rozmowy oceniającej bez zatwierdzonej części I kończy się komunikatem z serwisu.
