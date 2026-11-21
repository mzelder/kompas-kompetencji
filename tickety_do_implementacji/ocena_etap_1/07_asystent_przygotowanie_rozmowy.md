# 07. Asystent AI w przygotowaniu do rozmowy rozpoczynającej

Decyzja: D10. Wymaga ticketu 03. Przed zmianami w asystencie sprawdzić `do_rozwazenia.md`.

## Cel

Treść zadania: asystent ma „pomóc przygotować się do rozmowy z pracownikiem, np. poprzez proponowanie pytań, wskazywanie kwestii wymagających omówienia”. W Etapie I asystent pomaga przygotować rozmowę rozpoczynającą i sformułować oczekiwania oraz cele. Nie wybiera kryteriów i nie zapisuje niczego w arkuszu sam.

## Zakres

- Na karcie „Do omówienia na rozmowie” (ticket 03) przycisk „Przygotuj rozmowę z asystentem” (`ButtonStyle.Base`, ikona asystenta jak w `MainLayout`).
- Kliknięcie otwiera czat asystenta (`AssistantChat`) i wysyła gotowe pytanie zbudowane z danych etapu, bez imienia i nazwiska pracownika (stanowisko, cel stanowiska, główne zadania, arkusz, kryteria obowiązkowe i wybrane dodatkowe). Przykład:
  „Przygotowuję rozmowę rozpoczynającą ocenę okresową pracownika na stanowisku <stanowisko> (arkusz <B/C>). Zadania: <…>. Kryteria: <…>. Co powinienem omówić, jakie pytania zadać i jak sformułować oczekiwania do tych kryteriów?”
- Najpierw sprawdzić, czy `RadzenAIChat` pozwala wysłać wiadomość z kodu (np. metoda komponentu albo parametr początkowej wiadomości). Jeśli nie, przycisk wstawia pytanie do pola wiadomości i otwiera czat; jeśli i to niemożliwe, kopiuje pytanie do schowka z powiadomieniem „Pytanie skopiowane. Wklej je w czacie asystenta.”
- Odpowiedź opiera się na bazie wiedzy (§ 5, poradnik 6.2.1, model kompetencji) i podaje źródła jak dotąd. Pseudonimizacja działa bez zmian; danych osobowych w pytaniu i tak nie ma.
- Pytania testowe dopisać do `tests/KompasKompetencji.Tests/retrieval-questions.json` (np. „Co omówić na rozmowie rozpoczynającej?”, „Jak sformułować oczekiwania wobec pracownika do kryterium rzetelność?”) i poprawić synonimy, jeśli wyszukiwanie nie trafia.

## Poza zakresem

- Wpisywanie odpowiedzi asystenta do pól oczekiwań i celów jednym kliknięciem. Przełożony przepisuje lub kopiuje to, co uzna za trafne.
- Podpowiadanie kryteriów dodatkowych przez AI (D5: podpowiedź liczy jawna reguła).

## Gotowe, gdy

- Przycisk otwiera czat z pytaniem opartym na danych etapu, bez imienia i nazwiska pracownika.
- Odpowiedź wymienia tematy z § 5 i proponuje pytania, ze źródłami.
- `dotnet test` przechodzi z nowymi pytaniami.
