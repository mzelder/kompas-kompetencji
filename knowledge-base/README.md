# Baza wiedzy asystenta AI

Wszystko, z czego korzysta asystent: dokumenty źródłowe, skrypt dzielący je na fragmenty i wynikowy `knowledge-base.json`, który aplikacja wczytuje przy starcie.

## Przebudowa bazy

```powershell
python knowledge-base/build_knowledge_base.py
```

Wymaga Pythona 3 z pakietem `pymupdf` (PDF). DOCX i XLSX są czytane biblioteką standardową. Skrypt jest deterministyczny: te same źródła dają ten sam plik. Na końcu wypisuje liczbę fragmentów na dokument i szacunkową liczbę tokenów (znaki / 3,5).

Stan na 2026-11-21: 277 fragmentów, ok. 253 tys. znaków, ok. 72 tys. tokenów.

## Źródła (`sources/`)

| Plik | Pochodzenie | Fragmenty |
|---|---|---|
| `ustawa-o-sluzbie-cywilnej-2026-poz-590.pdf` | Tekst jednolity, Dz.U. 2026 poz. 590 (obwieszczenie Marszałka Sejmu z 17.04.2026). Pobrany z `https://api.sejm.gov.pl/eli/acts/DU/2026/590/text.pdf`; najnowszy tekst jednolity w API Sejmu, sprawdzone 2026-11-21 | wybrane artykuły: 1–3, 35–38, 71, 81–84, 89, 108; długie artykuły dzielone po ustępach |
| `rozporzadzenie-oceny-okresowe-2016.pdf` | Materiały konkursowe, `Ocena okresowa w służbie cywilnej/Rozporządzenie o ocenach okresowych z 2016 r. (3).pdf` (Dz.U. 2016 poz. 470) | paragrafy § 1–14 i załącznik nr 1 (po jednym kryterium) |
| `rozporzadzenie-pierwsza-ocena-2015.pdf` | Materiały konkursowe, `Pierwsza ocena w służbie cywilnej/Rozporządzenie o pierwszej ocenie z 2015 r. (1).pdf` (Dz.U. 2015 poz. 1144) | paragrafy § 1–14 i załącznik nr 1 (po jednym elemencie oceny) |
| `poradnik-oceny-okresowe.pdf` | Materiały konkursowe, `Poradnik - oceny okresowe w służbie cywilnej (2).pdf` | rozdziały i podrozdziały, bez załączników z przykładowymi arkuszami |
| `poradnik-pierwsza-ocena-2016.pdf` | Materiały konkursowe, `Pierwsza ocena w służbie cywilnej - poradnik z 2016 r. (1).pdf` | jak wyżej |
| `arkusz-oceny-b.docx`, `arkusz-oceny-c.docx` | Materiały konkursowe, `Arkusz oceny B (2).docx`, `Arkusz oceny C (2).docx` | części I–III z objaśnieniami z przypisów |
| `arkusz-pierwszej-oceny.docx` | Materiały konkursowe, `Arkusz pierwszej oceny (2).docx` | część ogólna, rozmowa wstępna, rozmowa oceniająca, ocena i wnioski |
| `model-kompetencji.xlsx` | Materiały konkursowe, `Model_kompetencji.xlsx` | wprowadzenie i po jednej kompetencji (wskaźniki z opisem pięciu poziomów) |
| `osp/osp-*.docx` | Materiały konkursowe, `OSP_word.zip` (starszy specjalista, radca, naczelnik wydziału) | dane stanowiska, zadania, charakterystyka pracy, wymagania i kompetencje |

Fragmenty dłuższe niż ok. 1500 znaków są dzielone (po ustępach albo akapitach). Pomijane są nagłówki i stopki Dziennika Ustaw, przypisy w PDF, puste pola formularzy, niezaznaczone pola wyboru i części z podpisami.

## Podjęte decyzje

- **Dwa pliki rozporządzenia z 2016 r.:** w materiałach są dwa pliki o różnych sumach kontrolnych. `(4).pdf` w folderze OSP to tylko 3 strony z załącznikiem nr 1 (wykaz kryteriów), wycięte z pełnego rozporządzenia. Treść jest identyczna z odpowiednimi stronami `(3).pdf`, więc bierzemy tylko pełny plik `(3)`.
- **Załączniki 2–4 rozporządzenia o ocenach okresowych i załącznik 2 rozporządzenia o pierwszej ocenie** (wzory arkuszy) są pominięte, bo te same arkusze są w bazie jako pliki DOCX.
- **Dane osobowe:** treść plików OSP nie zawiera danych osobowych (pola na imię i nazwisko są puste). Imiona i nazwiska były jednak w metadanych plików: autor OSP i poradników w PDF. Przy kopiowaniu do `sources/` pola autora w metadanych zostały wyczyszczone. Skrypt i tak nie czyta metadanych.
- **Tematy paragrafów rozporządzeń:** tytuł paragrafu ma dopisany krótki temat (`REGULATION_TOPICS` w skrypcie), np. „§ 4 – kryteria obowiązkowe (arkusz A, arkusz B, arkusz C)”. Treść paragrafów odsyła do „§ 3 ust. 4”, a nie do „arkusza C”, więc bez tematu wyszukiwarka nie znajdowała ich na pytania użytkownika.
- **Tabela kryteriów w arkuszach B i C** jest czytana kolumnami, a nie wierszami. W tej tabeli „Termin sporządzenia oceny” stoi w drugiej kolumnie w wierszu „Współpraca”, więc przy czytaniu wierszami wypadał przed „Komunikacją”. Model uznawał wtedy, że kryteriów obowiązkowych są 4, a nie 5.
- **Pominięte materiały:** `Kompas Kompetencji_ Ścieżki Oceny.png` (grafika) i plik blokady `.~lock.Model_kompetencji.xlsx#`.

## Format `knowledge-base.json`

```json
{
  "sources": [{ "file": "ustawa-o-sluzbie-cywilnej-2026-poz-590.pdf", "sha256": "..." }],
  "chunks": [
    {
      "id": "ustawa-art-83",
      "source": "Ustawa o służbie cywilnej",
      "location": "art. 83",
      "title": "Ustawa o służbie cywilnej, art. 83",
      "text": "Art. 83. 1. Od oceny okresowej służy, w terminie 7 dni ..."
    }
  ]
}
```

`title` to etykieta źródła pokazywana w czacie, `text` trafia do wyszukiwarki i do promptu. Zamiast daty wygenerowania plik ma skróty SHA-256 źródeł, żeby ponowne uruchomienie dawało identyczny wynik.
