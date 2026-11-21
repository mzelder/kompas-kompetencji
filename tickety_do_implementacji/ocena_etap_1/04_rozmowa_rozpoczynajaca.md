# 04. Rozmowa rozpoczynająca: termin, status, kalendarz

Decyzje: D3, D4. Wymaga ticketów 01–03.

## Cel

Przełożony wyznacza termin rozmowy rozpoczynającej ze strony pracownika, zmienia go i oznacza rozmowę jako odbytą. Rozmowa jest widoczna w kalendarzu obok rozmów oceniających.

## Zakres

### Karta rozmowy w `StartingConversationStage`

Nad dwiema kolumnami z ticketu 03 (pełna szerokość) `RadzenCard Variant.Outlined` z nagłówkiem karty z akcją (design.md: „Nagłówek karty z akcją”):

- tytuł „Rozmowa rozpoczynająca”, podpis: termin rozmowy albo „Termin rozmowy nie jest wyznaczony”,
- obok: „Termin: do <data> (za X dni)” z `StartingConversationRules.Deadline` i `EvaluationText.RelativeToToday`; po terminie opis na czerwono (`rz-color-danger`), jak na liście zadań,
- przycisk: „Wyznacz termin” (`ButtonStyle.Primary`) albo „Zmień termin” (`ButtonStyle.Secondary`); gdy rozmowa jest zaplanowana, a jej data minęła, dodatkowo „Oznacz jako odbytą” jednym kliknięciem (wzór: lista „Do zrobienia” na pulpicie),
- „termin minął” (data minęła, rozmowa nieodbyta): `RadzenAlert` Warning z następnym krokiem, jak w `ConversationDialog`,
- po rozmowie odbytej: `RadzenBadge` „Odbyta” i data; przycisk „Zmień termin” zostaje do zatwierdzenia części I.

### Okno rozmowy

- `ConversationDialog` dostaje parametr typu rozmowy. Tytuł okna i teksty: „Termin rozmowy rozpoczynającej” / „Termin rozmowy oceniającej”.
- Dla `Starting`: `Min`/`Max` i podpowiedź pod polem z `StartingConversationRules` („Rozmowa rozpoczynająca odbywa się w ciągu 30 dni od początku cyklu, czyli do <data>.”; po terminie: „Termin minął <data>. Wyznacz rozmowę jak najszybciej.”), proponowana data z `Suggest`.
- Zapis przez `SaveConversationAsync(..., ConversationType.Starting, ...)`; po zapisie strona przeładowuje dane.

### Kalendarz przełożonego

- `GetSupervisorOverviewAsync` zwraca rozmowy obu typów; pozycja kalendarza ma typ.
- Tytuł i szablon wydarzenia: „<Imię Nazwisko> · rozmowa rozpoczynająca” / „· rozmowa oceniająca” (dziś „rozmowa okresowa”).
- Wygląd statusów bez zmian (zaplanowana, odbyta, termin minął). Kliknięcie otwiera okno właściwego typu.
- Lista „Do zrobienia”: rozmowy rozpoczynające z minionym terminem, nieoznaczone jako odbyte, trafiają tam tak samo jak oceniające (przyciski „Oznacz jako odbytą” i „Zmień termin”).

## Gotowe, gdy

- Dla osoby w Etapie I da się wyznaczyć termin w ciągu 30 dni; data po terminie jest odrzucana przez kontrolkę i przez serwis.
- Dla osoby, której 30 dni już minęło, kontrolka pozwala na dowolną przyszłą datę, a strona pokazuje termin na czerwono.
- Rozmowa pojawia się w kalendarzu z nazwą typu; zmiana terminu wraca status do „Zaplanowana”.
- Rozmowa z minionym terminem jest na liście „Do zrobienia” i da się ją oznaczyć jako odbytą jednym kliknięciem.
- Rozmowy oceniające działają jak przed zmianą.
