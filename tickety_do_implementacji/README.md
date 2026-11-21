# Tickety do implementacji: asystent AI z bazą wiedzy (RAG)

Cel: asystent poprawnie odpowiada na pytania jury o zasady pierwszej i okresowej oceny, na podstawie bazy wiedzy i ze wskazaniem źródła (`mvp.md` sekcja 5, MUST).

## Stan wyjściowy (2026-11-21)

- Czat: `Components/Assistant/AssistantChat.razor`. Pływający dymek w prawym dolnym rogu, `RadzenAIChat` w `RadzenCard`. Montowany w `MainLayout.razor` dla zalogowanych.
- Radzen `AIChatService` (`AddAIChatService()`) trzyma historię rozmowy w pamięci sesji Blazora i przy każdym pytaniu wysyła całą historię do zarejestrowanego `IChatClient`.
- Klient modelu: `Assistant/AssistantClientFactory.cs`:
  - z kluczem `Assistant:ApiKey` (user-secrets) tworzy Claude API (pakiet `Anthropic` 13.12.0, `AsIChatClient(model)`, model `claude-haiku-5-5` z `appsettings.json`);
  - bez klucza zwraca `Assistant/MockChatClient.cs`.
- `AssistantClientFactory` zeruje `Temperature`, bo Radzen wysyła 0.7, a API Claude odrzuca niedomyślną temperaturę (400). Ustawia też `MaxOutputTokens ??= 16000`.
- Źródła w UI: `RadzenAIChat` pokazuje numerowane chipy z `CitationAnnotation` (`Title`, `Snippet`) dołączonych do `AIContent.Annotations`. Wzór w `MockChatClient` (ostatnia aktualizacja strumienia z pustym `TextContent` i adnotacjami).
- Logi `Microsoft.Extensions.AI` na poziomie `Trace` w `appsettings.Development.json`, klient opakowany w `UseLogging()` w `Program.cs`.
- Znany drobiazg: etykieta „Sources” nad chipami jest po angielsku (tekst z Radzena).

## Podjęte decyzje

| # | Temat | Decyzja |
|---|---|---|
| D1 | Podejście | RAG: do modelu trafiają tylko pasujące fragmenty, nie cała baza |
| D2 | Wyszukiwanie | BM25 (po słowach), bez embeddingów. Embeddingi ewentualnie później, jeśli testy pokażą problem z parafrazami |
| D3 | Zakres źródeł | Rozporządzenie o ocenach okresowych (2016), rozporządzenie o pierwszej ocenie (2015), poradnik o ocenach okresowych, poradnik o pierwszej ocenie, arkusze B, C i pierwszej oceny, `Model_kompetencji.xlsx`, przykładowe OSP (zip) oraz ustawa o służbie cywilnej |
| D4 | Ustawa o służbie cywilnej | Tekst jednolity Dz.U. 2026 poz. 590 (obwieszczenie z 17.04.2026, stan prawny na 14.04.2026). Najnowszy w API Sejmu na 2026-11-21, obejmuje zmiany z Dz.U. 2025 poz. 620 i 1661 oraz 2026 poz. 26 i 160. Pobrany z `https://api.sejm.gov.pl/eli/acts/DU/2026/590/text.pdf` (ISAP blokuje automatyczne pobieranie) |
| D5 | Własne FAQ | Na razie pomijamy; dopiszemy później do pytań, których przepisy nie wyjaśniają wprost |
| D6 | Kiedy wyciągamy tekst | Jednorazowo skryptem. Wynik `knowledge-base.json` trafia do repo, aplikacja tylko go wczytuje |
| D7 | Miejsce | **Cała baza wiedzy w jednym folderze `knowledge-base/`** w katalogu głównym repo: źródła, skrypt i wynikowy JSON |
| D8 | Podział na fragmenty | Według struktury dokumentu: przepisy po artykułach i paragrafach, poradniki po rozdziałach (długie dzielone do ok. 1500 znaków), arkusze po częściach, model kompetencji po kompetencji, OSP po sekcjach. Każdy fragment ma opis źródła |
| D9 | Brak trafnych fragmentów | Wariant B: model jest zawsze pytany, a prompt każe mu powiedzieć „Nie znalazłem tego w bazie wiedzy”, gdy fragmenty nie odpowiadają na pytanie. Sztywna odmowa (bez modelu) tylko przy pytaniach o poziom oceny |
| D10 | Dopytania | Zapytanie do wyszukiwarki = dwa ostatnie pytania użytkownika |
| D11 | Parametry | 5 fragmentów; normalizacja: małe litery, bez polskich znaków, bez słów pomijalnych (i, w, na…); prosta lista synonimów. **Zmiana po testach (2026-11-21):** zamiast pełnego stemmera słowa są obcinane do 7 znaków („sprzeciwu” → „sprzeci”). Bez tego recall@5 wynosił 83–88% (błędy na odmianie: „rozmowy oceniającej”, „skala punktowa”), z obcinaniem do 7 znaków 100% na 24 pytaniach. Długość 5–6 dawała gorsze wyniki |
| D12 | Testy | Nowy projekt `KompasKompetencji.Tests` (xUnit) w solucji, ok. 20 pytań z oczekiwanym źródłem, bez wywołań API |
| D13 | Dostawca AI | Zostaje Claude API (`claude-haiku-5-5`). Zgodność z wymogiem „darmowych narzędzi AI” jest otwarta, patrz `do_rozwazenia.md` |

## Stan realizacji (2026-11-21)

| Ticket | Stan | Uwagi |
|---|---|---|
| 01 | zrobiony | `knowledge-base/` ze źródłami; decyzje o duplikacie rozporządzenia i metadanych w `knowledge-base/README.md` |
| 02 | zrobiony | 277 fragmentów, ok. 72 tys. tokenów; zamiast `generatedAt` skróty SHA-256 źródeł (skrypt deterministyczny) |
| 03 | zrobiony | BM25 z obcinaniem słów (D11), synonimy w `Synonyms.cs` |
| 04 | zrobiony | sprawdzony ręcznie z Claude API; dodatkowo: usuwanie bloków rozumowania z historii (inaczej dopytanie kończy się błędem 400), numer fragmentu na chipie źródła zgodny z [n] w tekście, polska etykieta „Źródła” przez `ILocalizer` |
| 05 | zrobiony | 78 testów, recall@5 = 100% (24/24) |
| 06 | zrobiony | sprawdzone w logach `Trace`: do modelu trafia „Pracownik A”; ograniczenia w `do_rozwazenia.md` |
| 07 | nie robiony | czeka na odpowiedź organizatorów |

## Kolejność ticketów

1. [01_folder_bazy_wiedzy.md](01_folder_bazy_wiedzy.md): folder `knowledge-base/` i źródła
2. [02_skrypt_budujacy_baze.md](02_skrypt_budujacy_baze.md): skrypt wyciągający tekst i dzielący na fragmenty, `knowledge-base.json`
3. [03_wyszukiwanie_bm25.md](03_wyszukiwanie_bm25.md): wczytanie bazy i wyszukiwarka BM25 w aplikacji
4. [04_klient_rag.md](04_klient_rag.md): `KnowledgeBaseChatClient` (fragmenty w prompcie, źródła w UI, odmowa przy poziomie oceny)
5. [05_testy_wyszukiwania.md](05_testy_wyszukiwania.md): projekt testów i zestaw pytań
6. [06_pseudonimizacja.md](06_pseudonimizacja.md): imiona i nazwiska pracowników nie trafiają do modelu
7. [07_przelacznik_dostawcy.md](07_przelacznik_dostawcy.md): opcjonalnie, zależnie od odpowiedzi organizatorów

Tickety 03 i 05 można robić razem: testy wyszukiwania powstają razem z wyszukiwarką.

## Zasady obowiązujące we wszystkich ticketach

- Bez komentarzy w kodzie (C#, Razor, CSS, Python).
- Kod i nazwy po angielsku, teksty w UI po polsku.
- Po każdym uruchomieniu aplikacji do testów: zatrzymać ją i zamknąć kartę przeglądarki (aplikacja blokuje pliki w `bin`).
- Nie wpisywać kluczy API w czat ani w pliki repo; klucz tylko w user-secrets.
- Po zakończeniu ticketu zaktualizować `README.md` (uruchomienie) i `CLAUDE.md` (architektura), jeśli ticket je zmienia.
