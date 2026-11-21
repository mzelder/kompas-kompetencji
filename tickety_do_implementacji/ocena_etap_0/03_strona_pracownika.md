# 03. Strona pracownika z paskiem kroków

Decyzja: D4. Wymaga ticketu 02.

## Cel

Cały przebieg oceny odbywa się na stronie konkretnego pracownika. Przełożony widzi, na którym etapie jest ocena, i pod paskiem kroków formularz bieżącego etapu.

## Zakres

### Strona `Components/Pages/Przelozony/EmployeeEvaluation.razor`

- Adres `/przelozony/pracownicy/{EmployeeId:int}`, w folderze przełożonego (autoryzacja z `_Imports.razor`). Bez pozycji w menu bocznym; `NavLinkMatch` dla „Moi pracownicy” sprawdzić, żeby pozycja była podświetlona także na stronie pracownika.
- `PageTitle`: „<Imię i nazwisko> – Kompas kompetencji”.
- `PageHeader`: tytuł imię i nazwisko, podtytuł stanowisko i komórka; w środku `StatTile Compact="true"` z terminem oceny (etykieta „Termin oceny”, wartość data i opis względny z `EvaluationText.RelativeToToday`).
- Przycisk powrotu do listy „Moi pracownicy” (`Variant.Text`, ikona `arrow_back`).
- `RadzenSteps` z `ShowStepsButtons="false"` i krokami z `EvaluationStage` (nazwy z `EvaluationText`):
  - etapy przed bieżącym: klikalne, pokazują dane tylko do odczytu,
  - etap bieżący: zaznaczony przy wejściu na stronę, pokazuje formularz,
  - etapy po bieżącym: `Disabled`.
- Treść kroku:
  - `JobDescription`: komponent z ticketu 04,
  - pozostałe etapy (jeszcze bez formularzy): bieżący etap `StatusMessage` z nazwą etapu i krótką informacją, co przełożony robi na tym etapie (bez wzmianek o braku funkcji).
- Pracownik spoza zespołu albo nieistniejący: `StatusMessage` „Nie znaleziono pracownika w Twoim zespole.” z przyciskiem powrotu.
- Błąd wczytania: `RadzenAlert` jak w `MyEmployees.razor`.
- Wzór układu: `Hr/Employees.razor` i `Przelozony/Dashboard.razor`. Sprawdzić zawijanie na węższym oknie.

### Wejścia na stronę

- `MyEmployees.razor`:
  - lista tylko podwładnych zalogowanego przełożonego (`SubordinatesOf` dla `AppUser.EmployeeId`, jak na pulpicie), zamiast wszystkich pracowników,
  - kliknięcie wiersza (`RowClick`) otwiera stronę pracownika.
- Pulpit: imię i nazwisko w `EvaluationTaskItem.razor` jako link do strony pracownika (`RadzenLink`), przyciski rozmowy bez zmian.

## Gotowe, gdy

- Z „Moi pracownicy” i z pulpitu można wejść na stronę każdego podwładnego; lista zawiera tylko podwładnych Ewy Wiśniewskiej (6 osób).
- Strona innego pracownika (np. `/przelozony/pracownicy/7`) pokazuje komunikat o braku pracownika w zespole.
- Pracownik bez Etapu 0 ma zaznaczony krok „Opis stanowiska”, pracownik z zatwierdzonym arkuszem (np. Anna Kowalska) krok „Rozmowa rozpoczynająca”, a krok „Opis stanowiska” da się otworzyć do odczytu.
- `CLAUDE.md` (Widoki per rola, lista różnic) i `design.md` (wzorzec „Kroki procesu” z przykładem w kodzie) zaktualizowane.
