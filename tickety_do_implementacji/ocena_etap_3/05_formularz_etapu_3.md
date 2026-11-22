# 05. Formularz Etapu 3 na stronie pracownika

Decyzje: D3, D4, D5, D8, D9. Wymaga ticketów 02 i 03; przycisk PDF z ticketu 04.

## Cel

Krok „Zakończenie” na stronie pracownika prowadzi przełożonego przez dwa kroki: data sporządzenia i PDF do podpisu, potem data zapoznania i zamknięcie oceny. Po zamknięciu pracownik jest w nowym cyklu, a PDF poprzedniej oceny zostaje do pobrania.

## Zakres

### `Przelozony/Stages/CompletionStage.razor` + `.razor.cs`

Parametry jak w `AssessmentStage`: `View`, `SupervisorId`, `ReadOnly`, `OnChanged`. W `EmployeeEvaluation.razor` zastępuje opis w gałęzi `else`.

Karta „Sporządzenie oceny” (`RadzenCard Outlined`, nagłówek H6 z podpisem):

- podpis w kroku `Preparation`: „Wpisz datę sporządzenia oceny i pobierz arkusz do podpisu.”, w kroku `AwaitingAcquaintance`: „Ocena sporządzona <data>.”,
- `RadzenFormField` „Data sporządzenia oceny”: `RadzenDatePicker TValue="DateOnly?"` bez godziny, `Min`/`Max` z `CompletionRules`, domyślnie dziś; pomoc w `<Helper>`: „Data podpisu oceniającego. Kończy okres oceny i rozpoczyna nowy cykl. Nie wcześniej niż rozmowa oceniająca (<data>).”,
- „Zapisz datę sporządzenia” (`ButtonStyle.Primary` w `Preparation`, `Secondary` po zapisaniu, `IsBusy`),
- po zapisaniu: „Pobierz PDF” (`ButtonStyle.Primary`, ikona `picture_as_pdf`) jako link do `EvaluationPdfEndpoints.SheetUrl(...)` (pełne pobranie, nie nawigacja Blazora),
- obok `DetailItem`: „Okres oceny: <PeriodStart> – <PreparedOn>”, „Kolejna ocena: <PreparedOn + 24 miesiące>” (podpowiedź z harmonogramu).

Karta „Zapoznanie pracownika z oceną” (widoczna w `AwaitingAcquaintance`):

- podpis: „Po podpisaniu arkusza przez pracownika wpisz datę i zamknij ocenę.”,
- `RadzenDatePicker TValue="DateOnly?"` „Data zapoznania pracownika z oceną”, `Min` = data sporządzenia, `Max` = dziś, domyślnie dziś; pomoc: „Data podpisu pracownika na części II.”,
- „Zamknij ocenę” (`ButtonStyle.Primary`, `IsBusy`) z `DialogService.Confirm`: „Po zamknięciu oceny nie można jej zmienić, a pracownik zaczyna nowy cykl oceny. Pracownik podpisał arkusz?”,
- przy nieodbytej rozmowie oceniającej przycisk wyłączony, pod nim komunikat z `CompletionRules`.

Walidacja dat także w formularzu (pusta albo odrzucona przez kontrolkę data daje komunikat z zakresem), jak w `StartingConversationStage` (ticket 05 Etapu 1).

Po zamknięciu: `NotificationService.Notify` „Ocena zamknięta”, `OnChanged` przeładowuje stronę, która sama przechodzi na Etap 0 nowego cyklu.

### Strona pracownika po zamknięciu (`EmployeeEvaluation.razor`)

- Pod `PageHeader`, gdy `View.PreviousEvaluation` istnieje: jeden wiersz z ikoną `history`, tekstem (Body2, `rz-color-base-300`) „Poprzednia ocena sporządzona <data>” i małym przyciskiem „Pobierz PDF” (`Variant.Text`). Bez osobnej karty ani zakładki.
- Pasek kroków pokazuje nowy cykl (Etap 0), więc zamkniętego Etapu 3 nie ma w trybie tylko do odczytu.

### „Moi pracownicy” (`MyEmployees.razor.cs`, `NextStepFor`)

- Etap 3, `Preparation`: „Zakończenie” / „Wpisz datę sporządzenia oceny”.
- Etap 3, `AwaitingAcquaintance`: „Zakończenie” / „Zapoznaj pracownika z oceną, sporządzona <data>”.
- Bez przycisku (akcje są na stronie pracownika, kliknięcie wiersza ją otwiera). Wymaga `CompletionStep` w `EmployeeEvaluationRow` albo `EvaluationTask`.

### Teksty i dokumentacja

- `EvaluationText.StageDescription(Completion)`: „Wpisz datę sporządzenia oceny, wręcz pracownikowi arkusz do podpisu i zamknij ocenę. Od daty sporządzenia zaczyna się nowy cykl.”
- `CLAUDE.md`: z listy różnic usunąć punkty o `PeriodEnd` i o Etapie 3 bez formularza; w „Architektura” opisać `CompletionStage`, `CompletionRules`, `PreparedOn`, `PartIIAcquaintedOn`, endpoint PDF; zaktualizować opis danych demo (Monika Sikora w kroku „Sporządzenie”).
- `design.md`: wzorzec „Pobranie pliku (PDF)”: przycisk jako link do endpointu, ikona `picture_as_pdf`.

## Gotowe, gdy

- Monika Sikora: zapis daty sporządzenia → pojawia się „Pobierz PDF” i karta zapoznania; zmiana daty sporządzenia przed zamknięciem działa.
- Data spoza zakresu daje komunikat z zakresem; F5 zachowuje stan.
- Po „Zamknij ocenę” strona pokazuje Etap 0, termin oceny = data sporządzenia + 24 miesiące, a wiersz „Poprzednia ocena” pobiera PDF z obiema datami.
- „Moi pracownicy” pokazuje właściwy następny krok w obu krokach Etapu 3, a po zamknięciu „Zatwierdź arkusz”.
