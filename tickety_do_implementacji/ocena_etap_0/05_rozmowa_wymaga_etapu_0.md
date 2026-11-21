# 05. Wyznaczenie rozmowy wymaga zakończonego Etapu 0

Decyzja: D6. Wymaga ticketów 02 i 03.

## Cel

Rozmowy nie można wyznaczyć, dopóki przełożony nie zatwierdzi arkusza. Pulpit zamiast „Wyznacz termin” prowadzi wtedy do Etapu 0.

## Zakres

- `EvaluationTask`: nowa właściwość mówiąca, czy arkusz jest zatwierdzony (z `SheetConfirmedAt` otwartej oceny w `EvaluationSchedule.Describe`). `CanScheduleConversation` wymaga zatwierdzonego arkusza; osobna właściwość (np. `NeedsSheet`) dla pracownika w oknie 2 miesięcy bez Etapu 0.
- `EvaluationService.SaveConversationAsync`: bez zatwierdzonego arkusza zwraca `OperationResult.Fail("Najpierw zatwierdź arkusz oceny na stronie pracownika.")`, zanim cokolwiek zapisze. Nie zakłada już oceny przy wyznaczeniu rozmowy (ocena istnieje od Etapu 0).
- `EvaluationTaskItem.razor`: gdy pracownik potrzebuje arkusza, zamiast „Wyznacz termin” przycisk „Zatwierdź arkusz” (`ButtonStyle.Primary`, ikona np. `assignment`), który przechodzi na stronę pracownika. Szczegóły w drugiej linii pozycji wyjaśniają powód, np. „Przed wyznaczeniem rozmowy zatwierdź arkusz”.
- Pulpit: kafelek i lista „do zrobienia” nadal liczą takiego pracownika jako wymagającego działania.

## Gotowe, gdy

- Pracownik z danych demo w oknie 2 miesięcy bez Etapu 0 (ticket 01) ma na pulpicie przycisk „Zatwierdź arkusz”, a po zatwierdzeniu na jego stronie wraca na pulpit z przyciskiem „Wyznacz termin”.
- Rozmowy Anny i Michała (z zatwierdzonym arkuszem w seedzie) da się dalej zmieniać.
- `dotnet build` przechodzi.
- W `CLAUDE.md` usunięta różnica „Rozmowę można dziś wyznaczyć bez zakończonego Etapu 0”, a opis `SaveConversationAsync` i `EvaluationTask.CanScheduleConversation` zaktualizowany.
