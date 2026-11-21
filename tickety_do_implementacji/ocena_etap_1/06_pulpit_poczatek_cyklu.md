# 06. Pulpit przełożonego na początku cyklu

Decyzje: D4, D9. Wymaga ticketów 01, 02 i 05.

## Cel

Pracownik pojawia się na pulpicie przełożonego sam na początku cyklu (CLAUDE.md, „Przebieg w aplikacji”, pkt 2), a rozmowy oceniającej nie da się wyznaczyć bez zatwierdzonej części I.

## Zakres

### `EvaluationTask` i `EvaluationSchedule.Describe`

- Nowe dane: `CycleStart`, `StartingConversationDeadline`, `IsPartIConfirmed`, rozmowa rozpoczynająca (data, status).
- `NeedsStart`: część I niezatwierdzona i brak zaplanowanej przyszłej rozmowy rozpoczynającej. Osoba wymaga Etapu 0 albo I niezależnie od tego, czy 30 dni już minęło; na liście pokazujemy ją z terminem rozmowy rozpoczynającej.
- `CanScheduleConversation` (oceniająca): dodatkowo `IsPartIConfirmed` (D9). `NeedsSheet` zastąpić przez `NeedsStart`, bo „Zatwierdź arkusz” i „Ustal kryteria” prowadzą w to samo miejsce.
- `NeedsAction` uwzględnia `NeedsStart`.

### Lista „Do zrobienia” (`SupervisorOverview.ToDo`, `EvaluationTaskItem`)

Kolejność:
1. rozmowy obu typów z minionym terminem, nieoznaczone jako odbyte (bez zmian, ticket 04 dodał rozmowy rozpoczynające),
2. osoby na początku cyklu bez zatwierdzonej części I: termin po prawej to **termin rozmowy rozpoczynającej** (opis względny, po terminie na czerwono); przycisk zależnie od stanu:
   - bez arkusza: „Zatwierdź arkusz”,
   - z arkuszem, bez rozmowy rozpoczynającej: „Wyznacz termin” (otwiera okno rozmowy rozpoczynającej),
   - rozmowa odbyta, część I niezatwierdzona: „Zatwierdź część I”,
   - wszystkie przyciski poza „Wyznacz termin” prowadzą na stronę pracownika,
3. osoby z terminem oceny w ciągu 2 miesięcy (albo po terminie) z zatwierdzoną częścią I i bez rozmowy oceniającej: „Wyznacz termin” (bez zmian).

Zasada z design.md bez zmian: status tylko, gdy niesie informację („Po terminie”), resztę mówi termin i przycisk. Rozmowy zaplanowane na przyszłość nie trafiają na listę, są w kalendarzu.

### Dane demo

Sprawdzić, że lista Ewy Wiśniewskiej pokazuje: Pawła (początek cyklu, „Zatwierdź arkusz”), osobę w trakcie Etapu I (nie ma jej na liście, bo rozmowa zaplanowana w przyszłości; jest w kalendarzu), Jana Nowaka i Karolinę Mazur (termin oceny blisko, bez arkusza, „Zatwierdź arkusz”, po Etapie 0 przejdą przez Etap I).

### CLAUDE.md

Zaktualizować opis listy „Do zrobienia” i usunąć z listy różnic punkt „Pulpit pokazuje pracownika tylko w oknie 2 miesięcy przed terminem oceny”.

## Gotowe, gdy

- Paweł Kozłowski jest na liście „Do zrobienia” z terminem rozmowy rozpoczynającej i przyciskiem „Zatwierdź arkusz”.
- Po zatwierdzeniu arkusza Pawła przycisk zmienia się na „Wyznacz termin” i otwiera okno rozmowy rozpoczynającej.
- Osoba z terminem oceny w ciągu 2 miesięcy, z arkuszem, bez części I nie ma przycisku wyznaczenia rozmowy oceniającej, tylko przejście do Etapu I.
- Dotychczasowe przypadki rozmów oceniających (Łukasz, Anna, Michał) wyglądają jak przed zmianą.
