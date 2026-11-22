# 03. Reguły i serwis Etapu 3

Decyzje: D2, D3, D4, D5. Wymaga ticketów 01 i 02.

## Cel

Jawne zasady dat Etapu 3, zapis daty sporządzenia, zamknięcie oceny i rozpoczęcie nowego cyklu od daty sporządzenia.

## Zakres

### `Evaluations/CompletionRules.cs`

- Krok Etapu 3 (`CompletionStep`): `Preparation` (brak `PreparedOn`), `AwaitingAcquaintance` (`PreparedOn` zapisana, ocena otwarta). Jak `StartingConversationRules.Step`.
- `PreparedOnRange(evaluation, today)`: `Min` = data odbytej rozmowy oceniającej, `Max` = dziś. Bez górnej granicy w postaci terminu oceny.
- `AcquaintedOnRange(evaluation, today)`: `Min` = `PreparedOn`, `Max` = dziś.
- `ValidatePreparedOn`, `ValidateAcquaintedOn`: komunikaty z zakresem dat, np. „Data sporządzenia oceny musi przypadać między <data rozmowy oceniającej> a dziś.”
- Warunek zamknięcia: rozmowa oceniająca odbyta, `WrittenEvaluationAt` ustawione, `PreparedOn` zapisana. Komunikat przy braku odbytej rozmowy: „Oznacz rozmowę oceniającą jako odbytą przed zamknięciem oceny.”

### `Evaluations/EvaluationSchedule.cs`

- `CycleStart`: od `PreparedOn` ostatniej zakończonej oceny (zamiast `CompletedAt`), bez poprzedniej oceny od `EmployedSince`.
- `NextDueDate` z ticketu 01 korzysta z tego początku cyklu, więc kolejny termin = `PreparedOn` + 24 miesiące.

### `Evaluations/EvaluationService.cs`

- `SavePreparedOnAsync(employeeId, supervisorId, preparedOn, ct)`: tylko pracownik z zespołu, otwarta ocena na Etapie 3 (`EvaluationStages.Current == Completion`), walidacja z `CompletionRules`. Można wywołać ponownie do zamknięcia.
- `CloseEvaluationAsync(employeeId, supervisorId, acquaintedOn, ct)`: te same sprawdzenia oraz warunek zamknięcia; ustawia `PartIIAcquaintedOn`, `PeriodEnd = PreparedOn`, `CompletedAt = now`, `Status = Completed`. Nowej oceny nie zakłada: pracownik bez otwartej oceny jest na Etapie 0, a termin wynika z harmonogramu (jak dziś po zakończeniu).
- `EmployeeEvaluationView` dostaje dane Etapu 3: `PreparedOn`, `PartIIAcquaintedOn`, `CompletionStep`, zakresy dat oraz `PreviousEvaluation` (id, arkusz, data sporządzenia) ostatniej zakończonej oceny, do linku PDF z ticketu 05.
- `IsConversationEditable`: bez zmian (zamknięta ocena nie pozwala zmieniać rozmów).

## Gotowe, gdy

- Dla Moniki Sikory serwis przyjmuje datę sporządzenia z zakresu i odrzuca datę sprzed rozmowy oceniającej albo z przyszłości.
- Zamknięcie z datą zapoznania sprzed daty sporządzenia jest odrzucane; po poprawnym zamknięciu Monika jest na Etapie 0, a jej termin oceny to data sporządzenia + 24 miesiące.
- Termin rozmowy rozpoczynającej nowego cyklu to data sporządzenia + 30 dni.
