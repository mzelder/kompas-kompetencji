# 03. Przygotowanie do rozmowy: wersja robocza części I

Decyzje: D1, D2, D5, D6. Wymaga ticketów 01 i 02.

## Cel

Przed rozmową przełożony widzi, co ma z pracownikiem omówić, i przygotowuje propozycję: kryteria dodatkowe (z podpowiedzią z OSP), oczekiwania i cele. Propozycję zapisuje i wraca do niej na rozmowie i po niej.

## Zakres

Komponent `Components/Pages/Przelozony/Stages/StartingConversationStage.razor` (+ `.razor.cs`), wzór: `JobDescriptionStage`. Parametry: widok z `GetEmployeeEvaluationAsync`, `SupervisorId`, `ReadOnly`, callback po zmianie. Montowany w `EmployeeEvaluation.razor` dla `EvaluationStage.StartingConversation` zamiast samego opisu.

Ten ticket buduje kartę przygotowania. Karty rozmowy (04) i zatwierdzenia (05) dochodzą w kolejnych ticketach do tego samego komponentu.

### Układ

`RadzenRow` z dwiema kolumnami (`Size="12" SizeMD="6"`):

- **Lewa kolumna: „Do omówienia na rozmowie”** (`RadzenCard Variant.Outlined`, nagłówek H6):
  - lista czterech tematów z § 5 ust. 1: główne obowiązki z opisu stanowiska, kryteria oceny, oczekiwania, cele (jeśli uzasadnione charakterem stanowiska),
  - pod spodem „Główne zadania z opisu stanowiska” (lista z `JobDescription.TaskList`) i „Wymagane kompetencje” (`RadzenBadge`), jak w `JobDescriptionStage`; brak OSP: `RadzenAlert` Info,
  - podpis źródła: „§ 5 rozporządzenia w sprawie ocen okresowych” (`rz-color-base-300`, Caption).
- **Prawa kolumna: „Część I arkusza”** (`RadzenCard Variant.Outlined`, podpis karty: „Propozycja do omówienia z pracownikiem” w kroku przygotowania):
  - „Kryteria obowiązkowe (arkusz B/C)”: lista tylko do odczytu,
  - „Kryteria dodatkowe (do 3)”: `RadzenCheckBoxList` pionowo z `AvailableAdditional`, `Template`: nazwa (Body1) i przy podpowiedzianych dopisek „podpowiedź z OSP: <kompetencja>” (Caption `rz-color-base-300`); po zaznaczeniu 3 pozostałe opcje wyłączone, pod listą wyjaśnienie „Możesz wybrać najwyżej 3 kryteria dodatkowe. Nie musisz wybierać żadnego.”,
  - „Oczekiwania”: `RadzenFormField Variant.Outlined` + `RadzenTextArea` (4–5 wierszy), pomoc pod polem: „Jak pracownik ma spełniać kryteria. Odwołasz się do tego na rozmowie oceniającej.”,
  - „Cele (opcjonalnie)”: `RadzenTextArea`, pomoc: „Wpisz, jeśli charakter stanowiska to uzasadnia, razem ze sposobem realizacji.”,
  - „Termin oceny”: `DetailItem`, tylko do odczytu (D8),
  - przycisk „Zapisz propozycję” (`ButtonStyle.Secondary`, `IsBusy`, `BusyText="Zapisywanie..."`) wyrównany do prawej.
- Wzorce: wybór z podpowiedzią jak w `JobDescriptionStage` (design.md: „Wybór opcji z opisem i podpowiedzią systemu”), formularz jak w `Login.razor`.

### Zachowanie

- Przy pierwszym wejściu zaznaczone są kryteria z podpowiedzi (ticket 02), a pola tekstowe puste.
- „Zapisz propozycję” → `SavePartIDraftAsync`; sukces: `NotificationService.Notify` „Propozycja zapisana”; błąd: `RadzenAlert` Danger nad przyciskiem.
- Niezapisane zmiany nie są blokowane przy wyjściu ze strony (prostota).
- `ReadOnly` (część I zatwierdzona): zamiast kontrolek listy i teksty, bez przycisku (wspólnie z ticketem 05).
- Nowy wzorzec „Wybór kilku opcji z limitem i podpowiedzią” dopisać do `design.md`.

## Gotowe, gdy

- Dla osoby w Etapie I z OSP widać tematy rozmowy, zadania i kompetencje z OSP oraz zaznaczoną podpowiedź kryteriów.
- Zmiana kryteriów i wpisanie oczekiwań oraz celów zapisuje się i zostaje po F5.
- Nie da się zaznaczyć czwartego kryterium dodatkowego ani kryterium obowiązkowego.
- Osoba bez OSP: komunikat, brak podpowiedzi, wybór kryteriów nadal możliwy.
