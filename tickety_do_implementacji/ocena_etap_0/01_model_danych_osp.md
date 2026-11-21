# 01. Model danych: opis stanowiska (OSP) i zatwierdzenie arkusza

Decyzje: D2, D5. Bez zależności.

## Cel

W bazie są skrócone opisy stanowisk pracowników demo, a ocena zapamiętuje, kiedy przełożony zatwierdził arkusz (koniec Etapu 0).

## Zakres

- Nowa encja `Data/JobDescription.cs`, tabela `JobDescriptions`, jedna na pracownika:
  - `EmployeeId` (klucz główny i obcy do `Employees`), nawigacja `Employee.JobDescription` (opcjonalna),
  - `Purpose`: cel istnienia stanowiska,
  - `Tasks`: główne zadania, jedno zadanie w linii,
  - `RequiredCompetencies`: wymagane kompetencje, jedna w linii (tekst, bo model kompetencji nie jest jeszcze zaimportowany),
  - `SubordinatesCount` (`int?`): liczba podległych osób, tylko dla stanowisk kierowniczych.
  - Listy zwracać jako `IReadOnlyList<string>` przez właściwości pomocnicze (podział po nowych liniach), żeby widoki nie dzieliły tekstu same.
- Nowe pole `Evaluation.SheetConfirmedAt` (`DateTime?`).
- Migracja `AddJobDescriptionsAndSheetConfirmation`.

## Dane demo (`HasData` w `AppDbContext`)

- OSP dla wszystkich podwładnych Ewy Wiśniewskiej (Id 1, 2, 9, 10, 11, 12) i dla samej Ewy (Id 3, arkusz B). Treść skrócona (cel, 4–6 zadań, 3–5 kompetencji), oparta na 3 przykładowych OSP z `OSP_word.zip` i dopasowana do stanowiska i komórki pracownika w seedzie. Bez prawdziwych nazwisk i danych z przykładów.
- Jeden pracownik bez OSP (np. Paweł Kozłowski, nowo zatrudniony), żeby demo pokazało pusty stan.
- `SheetConfirmedAt` ustawione we wszystkich istniejących ocenach (Id 1–7): zakończone oceny i oceny z wyznaczoną rozmową przeszły już Etap 0. Data kilka dni po początku okresu oceny.
- Sprawdzić, że po zmianie zostaje przynajmniej jeden podwładny Ewy w oknie 2 miesięcy przed terminem oceny bez rekordu oceny (czyli bez Etapu 0). Na nim pokażemy blokadę z ticketu 05. Jeśli takiego nie ma, przesunąć `EmployedSince` jednej osoby.

## Gotowe, gdy

- `dotnet build` przechodzi, migracja stosuje się przy starcie w Development.
- W bazie jest tabela `JobDescriptions` z danymi demo, a oceny 1–7 mają `SheetConfirmedAt`.
- `CLAUDE.md` (sekcja Dane i Oceny) opisuje `JobDescription` i `SheetConfirmedAt`.
