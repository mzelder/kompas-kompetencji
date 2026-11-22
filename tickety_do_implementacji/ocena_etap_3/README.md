# Tickety do implementacji: Etap 3 oceny (zakończenie i dokument)

Cel: po ocenie na piśmie (część II arkusza) przełożony wpisuje datę sporządzenia oceny, pobiera PDF arkusza do podpisu, a po zapoznaniu pracownika z oceną wpisuje datę zapoznania i zamyka ocenę. Od daty sporządzenia liczy się nowy cykl: pracownik wraca do Etapu 0, a kolejna ocena wypada 24 miesiące później. PDF zamkniętej oceny zostaje do pobrania na stronie pracownika.

Źródło: artefakt „Etapy oceny w aplikacji” (https://claude.ai/artifact/WeR8mC72JtwDJ3oHUGrqqA), sekcja „3. Zakończenie i dokument” i luka „Formularz zakończenia”. Proces: `CLAUDE.md`, sekcja „Proces oceny okresowej”. Wymagania z treści zadania: „eksport do PDF” (`podsumowanie_rozwiazania.md`), „generowanie PDF arkusza” (MUST w `mvp.md`).

## Podstawa w przepisach (`tresci_zadania/pliki_dodatkowe/`)

| Źródło | Co mówi |
|---|---|
| Ustawa, art. 81 ust. 1 | Ocena okresowa co 24 miesiące |
| Rozporządzenie, § 12 | Oceniający niezwłocznie zapoznaje ocenianego z oceną. Oryginał do akt osobowych, kopia dla ocenianego |
| Arkusze B i C, część II | Data rozmowy oceniającej; okres oceny OD (zapoznanie z częścią I) i DO; oceny cząstkowe; średnia; poziom; pozytywna / negatywna; uzasadnienie; wnioski do IPRZ; sekcje tylko dla urzędnika SC; dane i podpis oceniającego z datą; potwierdzenie ocenianego „Przeprowadzono ze mną rozmowę oceniającą oraz zapoznałem(-łam) się z oceną okresową i wnioskami dotyczącymi IPRZ” z datą i podpisem; pouczenie o sprzeciwie (art. 83 ust. 1, 7 dni) |
| `flow_oceny_okresowej.md`, Etap V | Podpis ocenianego nie musi być złożony tego samego dnia, ale bez zbędnej zwłoki. Nowy cykl zaczyna się niezależnie od ewentualnego sprzeciwu |

## Stan wyjściowy (2026-11-22)

- Etapy 0, 1 i część Etapu 2 (notatki, rozmowa oceniająca) gotowe. `EvaluationStages.Current` zwraca `Completion`, gdy otwarta ocena ma `WrittenEvaluationAt`.
- **Części II arkusza nie ma.** `WrittenEvaluationAt` ustawiają tylko dane demo (Monika Sikora, ocena 15).
- Krok „Zakończenie” na stronie pracownika (`Przelozony/EmployeeEvaluation.razor`) pokazuje sam opis z `EvaluationText.StageDescription`. „Moi pracownicy” na Etapie 3: „Zapoznaj pracownika z oceną do <termin oceny>”, bez przycisku.
- `PeriodEnd` jest wymagane i ustawiane przy zakładaniu oceny na „termin oceny minus dzień” (`EvaluationService.CreateOpenEvaluation`).
- Nowy cykl liczy się od `CompletedAt` ostatniej zakończonej oceny (`EvaluationSchedule.CycleStart`), a kolejny termin oceny to **termin poprzedniej oceny plus rok** (`EvaluationSchedule.NextDueDate`). Pierwsza ocena rok po zatrudnieniu.
- `CLAUDE.md` mówi o cyklu rocznym jako ustaleniu z organizatorami. `ProcessGuide.razor`: „Ocena odbywa się co roku. Pierwsza jest po roku pracy…”.
- Brak biblioteki PDF. Brak endpointów do pobierania plików (jest tylko `Auth/AuthEndpoints.cs`).

## Podjęte decyzje

| # | Temat | Decyzja |
|---|---|---|
| D1 | Część II | Plan **zakłada gotową część II arkusza** z osobnego planu Etapu 2 (oceny cząstkowe, sugerowany poziom i decyzja przełożonego, średnia, wynik, uzasadnienie, wnioski do IPRZ; `WrittenEvaluationAt` ustawia jej zatwierdzenie). Etap 3 jej nie zmienia, tylko ją drukuje |
| D2 | Długość cyklu | **Zawsze 24 miesiące**: pierwsza ocena 24 miesiące po zatrudnieniu, kolejna 24 miesiące po dacie sporządzenia poprzedniej. Zastępuje ustalenie o cyklu rocznym w `CLAUDE.md`. Wyliczenie zostaje podpowiedzią terminu, dopóki termin nie będzie wpisywany przez HR |
| D3 | Daty w Etapie 3 | Obie daty wpisuje przełożony w Etapie 3: **data sporządzenia oceny** (data podpisu oceniającego w części II, koniec okresu oceny, początek nowego cyklu) i **data zapoznania pracownika z oceną** (podpis pracownika). `WrittenEvaluationAt` to tylko moment zatwierdzenia części II i nie jest datą sporządzenia |
| D4 | Dwa kroki | 1) Przełożony zapisuje datę sporządzenia i pobiera PDF do podpisu (miejsce na datę zapoznania puste). 2) Wpisuje datę zapoznania i „Zamknij ocenę”. Datę sporządzenia można poprawiać do zamknięcia. PDF po zamknięciu ma obie daty |
| D5 | Zasady dat | Data sporządzenia: nie wcześniej niż data odbytej rozmowy oceniającej, nie później niż dziś; po terminie oceny dozwolona (jak rozmowy po terminie, bez blokady). Data zapoznania: nie wcześniej niż data sporządzenia, nie później niż dziś. Zamknięcie wymaga odbytej rozmowy oceniającej (potwierdza ją podpis pracownika) |
| D6 | PDF | **QuestPDF** (licencja Community ustawiana w `Program.cs`), generowany na serwerze, pobierany przez endpoint GET z autoryzacją roli i sprawdzeniem, że ocena dotyczy pracownika z zespołu przełożonego |
| D7 | Zawartość PDF | Wierne odwzorowanie arkusza B albo C: część I, część II i pouczenie o sprzeciwie (tekst z arkusza). Bez części III (wniosek o stopień). „Nazwa urzędu” i „miejscowość” to **puste linie** do wpisania ręcznie. Sekcje tylko dla urzędnika SC (dane poprzedniej oceny, wniosek o kolejny stopień) **puste do ręcznego wpisania**. Podpisy jako puste linie. Oczekiwań i celów z części I nie ma w arkuszu, więc nie trafiają do PDF |
| D8 | Dostęp do PDF | W Etapie 3 od zapisania daty sporządzenia i **także po zamknięciu**: na stronie pracownika w nowym cyklu link „Poprzednia ocena” z pobraniem PDF. Bez osobnej zakładki z historią ocen |
| D9 | Poza zakresem | Pouczenie o sprzeciwie jako osobny element UI, ponowna ocena po 6 miesiącach przy ocenie negatywnej, informacja „ocena sporządzona” u HR, historia ocen w zakładce „Oceny”, asystent AI. Pouczenie jest tylko w PDF, bo jest częścią arkusza |

## Kolejność ticketów

1. [01_cykl_24_miesiace.md](01_cykl_24_miesiace.md): cykl 24 miesiące w harmonogramie, danych demo, tekstach i `CLAUDE.md`
2. [02_model_danych.md](02_model_danych.md): data sporządzenia, data zapoznania z oceną, koniec okresu, migracja, dane demo
3. [03_reguly_i_serwis.md](03_reguly_i_serwis.md): kroki Etapu 3, walidacja dat, zapis daty sporządzenia, zamknięcie oceny, nowy cykl
4. [04_pdf_arkusza.md](04_pdf_arkusza.md): QuestPDF, dokument arkusza B/C, endpoint pobierania
5. [05_formularz_etapu_3.md](05_formularz_etapu_3.md): formularz Etapu 3 na stronie pracownika, „Moi pracownicy”, link do PDF poprzedniej oceny

01 jest niezależny od reszty. 02 i 03 są podstawą 04 i 05. 04 wymaga gotowej części II (D1); 05 można zacząć przed 04, z przyciskiem PDF dopiętym na końcu.

## Stan realizacji

| Ticket | Stan | Uwagi |
|---|---|---|
| 01 | do zrobienia | |
| 02 | do zrobienia | |
| 03 | do zrobienia | |
| 04 | do zrobienia | wymaga części II |
| 05 | do zrobienia | |

## Zasady obowiązujące we wszystkich ticketach

- Bez komentarzy w kodzie (C#, Razor, CSS). Kod komponentu w `Nazwa.razor.cs`.
- Bez nowych testów automatycznych. Sprawdzenie ręczne w aplikacji według sekcji „Gotowe, gdy”.
- Kod i nazwy po angielsku, teksty w UI i w PDF po polsku. Dane pracowników fikcyjne.
- UI według `design.md`: najpierw komponenty Radzen, wzorce z istniejących widoków (`StartingConversationStage`, `AssessmentStage`), motyw `standard-dark` bez nadpisywania.
- W UI zawsze nazwane daty: „termin oceny”, „data sporządzenia oceny”, „data zapoznania pracownika z oceną”. Bez informacji o prototypie i brakujących funkcjach.
- Bez powiadomień i przypomnień (`CLAUDE.md`).
- Baza jest tworzona od nowa przy starcie, migracje można wygenerować od nowa (jedna `Initial`). Bez migracji przenoszących dane; liczy się aktualny model i `HasData`.
- Po każdym uruchomieniu aplikacji do testów: zatrzymać ją (aplikacja blokuje pliki w `bin`).
- Po zakończeniu ticketu zaktualizować `CLAUDE.md` (architektura, lista różnic w „Proces oceny okresowej”) i `design.md` (nowe wzorce), jeśli ticket je zmienia, oraz tabelę „Stan realizacji” powyżej.
