# Kompas kompetencji: propozycja MVP

Źródła: treść zadania (PDF), grafika `Kompas Kompetencji_ Ścieżki Oceny.png`, `Model_kompetencji.xlsx`, arkusze ocen (pierwsza ocena, B, C).

## 1. Cel MVP

Działający prototyp, który na demo przejdzie **jedną kompletną ścieżkę przełożonego**:

> rozpoczęcie procesu → kryteria i kompetencje → ocena wskaźników behawioralnych → **sugerowany poziom** → **decyzja przełożonego** → dokument PDF

Do tego **asystent AI**, który poprawnie odpowiada na pytania jury o zasady pierwszej i okresowej oceny. To dokładnie minimum z sekcji 4 i 6 treści zadania. Wszystko poza tym jest dodatkiem.

## 2. Zakres w skrócie

| Priorytet | Co | Dlaczego |
|---|---|---|
| **MUST** | Ścieżka **oceny okresowej (arkusz C)** od początku do końca | Wymóg demo: jedna kompletna ścieżka |
| **MUST** | Model kompetencyjny: wskaźniki, wybór zachowań, sugerowany poziom, decyzja przełożonego | Rdzeń zadania, oceniany wprost |
| **MUST** | Asystent formalny oceny (RAG ze źródłami, mówi „nie wiem”, gdy nie ma odpowiedzi) | Jury testuje go pytaniami |
| **MUST** | Generowanie PDF arkusza | Wymaganie techniczne, „poprawność dokumentacji” |
| **SHOULD** | Asystent rozmowy (pytania, tematy, argumenty, plan rozwoju) | Drugi obszar AI z treści zadania |
| **SHOULD** | Panel HR: statusy, terminy, przycisk „wyślij przypomnienie” | Perspektywa HR z treści zadania |
| **SHOULD** | Arkusz B (kierownicy) i podpowiedź B/C na podstawie stanowiska | Mały koszt, bo ta sama logika co C |
| **COULD** | Ścieżka **pierwszej oceny** | Druga ścieżka, prostsza (stałe kryteria) |
| **COULD** | Podpowiedź kryteriów dodatkowych na podstawie OSP (AI) | Efektowne, ale nie jest wymagane |
| **POZA MVP** | Integracje EZD/QNT, SSO, e-podpis, prawdziwe maile | Tylko jako kierunek rozwoju na slajdzie |

### Dlaczego ocena okresowa jako pierwsza ścieżka?
- Skala arkusza B/C ma **5 poziomów (1/3/5/7/9 pkt)** i odpowiada 1:1 pięciu poziomom modelu kompetencji. Nie trzeba niczego przeliczać.
- Jest w niej **prawdziwy dobór kryteriów**: 5 obowiązkowych i do 3 dodatkowych wybranych przez oceniającego. Jury chce zobaczyć „pracę z kryteriami”.
- Pierwsza ocena ma stałe 5 elementów i skalę 3-poziomową (3/5/7 pkt). Trzeba by tam ustalić mapowanie 5 → 3 poziomy (zob. pytania otwarte).

## 3. Ścieżka przełożonego (ekrany)

Kolejność według grafiki koncepcyjnej: *Analiza OSP → wybór formularza → Rozpocznij ocenę → Zakończ ocenę z modelem kompetencji*.

1. **Logowanie / wybór roli.** W MVP konta demo: przełożony i HR, z prawdziwym podziałem uprawnień w backendzie.
2. **Pulpit przełożonego.** Lista podwładnych (dane fikcyjne) ze statusem oceny i najbliższym terminem. Przycisk „Rozpocznij ocenę”.
3. **Wybór rodzaju oceny i arkusza.** Pierwsza albo okresowa. Dla okresowej arkusz **B** (osoba kierująca zespołem) lub **C** (pozostali), z podpowiedzią na podstawie stanowiska.
4. **Część I: kryteria i termin.**
   - Kryteria obowiązkowe wstawione automatycznie i zablokowane. Arkusz C: Wykorzystywanie wiedzy, Rzetelność, Organizacja pracy, Współpraca, Komunikacja. Arkusz B: Zarządzanie ludźmi, Podejmowanie decyzji, Organizacja pracy, Współpraca, Komunikacja.
   - Wybór **maks. 3 kryteriów dodatkowych** z modelu, z walidacją limitu.
   - Termin sporządzenia oceny i potwierdzenie rozmowy oraz zapoznania ocenianego z kryteriami.
5. **Część II: ocena kompetencji (główny ekran).** Dla każdego kryterium:
   - przełożony **zaznacza wskaźniki adekwatne do stanowiska** (model wprost to zaleca),
   - przy każdym wskaźniku widzi **5 kart z opisami zachowań** i wybiera tę, która najlepiej opisuje to, co obserwował, albo **„brak podstaw do oceny”**,
   - pole na przykład w formacie *sytuacja → działanie → rezultat*,
   - w panelu bocznym widzi **sugerowany poziom** na żywo, z wyjaśnieniem, jak go policzono.
6. **Decyzja przełożonego.** Przyjęcie sugestii jednym kliknięciem albo wybór innego poziomu. **Zmiana wymaga krótkiego uzasadnienia.** System zapisuje sugestię i decyzję.
7. **Podsumowanie oceny.**
   - Średnia arytmetyczna z ocen cząstkowych i ogólny poziom według progów z arkusza (1–2, >2–4, >4–6, >6–8, >8–9 pkt).
   - Ocena pozytywna/negatywna według reguły z rozporządzenia.
   - **Uzasadnienie** i **wnioski do IPRZ**. AI może przygotować szkic z notatek przełożonego, ale przełożony go redaguje i zatwierdza.
8. **Przygotowanie do rozmowy.** Asystent rozmowy przygotowuje listę pytań, tematów do omówienia i argumentów na podstawie ocen i notatek.
9. **Generuj PDF.** Wypełniony arkusz C (lub B) do pobrania.

## 4. Logika sugerowanego poziomu (bez AI)

Sugestia jest liczona **deterministyczną, jawną regułą, a nie przez model językowy**. Dzięki temu jest przewidywalna i łatwa do wyjaśnienia, a AI z definicji nie decyduje o ocenie.

1. Każdy oceniony wskaźnik dostaje wartość 1–5 (znacznie poniżej … znacznie powyżej). Wskaźniki z „brak podstaw” są pomijane.
2. **Sugerowany poziom kryterium** to średnia z ocenionych wskaźników zaokrąglona do najbliższego poziomu.
   - Przy remisie (np. 3,5) system pokazuje oba sąsiednie poziomy i prosi przełożonego o wybór.
   - Jeśli oceniono mniej niż połowę zaznaczonych wskaźników, pojawia się ostrzeżenie „za mało podstaw”.
3. Poziom przelicza się na punkty arkusza: 1 / 3 / 5 / 7 / 9.
4. **Ostateczny poziom zawsze wybiera przełożony.** Ślad audytowy zapisuje sugestię, decyzję i uzasadnienie zmiany.
5. Ogólny poziom i ocenę pozytywną/negatywną liczy się już wprost według arkusza i rozporządzenia.

UI pokazuje rozkład, np. „2 × na poziomie, 1 × powyżej → sugestia: na poziomie oczekiwań”.

## 5. Asystent AI

Jeden panel czatu dostępny z każdego ekranu, z dwoma trybami zgodnymi z grafiką koncepcyjną.

### Asystent formalny oceny (MUST)
- **Baza wiedzy (RAG):** ustawa o służbie cywilnej (potrzebny zakres), rozporządzenie o pierwszej ocenie (2015), rozporządzenie o ocenach okresowych (2016), poradniki KPRM, model kompetencji, własny zestaw pytań i odpowiedzi. Wszystko jest w `tresci_zadania/pliki_dodatkowe`.
- Każda odpowiedź **cytuje źródło** (dokument + paragraf/strona) i pokazuje fragment, na którym się opiera.
- Jeśli w bazie nie ma wystarczająco trafnych fragmentów (próg podobieństwa), asystent mówi **„Nie znalazłem tego w bazie wiedzy”** zamiast zgadywać.
- Asystent zna kontekst, czyli na którym etapie i w której ścieżce jest użytkownik.
- Twarda reguła w prompcie i w UI: **nie proponuje poziomu oceny pracownika**.

### Asystent rozmowy (SHOULD)
- Na podstawie wybranych zachowań i notatek proponuje pytania do pracownika, kwestie do omówienia, uporządkowane argumenty uzasadniające ocenę oraz pomysły na plan rozwoju (IPRZ).

### Bezpieczeństwo AI
- **Pseudonimizacja:** do modelu nie trafiają imię, nazwisko ani dane identyfikujące, tylko „Pracownik A” i treść notatek.
- Zgodnie z regulaminem tylko **darmowe narzędzia AI** (np. lokalny model przez Ollama lub darmowy tier API; do potwierdzenia w regulaminie).
- **Zestaw testowy ok. 20–30 pytań** o zasady pierwszej i okresowej oceny z oczekiwanymi odpowiedziami. Uruchamiamy go przed demo, bo jury będzie zadawać podobne pytania.

## 6. Panel HR / administratora (SHOULD)

- Tabela procesów: pracownik, przełożony, rodzaj oceny, arkusz, **status**, **termin**, liczba dni do terminu (kolor: OK / zbliża się / po terminie).
- Filtry: status, komórka, przekroczony termin.
- Przycisk **„Wyślij przypomnienie”**. W MVP jest to powiadomienie w aplikacji plus log „wysłanego” maila, bez prawdziwej wysyłki.
- Proponowane statusy oceny okresowej: *Nierozpoczęta → Kryteria ustalone (Część I) → Ocena w toku → Gotowa do rozmowy → Zakończona*.

## 7. Wybrany stack

| Warstwa | Wybór | Uzasadnienie |
|---|---|---|
| Frontend | **Blazor** (.NET 11) + **Radzen.Blazor** | Gotowe komponenty do kreatora i formularzy, jeden język i jeden projekt z backendem |
| Backend | **ASP.NET Core** (.NET 11) + Minimal API z dokumentacją **OpenAPI** | Wymóg „udokumentowanego API”, gotowość na integracje z EZD/QNT |
| Baza danych | **SQL Server** + **EF Core** (migracje) | Dane procesu. Embeddingi bazy wiedzy w zwykłej tabeli lub pliku JSON, podobieństwo liczone w pamięci (mały korpus, baza wektorowa niepotrzebna) |
| AI | Darmowy LLM przez API zgodne z OpenAI (`Microsoft.Extensions.AI`), dostawca w konfiguracji | Zgodność z regulaminem, zmiana dostawcy bez zmian w kodzie (docelowo model lokalny) |
| PDF | Generowanie po stronie serwera w .NET | Wierne odwzorowanie arkusza |
| Auth | W MVP: ASP.NET Core Identity + role (przełożony, HR). Docelowo: OIDC/SSO | Wymóg ról i uprawnień, prosta droga do wdrożenia |
| Uruchomienie | Docker Compose (aplikacja + SQL Server) | Łatwe odpalenie przez jury i dalszy rozwój |

Model kompetencji z `Model_kompetencji.xlsx` (14 kompetencji, 49 wskaźników, 5 poziomów) jest importowany skryptem do JSON/bazy. Dzięki temu nowa wersja modelu nie wymaga zmian w kodzie.

## 8. Minimalny model danych

- `User` (rola: przełożony / HR)
- `Employee` (fikcyjne dane: stanowisko, komórka, czy kieruje zespołem, przełożony)
- `Evaluation` (rodzaj, arkusz B/C, status, okres oceny, terminy)
- `CriterionSelection` (kryterium, obowiązkowe/dodatkowe)
- `IndicatorRating` (wskaźnik, poziom 1–5 lub brak podstaw, przykład/notatka)
- `CriterionDecision` (poziom sugerowany, poziom ostateczny, uzasadnienie zmiany)
- `Reminder` (do kogo, czego dotyczy, kiedy wysłane)

## 9. Dane i scenariusz demo

- 3 fikcyjnych pracowników opartych na 3 szablonach OSP z materiałów. Jeden kieruje zespołem (arkusz B), dwóch nie (arkusz C).
- Gotowy scenariusz: przełożony ocenia pracownika na arkuszu C, w jednym kryterium **nie zgadza się z sugestią** i podaje uzasadnienie. Potem pyta asystenta o termin na sprzeciw (7 dni, art. 83 ustawy) i generuje PDF.
- W panelu HR widać jedną ocenę po terminie i wysyłkę przypomnienia.

## 10. Kolejność prac

1. Import modelu kompetencji (xlsx → JSON/baza) i szkielet aplikacji (auth + role).
2. Ścieżka oceny okresowej, arkusz C: kryteria, ocena wskaźników, sugestia, decyzja, podsumowanie.
3. Asystent formalny: indeksacja dokumentów, cytaty, odpowiedź „nie wiem”, zestaw testowy pytań.
4. Generowanie PDF arkusza.
5. Asystent rozmowy.
6. Panel HR z przypomnieniami.
7. Arkusz B, potem ścieżka pierwszej oceny, jeśli zostanie czas.
8. Materiały do oddania: opis projektu, prezentacja (maks. 10 slajdów), opis wykorzystania AI, opis technologii i architektury.

## 11. Poza MVP (kierunek rozwoju na slajd)

- Integracja z **EZD** (obieg i archiwizacja arkuszy) i **QNT** (dane kadrowe, terminy ocen).
- SSO i podpis elektroniczny arkusza.
- Prawdziwe powiadomienia e-mail i kalendarz terminów.
- Analityka dla HR: rozkład ocen i porównanie spójności ocen między przełożonymi.
- Konfiguracja modelu kompetencji i wdrożenie w innych urzędach (multi-tenant).

## 12. Pytania otwarte do mentorów

1. **Do kogo idą przypomnienia** od HR: do przełożonego, do pracownika, do obu?
2. **Pierwsza ocena:** jak przełożyć 5 poziomów modelu kompetencji na 3 poziomy arkusza (poniżej / na poziomie / powyżej)?
3. Reguły **oceny pozytywnej/negatywnej** i oceny łącznej. Do potwierdzenia w rozporządzeniach przed implementacją.
4. Jakie **darmowe narzędzia AI** są dopuszczalne (sprawdzić w `Regulamin zadania konkursowego.pdf`)?
5. „Responsywny, ale nie na smartfony”: czy tablety mają być obsługiwane?
