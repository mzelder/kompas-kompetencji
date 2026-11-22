# 04. PDF arkusza oceny

Decyzje: D1, D6, D7, D8. Wymaga ticketów 02 i 03 oraz gotowej części II.

## Cel

Przełożony pobiera wypełniony arkusz B albo C jako PDF: do podpisu przy zapoznaniu pracownika z oceną i jako kopię po zamknięciu oceny.

## Zakres

### Biblioteka

- Pakiet `QuestPDF` w `KompasKompetencji.Web.csproj` (najnowsza wersja zgodna z .NET 11).
- `QuestPDF.Settings.License = LicenseType.Community;` w `Program.cs`.
- Sprawdzić polskie znaki w domyślnym foncie; w razie braków użyć fontu wbudowanego w QuestPDF (bez dodawania własnych plików fontów, jeśli nie trzeba).

### Dane i dokument (`Evaluations/Pdf/`)

- `EvaluationSheetData`: rekord z wszystkim, co trafia na arkusz, budowany przez `EvaluationService.GetEvaluationSheetAsync(evaluationId, supervisorId)` (ocena pracownika z zespołu przełożonego, otwarta na Etapie 3 z zapisaną datą sporządzenia albo zakończona; w przeciwnym razie `null`).
- `EvaluationSheetDocument : IDocument`: układ jak wzór arkusza (`tresci_zadania/pliki_dodatkowe/Ocena okresowa w służbie cywilnej/Arkusz oceny B (2).docx`, `… C (2).docx`), A4, tabele z ramkami.

| Sekcja arkusza | Źródło |
|---|---|
| Nagłówek „ARKUSZ B/C”, „OCENA OKRESOWA…” | `Evaluation.Sheet` |
| Nazwa urzędu | pusta linia |
| Dane ocenianego i oceniającego (imię, nazwisko, komórka, stanowisko) | `Employee`, `Evaluator` |
| Część I: kryteria obowiązkowe i dodatkowe | `EvaluationCriteria` dla arkusza, `AdditionalCriteria` |
| Część I: termin sporządzenia oceny | `DueDate` |
| Część I: potwierdzenia, daty | oceniający: `PartIConfirmedAt`, oceniany: `EmployeeAcquaintedOn`; miejscowość i podpisy puste |
| Część II: data rozmowy oceniającej | rozmowa `Assessment` |
| Część II: okres OD / DO | `PeriodStart` / `PreparedOn` |
| Oceny cząstkowe (X w kolumnie 1/3/5/7/9 pkt), średnia, poziom, pozytywna / negatywna, uzasadnienie, wnioski do IPRZ | model części II (D1); drukowana jest decyzja przełożonego, nie sugestia reguły |
| Dane poprzedniej oceny urzędnika SC, wniosek o kolejny stopień (TAK / NIE / NIE DOTYCZY) | puste do ręcznego wpisania |
| Dane oceniającego, data | `Evaluator`, `PreparedOn` |
| Potwierdzenie ocenianego, data | tekst z arkusza; `PartIIAcquaintedOn` albo pusta linia przed zamknięciem |
| Pouczenie o sprzeciwie | tekst z arkusza (art. 83 ust. 1, 7 dni) |
| Część III | brak |

### Endpoint

- `Evaluations/Pdf/EvaluationPdfEndpoints.cs` z `MapEvaluationPdfEndpoints()`, wywołanym w `Program.cs` obok `MapAuthEndpoints()`.
- `GET /przelozony/oceny/{evaluationId:int}/arkusz.pdf`, `RequireAuthorization` z rolą `AppRoles.Przelozony`; id przełożonego z konta (`ClaimsPrincipalExtensions`). Brak oceny albo cudza ocena: `404`.
- Odpowiedź `Results.File(bytes, "application/pdf", nazwa)`, nazwa pliku bez polskich znaków: `Arkusz_C_Sikora_Monika_2026-11-21.pdf` (nazwisko, imię, data sporządzenia).
- Statyczna metoda `EvaluationPdfEndpoints.SheetUrl(evaluationId)` dla przycisków w UI.

## Gotowe, gdy

- Dla Moniki Sikory po zapisaniu daty sporządzenia PDF się pobiera, ma arkusz C z częścią I i II, polskie znaki, pustą linię na datę zapoznania i pouczenie.
- Po zamknięciu oceny ten sam adres zwraca PDF z datą zapoznania.
- Inny przełożony (albo HR) dostaje odmowę / `404`.
- Dla oceny na arkuszu B nagłówek i kryteria obowiązkowe są z arkusza B.
