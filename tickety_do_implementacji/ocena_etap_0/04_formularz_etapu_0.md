# 04. Formularz Etapu 0: opis stanowiska i wybór arkusza

Decyzje: D2, D3. Wymaga ticketów 02 i 03 (można robić razem z 03).

## Cel

Przełożony przegląda opis stanowiska pracownika i zatwierdza arkusz oceny B albo C, z podpowiedzią systemu.

## Zakres

Komponent `Components/Pages/Przelozony/Stages/JobDescriptionStage.razor`, parametry: dane z `GetEmployeeEvaluationAsync`, tryb tylko do odczytu, callback po zatwierdzeniu.

### Układ

`RadzenRow` z dwiema kolumnami `RadzenColumn Size="12" SizeMD="6"`, każda w `RadzenCard Variant.Outlined`:

- **Opis stanowiska** (tytuł karty H6, podpis: stanowisko i komórka):
  - cel stanowiska (`RadzenText Body1`),
  - „Główne zadania”: lista,
  - „Wymagane kompetencje”: `RadzenBadge Variant.Flat Shade.Lighter`,
  - liczba podległych osób (`DetailItem`), jeśli jest.
  - Brak OSP: `RadzenAlert` (Info) „Opis stanowiska nie został jeszcze uzupełniony. Arkusz możesz wybrać na podstawie stanowiska.” Zatwierdzenie arkusza nadal możliwe.
- **Arkusz oceny**:
  - `RadzenRadioButtonList` pionowo z opcjami „Arkusz B” i „Arkusz C”, każda z krótkim opisem, dla kogo jest (z `EvaluationText`); przy podpowiedzianej opcji dopisek „(podpowiedź)”,
  - pod listą uzasadnienie podpowiedzi (`rz-color-base-300`),
  - domyślnie zaznaczona podpowiedź; zmiana bez uzasadnienia (D3),
  - przycisk „Zatwierdź arkusz” (`ButtonStyle.Primary`, `IsBusy`, `BusyText="Zapisywanie..."`), wyrównany do prawej.
- Wzór wyboru opcji: `Przelozony/ConversationDialog.razor`.

### Zachowanie

- Zatwierdzenie wywołuje `ConfirmSheetAsync`; sukces: `NotificationService.Notify` („Arkusz zatwierdzony”) i callback, po którym strona przeładowuje dane i zaznacza następny krok; błąd: `RadzenAlert` (Danger) nad przyciskiem z treścią z `OperationResult`.
- Tryb tylko do odczytu (etap zakończony): zamiast listy wyboru `DetailItem` „Arkusz” i „Zatwierdzono” (data po polsku), bez przycisku.

## Gotowe, gdy

- Dla pracownika bez Etapu 0 podpowiedź jest zgodna z `IsSupervisor`, zatwierdzenie zapisuje arkusz i przenosi na krok „Rozmowa rozpoczynająca”; po F5 stan się utrzymuje.
- Zmiana podpowiedzi (np. C → B) zapisuje się bez pytania o uzasadnienie.
- Pracownik bez OSP pokazuje komunikat i nadal da się zatwierdzić arkusz.
- Ponowne wejście w krok „Opis stanowiska” pokazuje dane tylko do odczytu.
