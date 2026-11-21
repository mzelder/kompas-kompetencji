# 05. Zatwierdzenie części I i potwierdzenie zapoznania

Decyzje: D1, D2, D7, D8. Wymaga ticketów 01–04.

## Cel

Po odbytej rozmowie przełożony uzupełnia część I tym, co ustalono na rozmowie (ta sama wersja robocza), wpisuje datę zapoznania pracownika i zatwierdza część I. To kończy Etap I i rozpoczyna okres oceny.

## Zakres

### Karta „Część I arkusza” po rozmowie

W kroku `ReadyToConfirm` (rozmowa odbyta, część I niezatwierdzona) prawa kolumna z ticketu 03 zmienia się:

- podpis karty: „Uzupełnij ustalenia z rozmowy i zatwierdź”,
- `RadzenAlert` Info na górze karty: „Rozmowa odbyła się <data>. Sprawdź kryteria, oczekiwania i cele i popraw je zgodnie z ustaleniami.”,
- te same kontrolki co w przygotowaniu, z zapisaną propozycją (nie od zera),
- nowe pole „Data zapoznania pracownika z częścią I”: `RadzenDatePicker TValue="DateOnly?"` bez godziny, `Min` = data rozmowy, `Max` = dziś, domyślnie data rozmowy; pomoc pod polem: „Data podpisu pracownika na części I. Od tego dnia liczy się okres oceny.”,
- przycisk „Zatwierdź część I” (`ButtonStyle.Primary`, `IsBusy`) obok „Zapisz propozycję”,
- zatwierdzenie przez `DialogService.Confirm`: „Po zatwierdzeniu kryteriów nie można zmienić. Pracownik potwierdził rozmowę i zapoznanie z kryteriami oraz terminem oceny?”,
- walidacja w formularzu i w serwisie: puste oczekiwania → „Wpisz oczekiwania omówione z pracownikiem.”

W kroku `Preparation` przycisk „Zatwierdź część I” jest niewidoczny, a pod przyciskiem „Zapisz propozycję” jest wyjaśnienie (`rz-color-base-300`): „Część I zatwierdzisz po rozmowie rozpoczynającej.”

### Po zatwierdzeniu

- `NotificationService.Notify` „Część I zatwierdzona”, strona przeładowuje dane i przechodzi na krok „Ocenianie”.
- Krok „Rozmowa rozpoczynająca” tylko do odczytu: karta rozmowy (data, „Odbyta”, bez przycisków), część I jako `DetailItem` i listy: arkusz, kryteria obowiązkowe, kryteria dodatkowe (albo „Brak kryteriów dodatkowych”), oczekiwania, cele (jeśli są), termin oceny, „Zapoznanie pracownika: <data>”, „Okres oceny od: <data>”.
- Lewa kolumna „Do omówienia” w trybie tylko do odczytu jest ukryta (nie ma już czego przygotowywać).

### CLAUDE.md

- W „Etapy cyklu oceny” zamienić „Data rozmowy rozpoczynającej to początek okresu oceny” na „Data zapoznania pracownika z częścią I to początek okresu oceny”.
- Z listy różnic usunąć punkty o braku rozmowy rozpoczynającej i formularza Etapu I oraz o `PeriodStart` liczonym z terminu oceny (koniec okresu nadal do zmiany w Etapie V).

## Gotowe, gdy

- Bez odbytej rozmowy nie da się zatwierdzić części I (brak przycisku; serwis też odrzuca).
- Po rozmowie formularz zawiera propozycję sprzed rozmowy; po zmianach i zatwierdzeniu strona pokazuje krok „Ocenianie”.
- Data zapoznania spoza zakresu jest odrzucana; po zatwierdzeniu `PeriodStart` = data zapoznania (widać „Okres oceny od”).
- Ponowne wejście w krok „Rozmowa rozpoczynająca” pokazuje część I tylko do odczytu; po F5 stan się utrzymuje.
