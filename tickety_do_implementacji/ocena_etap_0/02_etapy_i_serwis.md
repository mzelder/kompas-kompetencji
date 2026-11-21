# 02. Etapy oceny i zatwierdzanie arkusza w serwisie

Decyzje: D1, D3, D5. Wymaga ticketu 01.

## Cel

Jedno miejsce w kodzie wie, na którym etapie jest ocena pracownika, a serwis pozwala zatwierdzić arkusz (zakończyć Etap 0).

## Zakres

### `Evaluations/EvaluationStage.cs`

- Enum etapów w kolejności z `CLAUDE.md`: `JobDescription` (Etap 0), `StartingConversation` (I), `Assessment` (II), `EvaluationConversation` (III), `WrittenEvaluation` (IV), `Completion` (V).
- Statyczna metoda wyliczająca bieżący etap z otwartej oceny (`Evaluation?`). W tym tickecie dwie reguły: brak oceny albo `SheetConfirmedAt == null` → `JobDescription`, w przeciwnym razie `StartingConversation`. Kolejne etapy dopiszą tu swoje warunki.
- Polskie nazwy etapów do paska kroków w `EvaluationText`: „Opis stanowiska”, „Rozmowa rozpoczynająca”, „Ocenianie”, „Rozmowa oceniająca”, „Ocena na piśmie”, „Zakończenie”.

### Podpowiedź arkusza

- Obok `EvaluationSchedule.SheetFor` metoda zwracająca uzasadnienie podpowiedzi po polsku, np. „Pracownik kieruje zespołem, dlatego podpowiadamy arkusz B.” / „Pracownik nie kieruje zespołem, dlatego podpowiadamy arkusz C.”. Opisy arkuszy B i C (dla kogo są) też w `EvaluationText`.

### `EvaluationService`

- Wydzielić z `SaveConversationAsync` zakładanie otwartej oceny (`NotStarted`, `Sheet`, okres, `DueDate`) do prywatnej metody używanej przez obie operacje.
- `GetEmployeeEvaluationAsync(supervisorId, employeeId, today)`: sprawdza, że pracownik jest w zespole przełożonego (jak w `SaveConversationAsync`); zwraca rekord z: pracownikiem, OSP (`JobDescription?`), `EvaluationTask` z `EvaluationSchedule.Describe`, otwartą oceną (`Sheet`, `SheetConfirmedAt`), bieżącym etapem i podpowiedzią arkusza. `null`, gdy pracownika nie ma w zespole.
- `ConfirmSheetAsync(supervisorId, employeeId, sheet, now)`: sprawdza zespół, zakłada otwartą ocenę, jeśli jej nie ma, ustawia `Sheet` i `SheetConfirmedAt = now`. Odrzuca (`OperationResult.Fail`) ponowne zatwierdzenie, gdy Etap 0 jest już zakończony (D4: bez cofania).

## Gotowe, gdy

- `dotnet build` przechodzi.
- `SaveConversationAsync` działa jak wcześniej (wspólne zakładanie oceny nie zmienia jego zachowania).
- `CLAUDE.md` (sekcja Oceny) wymienia `EvaluationStage` i nowe metody serwisu.
