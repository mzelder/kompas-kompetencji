# Tickety do implementacji: Etap 0 oceny (opis stanowiska i wybór arkusza) i strona pracownika

Cel: przełożony wchodzi na stronę konkretnego pracownika, widzi wszystkie etapy oceny (pasek kroków) i formularz bieżącego etapu. Pierwszy zrealizowany etap to Etap 0: przegląd opisu stanowiska pracy (OSP) i zatwierdzenie arkusza B albo C. Bez zakończonego Etapu 0 nie można wyznaczyć rozmowy.

Proces i etapy: `CLAUDE.md`, sekcja „Proces oceny okresowej”, podsekcja „Etapy cyklu oceny” (etapy według artefaktu https://claude.ai/artifact/ULYd2HrnbtFT2dyK5vKiMt, cykl roczny według ustaleń od organizatorów).

## Stan wyjściowy (2026-11-21)

- `Data/Evaluation.cs`: ocena ma `Sheet` (B/C), `Status` (`NotStarted` → … → `Completed`, zapisywany jako tekst) i jedną rozmowę (`ConversationAt`, `ConversationStatus`), która odpowiada rozmowie oceniającej (Etap III).
- Arkusz podpowiada `EvaluationSchedule.SheetFor` (B, gdy `Employee.IsSupervisor`). Nikt go nie zatwierdza.
- Rekord oceny powstaje dopiero przy wyznaczeniu rozmowy (`EvaluationService.SaveConversationAsync`).
- Brak danych OSP w bazie. Przykładowe OSP: `tresci_zadania/pliki_dodatkowe/Kompetencje wspierające analizę OSP + przykładowe OSP/OSP_word.zip` (starszy specjalista, radca, naczelnik wydziału).
- Nie ma strony pracownika. `Przelozony/MyEmployees.razor` pokazuje wszystkich pracowników (`GetChartAsync().Employees`) zamiast podwładnych zalogowanego przełożonego.
- Testy (`tests/KompasKompetencji.Tests`) obejmują tylko asystenta AI.

## Podjęte decyzje

| # | Temat | Decyzja |
|---|---|---|
| D1 | Kiedy Etap 0 | Na początku cyklu (po zatrudnieniu albo po sporządzeniu poprzedniej oceny), razem z Etapem I w ciągu 30 dni. Etap 0 jest dostępny przez cały cykl, bez okna czasowego |
| D2 | Źródło OSP | Skrócone OSP w danych demo (`HasData`), na podstawie 3 przykładowych OSP, z fikcyjnymi danymi. Przełożony tylko przegląda, nie edytuje |
| D3 | Wybór arkusza | Podpowiedź z `IsSupervisor` z wyjaśnieniem reguły. Przełożony może zmienić B/C **bez uzasadnienia** |
| D4 | Strona pracownika | `/przelozony/pracownicy/{id}`: `RadzenSteps` ze wszystkimi etapami, pod nim formularz bieżącego etapu. Przyszłe etapy wyłączone, zakończone tylko do odczytu, bez cofania |
| D5 | Zakończenie Etapu 0 | Nowe pole `Evaluation.SheetConfirmedAt`, bez nowego statusu oceny. Bieżący etap wyliczany w jednym miejscu (`EvaluationStage`) |
| D6 | Rozmowy | Wyznaczenie którejkolwiek rozmowy wymaga zakończonego Etapu 0. Dzisiejsza rozmowa to rozmowa oceniająca; rozmowa rozpoczynająca powstanie w ticketach Etapu I |
| D7 | Zadanie „Ustal arkusz i kryteria” na pulpicie na początku cyklu | Poza tym zakresem, razem z Etapem I |

## Kolejność ticketów

1. [01_model_danych_osp.md](01_model_danych_osp.md): encja `JobDescription`, pole `SheetConfirmedAt`, migracja, dane demo
2. [02_etapy_i_serwis.md](02_etapy_i_serwis.md): `EvaluationStage`, zatwierdzanie arkusza w serwisie
3. [03_strona_pracownika.md](03_strona_pracownika.md): strona pracownika z paskiem kroków i wejścia na nią
4. [04_formularz_etapu_0.md](04_formularz_etapu_0.md): formularz OSP i wyboru arkusza
5. [05_rozmowa_wymaga_etapu_0.md](05_rozmowa_wymaga_etapu_0.md): blokada wyznaczenia rozmowy bez Etapu 0 (serwis i pulpit)

Tickety 03 i 04 można robić razem. Ticket 05 wymaga strony z ticketu 03 (przycisk na pulpicie do niej prowadzi).

## Stan realizacji

| Ticket | Stan | Uwagi |
|---|---|---|
| 01 | zrobiony (2026-11-21) | Migracja `AddJobDescriptionsAndSheetConfirmation`; bez Etapu 0 w demo: Jan Nowak i Karolina Mazur |
| 02 | zrobiony (2026-11-21) | Enum: `JobDescriptionReview`, `StartingConversation`, `Assessment`, `AssessmentConversation`, `WrittenEvaluation`, `Completion` |
| 03 | zrobiony (2026-11-21) | `StatTile.Value` zmieniony na tekst |
| 04 | zrobiony (2026-11-21) |  |
| 05 | zrobiony (2026-11-21) | Nowe `IsInConversationWindow`, `NeedsSheet`, `NeedsAction` (kolejność na pulpicie) |

## Zasady obowiązujące we wszystkich ticketach

- Bez komentarzy w kodzie (C#, Razor, CSS).
- Bez nowych testów automatycznych. Sprawdzenie ręczne w aplikacji według sekcji „Gotowe, gdy”.
- Kod i nazwy po angielsku, teksty w UI po polsku. Dane pracowników i OSP fikcyjne.
- UI według `design.md`: najpierw komponenty Radzen, wzorce z istniejących widoków, motyw `standard-dark` bez nadpisywania.
- W UI zawsze nazwany termin: „termin oceny”, „termin rozmowy oceniającej”. Bez informacji o prototypie i brakujących funkcjach.
- Po każdym uruchomieniu aplikacji do testów: zatrzymać ją (aplikacja blokuje pliki w `bin`).
- Po zakończeniu ticketu zaktualizować `CLAUDE.md` (architektura, lista różnic w „Proces oceny okresowej”) i `design.md` (nowe wzorce), jeśli ticket je zmienia, oraz tabelę „Stan realizacji” powyżej.
