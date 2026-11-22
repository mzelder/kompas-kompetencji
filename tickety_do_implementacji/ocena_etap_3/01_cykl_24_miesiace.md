# 01. Cykl oceny 24 miesiące

Decyzja: D2. Niezależny od pozostałych ticketów.

## Cel

Kolejna ocena wypada 24 miesiące po początku cyklu: po zatrudnieniu (pierwsza ocena) albo po dacie sporządzenia poprzedniej oceny. Dotychczasowe ustalenie o cyklu rocznym przestaje obowiązywać.

## Zakres

### Harmonogram (`Evaluations/EvaluationSchedule.cs`)

- Stała `CycleMonths = 24`.
- `NextDueDate` liczy termin od początku cyklu, a nie od terminu poprzedniej oceny: `CycleStart(employee, evaluations).AddMonths(CycleMonths)`. Pierwsza ocena: `EmployedSince.AddMonths(CycleMonths)`.
- `CycleStart` bez zmian w tym tickecie (nadal `CompletedAt`); ticket 03 zmienia go na datę sporządzenia.
- Okno rozmowy oceniającej (`DueSoonDays = 60`) i okno 30 dni rozmowy rozpoczynającej bez zmian.

### Dane demo (`Data/AppDbContext.cs`, `HasData`)

Przypadki pulpitu z `CLAUDE.md` (stan na 21.11.2026) mają zostać takie same: te same etapy, te same terminy ocen i rozmów otwartych ocen. Zmieniają się tylko daty zakończonych ocen, tak żeby otwarte terminy wynikały z cyklu 24 miesięcy:

- każda zakończona ocena przesunięta o rok wstecz (okres, `DueDate`, rozmowy w historii kalendarza, `CompletedAt`), tak by `CompletedAt` + 24 miesiące ≈ termin otwartej oceny,
- osoby bez poprzedniej oceny (pierwsza ocena, np. Paweł Kozłowski): `EmployedSince` przesunięte o rok wstecz,
- sprawdzić, że Jan Nowak nadal ma ocenę po terminie, Karolina Mazur termin w ciągu 2 miesięcy, Bartosz Wróbel jest w oknie rozmowy oceniającej, a Natalia Krawczyk przed nim.

### Teksty i dokumentacja

- `Przelozony/ProcessGuide.razor`: „Ocena odbywa się co 24 miesiące. Pierwsza jest po 24 miesiącach pracy, a jej termin wpisuje HR.”
- Okno dodawania pracownika (HR) pokazuje termin z harmonogramu, więc zmienia się samo; sprawdzić podpis pod polem, jeśli wspomina rok.
- Prompt systemowy asystenta (`Components/Assistant/AssistantChat.razor`) i baza wiedzy: sprawdzić, czy nie podają cyklu rocznego jako zasady projektu (synonimy w `Synonyms.cs` już kierują „jak często” na „24 miesiące”).
- `CLAUDE.md`, sekcja „Ustalenia od organizatorów”: zastąpić oba punkty o cyklu rocznym zdaniem „Ocena okresowa odbywa się co 24 miesiące. Pierwsza ocena jest po 24 miesiącach od zatrudnienia, kolejna 24 miesiące po dacie sporządzenia poprzedniej. Wyliczenie służy jako podpowiedź terminu; sam termin wpisuje HR.” Poprawić też: zdanie o artefakcie „Luki…” („artefakt zakłada cykl 24 miesiące” przestaje być różnicą), punkt 1 przebiegu („cykl roczny … może jedynie podpowiadać”), listę różnic („wyliczenie roczne”) i opis `EvaluationSchedule` w „Architektura”.

## Gotowe, gdy

- Dla nowego pracownika okno HR podpowiada termin oceny 24 miesiące po zatrudnieniu.
- Pulpit, kalendarz i „Moi pracownicy” Ewy Wiśniewskiej pokazują te same przypadki co przed zmianą.
- W `CLAUDE.md` i w UI nie ma już cyklu rocznego.
