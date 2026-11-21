# 01. Model danych: rozmowy z typem, część I arkusza, wykaz kryteriów

Decyzje: D2, D3, D5, D6, D7.

## Cel

Baza przechowuje obie rozmowy oceny, wersję roboczą i zatwierdzenie części I arkusza. Kod zna wykaz 14 kryteriów oceny i kryteria obowiązkowe arkuszy B i C.

## Zakres

### Wykaz kryteriów: `Evaluations/EvaluationCriteria.cs`

- `enum Criterion` z 14 wartościami w kolejności z załącznika nr 1 do rozporządzenia: `Communication`, `Creativity`, `Negotiation`, `WorkOrganization`, `CustomerOrientation`, `DecisionMaking`, `CrisisManagement`, `Reliability`, `AnalyticalThinking`, `KnowledgeAndDevelopment`, `Cooperation`, `PublicSpeaking`, `PeopleManagement`, `StrategicManagement`.
- `static class EvaluationCriteria`:
  - `Name(Criterion)`: polska nazwa dokładnie jak w rozporządzeniu („Organizacja pracy i orientacja na osiąganie celów”, „Orientacja na klienta/interesanta” itd.),
  - `Mandatory(EvaluationSheet)`: B: zarządzanie ludźmi, podejmowanie decyzji i odpowiedzialność, organizacja pracy…, współpraca, komunikacja; C: wykorzystywanie wiedzy i doskonalenie zawodowe, rzetelność, organizacja pracy…, współpraca, komunikacja (§ 4 ust. 2 i 3),
  - `AvailableAdditional(EvaluationSheet)`: wykaz bez obowiązkowych dla arkusza,
  - `const int MaxAdditional = 3`.

### Rozmowy: `Data/Conversation.cs`

- `enum ConversationType { Starting, Assessment }`.
- `Conversation`: `Id`, `EvaluationId` + nawigacja, `Type`, `At` (`DateTime`), `Status` (`ConversationStatus`). Typ i status zapisywane jako tekst, jak `Evaluation.Status`.
- Unikalny indeks (`EvaluationId`, `Type`): jedna rozmowa każdego typu na ocenę.
- `Evaluation.Conversations` (kolekcja). Z `Evaluation` usunąć `ConversationAt` i `ConversationStatus`.
- Wszystkie miejsca, które dziś czytają `ConversationAt` / `ConversationStatus` (`EvaluationSchedule.Describe`, `EvaluationService.GetSupervisorOverviewAsync`, `SaveConversationAsync`, dane demo), przejść na rozmowę typu `Assessment`. Zachowanie pulpitu i kalendarza bez zmian.
- Uwaga na nazwę: w `EvaluationService.cs` jest już rekord `EvaluationConversation` (pozycja kalendarza). Encja nazywa się `Conversation`; rekord kalendarza można zostawić.

### Część I arkusza: pola na `Evaluation`

- `Expectations` (`string?`, max 4000): oczekiwania przełożonego co do spełniania kryteriów.
- `Goals` (`string?`, max 4000): cele na okres oceny i sposób ich realizacji.
- `PartIConfirmedAt` (`DateTime?`): zatwierdzenie części I przez przełożonego, koniec Etapu I.
- `EmployeeAcquaintedOn` (`DateOnly?`): data zapoznania pracownika z częścią I (data podpisu ocenianego).

### Kryteria dodatkowe: `Data/EvaluationAdditionalCriterion.cs`

- Tabela `EvaluationAdditionalCriteria`: klucz złożony (`EvaluationId`, `Criterion`), `Criterion` jako tekst. Kryteria obowiązkowe nie są zapisywane (wynikają z arkusza).
- `Evaluation.AdditionalCriteria` (kolekcja).

### Migracja i dane demo

- Migracja: dowolnie. Można dodać nową albo usunąć wszystkie z `Data/Migrations/` i wygenerować jedną od nowa (`dotnet ef migrations add Initial ...`). Historia migracji nie ma znaczenia.
- Dane demo (`HasData`), daty pod 21.11.2026. Po zmianie `EvaluationStages.Current` (ticket 02) oceny bez zatwierdzonej części I wrócą do Etapu I, więc:
  - Anna Kowalska, Michał Szymański, Łukasz Grabowski (oceny z rozmową oceniającą): rozmowa `Starting` odbyta rok wcześniej, 1–3 kryteria dodatkowe zgodne z ich OSP, krótkie oczekiwania, `PartIConfirmedAt` i `EmployeeAcquaintedOn` z tego samego okresu, `PeriodStart` = `EmployeeAcquaintedOn`,
  - zakończone oceny w historii: tak samo (część I zatwierdzona), żeby historia była spójna,
  - **nowy przypadek początku cyklu**: jeden podwładny Ewy Wiśniewskiej z początkiem cyklu w ostatnich 30 dniach. Propozycja: Paweł Kozłowski z `EmployedSince` = 2026-11-05 (pierwsza ocena), z OSP na podstawie OSP „starszy specjalista” z `OSP_word.zip` (fikcyjne dane), bez Etapu 0,
  - **przypadek w trakcie Etapu I**: jedna osoba z zatwierdzonym arkuszem, wersją roboczą (2 kryteria dodatkowe, oczekiwania) i zaplanowaną rozmową rozpoczynającą w przyszłości. Np. Agnieszka Woźniak po zmianie dat tak, żeby początek cyklu wypadał w ostatnich 30 dniach (zakończona poprzednia ocena z `CompletedAt` w listopadzie 2026).
- Opis przypadków demo w CLAUDE.md zaktualizować.

## Gotowe, gdy

- `dotnet build` przechodzi, aplikacja startuje, baza tworzy się z migracji.
- Pulpit i kalendarz Ewy Wiśniewskiej pokazują rozmowy oceniające tak jak przed zmianą (Łukasz: termin minął, Anna i Michał: zaplanowane, historia).
- W bazie są rozmowy `Starting` i kryteria dodatkowe dla ocen z danych demo.
