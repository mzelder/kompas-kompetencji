# Do rozważenia

Otwarte decyzje i ryzyka, które trzeba rozstrzygnąć przed oddaniem rozwiązania.

## Dostawca modelu AI a wymóg „darmowych narzędzi AI”

**Stan obecny:** asystent korzysta z Claude API (`claude-haiku-5-5`, pakiet `Anthropic`, `Assistant/AssistantClientFactory.cs`). API jest płatne za zużycie, a darmowe są tylko kredyty startowe dla nowych kont.

**Co mówią dokumenty konkursu:**

- `Regulamin zadania konkursowego.pdf` nie ogranicza dostawców AI. Jedyny zapis (§2 pkt 7): użycie AI jest dopuszczalne, jeśli finalnie praca jest efektem analizy i pracy człowieka i autorskim rozwiązaniem zespołu.
- `Pełna treść zadania konkursowego.pdf` zawiera dwa zapisy, które trzeba pogodzić:
  - „Zespoły przygotowujące rozwiązanie mogą korzystać na tym etapie wyłącznie z darmowych narzędzi AI.”
  - „W ramach hackathonu dopuszczalne jest wykorzystanie gotowych modeli językowych, usług chmurowych, bibliotek open source oraz zewnętrznych API. Zespół powinien wskazywać zastosowane technologie oraz elementy wymagające dodatkowego zabezpieczenia lub dostosowania przed wdrożeniem rozwiązania w środowisku produkcyjnym.”

**Interpretacja:** zewnętrzne API są dozwolone, ale narzędzia AI mają być darmowe. Płatne API na darmowych kredytach startowych to szara strefa i jury może uznać je za niezgodne z wymogiem.

**Do zrobienia:**

1. Zapytać organizatorów lub mentorów HackNation, czy płatne API używane w ramach darmowych kredytów startowych mieści się w „darmowych narzędziach AI”.
2. Do czasu odpowiedzi rozwijać bazę wiedzy, RAG, zestaw testowy i UI. Nie zależą od dostawcy, bo całość idzie przez `IChatClient`, a dostawcę wybiera tylko `AssistantClientFactory`.
3. Jeśli odpowiedź będzie negatywna, przejść na wariant w pełni darmowy.

**Warianty dostawcy:**

| Wariant | Koszt | Zalety | Wady |
|---|---|---|---|
| Claude API (obecny) | płatny za zużycie, kredyty startowe | dobra jakość po polsku, Anthropic domyślnie nie trenuje na danych z API, oficjalny pakiet .NET z `IChatClient` | zgodność z wymogiem „darmowych narzędzi AI” niepewna |
| Ollama z modelem lokalnym (np. Bielik) | darmowy | w pełni zgodny z wymogiem, dane nie opuszczają komputera, polski model, brak regulaminu dostawcy | wymaga mocnego komputera na demo, wolniejsze odpowiedzi |
| Gemini API, darmowy plan | darmowy | jeden klucz do czatu i embeddingów | na darmowym planie Google używa danych do ulepszania modeli; regulamin wymaga płatnego planu przy udostępnianiu aplikacji użytkownikom z EOG |
| GitHub Models | niedostępny | | usługa wyłączona 30 lipca 2026 |

**Możliwe usprawnienie:** ustawienie `Assistant:Provider` (`Anthropic` albo `Ollama`) w konfiguracji, żeby przełączać dostawcę bez zmian w kodzie.

## Pseudonimizacja danych pracowników w asystencie AI

**Stan obecny:** przed wysłaniem rozmowy do modelu imiona i nazwiska osób z tabeli `Employees` są zamieniane na „Pracownik A”, „Pracownik B”… (kolejność według Id), a w odpowiedzi z powrotem (`Assistant/Privacy/`). Sprawdzone w logach `Trace`.

**Ograniczenia:**
- **Odmiana:** rozpoznawanie odmiany to rdzeń imienia lub nazwiska plus typowa końcówka („Kowalsk” + „iej”, „Ann” + „ie”, „Mark” + „iem”). Formy z oboczności („Piotrze” działa, ale np. „Pawle” od „Paweł” już nie) i zdrobnienia („Ania”, „Janek”) nie są rozpoznawane.
- **Przypadek po odtworzeniu:** model odmienia pseudonim („z Pracownikiem A”), a po zamianie wraca forma podstawowa („z Anna Kowalska”). Tekst jest zrozumiały, ale niegramatyczny.
- **Osoby spoza bazy:** nazwisko, którego nie ma w `Employees`, trafia do modelu bez zmian. Dlatego zostaje ostrzeżenie w czacie „Nie wpisuj imion ani nazwisk pracowników”.
- **Fałszywe trafienia:** słowo identyczne z imieniem lub nazwiskiem z bazy (np. „marka” przy pracowniku Marek) też zostanie zamienione.
- **Te same imiona:** samo imię wspólne dla kilku osób zawsze trafia do pierwszej z nich.

**Możliwe usprawnienia:** słownik odmiany (np. Morfeusz), rozpoznawanie nazw własnych (NER), osobny model lokalny do anonimizacji.

## Wyszukiwarka bazy wiedzy (BM25)

**Stan obecny:** 31 pytań testowych, recall@5 = 100% (`dotnet test`). Obcinanie słów do 7 znaków zastępuje stemmer (decyzja D11 w `tickety_do_implementacji/README.md`).

**Weryfikacja odpowiedzi z oryginałami (2026-11-21):** 9 pytań zadanych w UI, odpowiedzi porównane z oryginalnymi plikami z `tresci_zadania/pliki_dodatkowe/` i tekstem ustawy z API Sejmu. W żadnej odpowiedzi nie było faktu sprzecznego z dokumentami poza jednym: asystent zgadł treść § 5, do którego fragment tylko odsyłał. Pozostałe problemy wynikały z wyszukiwania: właściwy fragment był w bazie, ale nie trafił do 5 wybranych, więc asystent pisał, że „nie znalazł”. Poprawione zostały synonimy, tytuły paragrafów rozporządzeń, kolejność tabeli kryteriów w arkuszach B i C oraz prompt (zakaz zgadywania treści przepisu, do którego fragment odsyła).

Sprawdzone i odrzucone warianty wyszukiwarki (recall@5 na 31 pytaniach): obcinanie do 7 znaków 31/31 (po poprawkach synonimów), obcinanie końcowych samogłosek 29/31, oba indeksy połączone przez reciprocal rank fusion 28/31 (31/31 dopiero przy 8 fragmentach), scalanie długich ustępów w jeden fragment 29/31.

**Ryzyka:**
- Pytania łączące dwa tematy („jakie są poziomy skali i kiedy ocena jest negatywna”) dostają 5 fragmentów na oba tematy naraz. Asystent odpowiada wtedy poprawnie, ale może napisać, że czegoś „nie ma we fragmentach”, choć jest w bazie (np. § 11 ust. 1 pkt 4 jest w osobnej części `rozp-okresowe-11-cz2`).
- Numer strony poradnika w źródle to strona początku rozdziału. Cytowane zdanie bywa stronę dalej (np. „punkt wyjścia 5 pkt” jest na s. 26, a źródło podaje s. 25).
- Pytania jury sformułowane zupełnie innymi słowami niż przepisy mogą nie trafić we właściwy fragment. Każde takie pytanie warto dopisać do `retrieval-questions.json` i poprawić synonimy.
- Fragmenty z arkuszy, w których przypisy wypadają w drugiej części (`-cz2`), są mniej czytelne niż pozostałe.
- Jeśli parafraz będzie dużo: embeddingi albo wariant „cała baza w kontekście z prompt cachingiem” (ok. 72 tys. tokenów, poniżej progu 100 tys., od którego Haiku 5.5 drożeje).

