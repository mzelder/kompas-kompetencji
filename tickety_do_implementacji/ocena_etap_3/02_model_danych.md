# 02. Model danych Etapu 3

Decyzje: D3, D4. Wymaga gotowej części II (D1) tylko w danych demo.

## Cel

Ocena przechowuje datę sporządzenia i datę zapoznania pracownika z oceną. Koniec okresu oceny przestaje być wyliczany z terminu oceny.

## Zakres

### `Data/Evaluation.cs`

- `DateOnly? PreparedOn`: data sporządzenia oceny (podpis oceniającego w części II). Ustawiana i poprawiana w Etapie 3 do zamknięcia.
- `DateOnly? PartIIAcquaintedOn`: data zapoznania pracownika z oceną (podpis ocenianego w części II). Ustawiana przy zamknięciu.
- `PeriodEnd` zmienia się na `DateOnly?`: pusty do zamknięcia, przy zamknięciu = `PreparedOn`. W UI okres w toku pokazujemy jako „od <PeriodStart>”.
- `CompletedAt` zostaje: moment kliknięcia „Zamknij ocenę”. `Status = Completed` jak dziś.
- `EvaluationService.CreateOpenEvaluation` przestaje ustawiać `PeriodEnd`.

### Migracja

Usunąć obecną migrację i wygenerować jedną `Initial` (zob. `CLAUDE.md`).

### Dane demo

- Zakończone oceny: `PreparedOn` = dzień `CompletedAt` albo wcześniejszy (po rozmowie oceniającej), `PartIIAcquaintedOn` ≥ `PreparedOn`, `PeriodEnd` = `PreparedOn`.
- Monika Sikora (ocena 15, Etap 3): bez `PreparedOn` i bez `PartIIAcquaintedOn`, żeby na demo przejść oba kroki. Jej dane części II (D1) muszą być kompletne, żeby PDF miał treść.
- Otwarte oceny: `PeriodEnd = null`.

## Gotowe, gdy

- Aplikacja startuje z nową migracją, dane demo się wczytują.
- Wszędzie, gdzie dziś czytany jest `PeriodEnd`, kod obsługuje pusty koniec okresu (kompilacja bez ostrzeżeń o null).
