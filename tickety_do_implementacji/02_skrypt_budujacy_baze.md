# 02. Skrypt budujący `knowledge-base.json`

Decyzje: D6, D8 (`README.md` w tym folderze). Wymaga ticketu 01.

## Cel

`knowledge-base/build_knowledge_base.py` czyta `knowledge-base/sources/`, wyciąga tekst, czyści go, dzieli na fragmenty i zapisuje `knowledge-base/knowledge-base.json`.

## Narzędzia dostępne na maszynie (sprawdzone 2026-11-21)

- Python 3.14 z modułem `fitz` (PyMuPDF) do PDF.
- `pdftotext` (Git for Windows, `/mingw64/bin/pdftotext`) jako alternatywa. Opcja `-enc UTF-8` jest konieczna dla polskich znaków.
- **Brak** `python-docx`, `openpyxl` i `pypdf`. DOCX i XLSX czytać standardową biblioteką: `zipfile` + `xml.etree` (`word/document.xml`; `xl/sharedStrings.xml` + `xl/worksheets/sheet*.xml`). Bez instalowania pakietów.
- Skrypt bez komentarzy (zasada projektu), nazwy funkcji po angielsku.

## Format wyniku

```json
{
  "generatedAt": "2026-11-21",
  "chunks": [
    {
      "id": "ustawa-art-83",
      "source": "Ustawa o służbie cywilnej",
      "location": "art. 83",
      "title": "Ustawa o służbie cywilnej, art. 83",
      "text": "1. Od oceny okresowej służy, w terminie 7 dni od dnia zapoznania się z oceną, sprzeciw ..."
    }
  ]
}
```

- `title` to gotowa etykieta do chipa źródła w czacie (`CitationAnnotation.Title`).
- `text` to treść fragmentu, z której liczy się BM25 i która trafia do promptu.
- JSON w UTF-8, z wcięciami, żeby dało się go czytać w diffach.

## Czyszczenie tekstu

- **Ustawa** (Dziennik Ustaw): usunąć nagłówki i stopki stron („Dziennik Ustaw – N – Poz. 590”), numery stron i przypisy. Scalić słowa łamane myślnikiem na końcu linii („ogłosze-/nia”). Pominąć obwieszczenie na początku i zacząć od tekstu ustawy.
- **Rozporządzenia i poradniki:** te same reguły; sprawdzić ręcznie, jakie nagłówki się powtarzają.
- Zbędne spacje i puste linie ujednolicić.

## Podział na fragmenty

| Dokument | Fragment | `location` |
|---|---|---|
| Ustawa | Na start tylko przepisy potrzebne przełożonemu: rozdział o ocenach okresowych (art. 81–83), przepisy o pierwszej ocenie i służbie przygotowawczej (sprawdzić numery artykułów w tekście) oraz definicje z art. 1–3. Fragment = jeden artykuł; artykuł dłuższy niż ok. 1500 znaków dzielić po ustępach | `art. 83`, `art. 81 ust. 1–4` |
| Rozporządzenia | Jeden paragraf; załączniki (wzory arkuszy, skale) osobno | `§ 3`, `załącznik nr 1` |
| Poradniki | Rozdział lub podrozdział według nagłówków; długie dzielić do ok. 1500 znaków po akapitach | tytuł rozdziału, ewentualnie strona |
| Arkusze B, C, pierwszej oceny | Część arkusza (np. Część I – kryteria) | `Część I` |
| Model kompetencji | Jedna kompetencja: nazwa, definicja, wskaźniki behawioralne i poziomy. Najpierw przejrzeć strukturę arkuszy w xlsx | nazwa kompetencji |
| OSP | Sekcja opisu stanowiska (cel, zadania, wymagania, kompetencje); w tytule stanowisko, bez danych osobowych (ticket 01) | `OSP: starszy specjalista, zadania` |

- Fragment krótszy niż ok. 100 znaków dołączyć do sąsiedniego.
- Każdy fragment musi mieć niepusty `text` i unikalne `id`.

## Weryfikacja

1. Uruchomić skrypt i przejrzeć `knowledge-base.json`.
2. Sprawdzić ręcznie kluczowe fragmenty:
   - art. 81 (termin 24 miesięcy, ocenę sporządza bezpośredni przełożony);
   - art. 83 (sprzeciw, 7 dni);
   - kryteria obowiązkowe arkusza C;
   - skala pierwszej oceny.

   Każdy ma być jednym czytelnym fragmentem, bez śmieci ze stopek.
3. Wypisać statystyki: liczbę fragmentów na dokument i szacunkową liczbę tokenów całej bazy (np. znaki / 3,5). Liczba tokenów jest potrzebna do decyzji, czy warto kiedyś rozważyć wariant „cała baza w kontekście z prompt cachingiem” (Haiku 5.5 drożeje 5 razy powyżej 100 tys. tokenów w zapytaniu).
4. Dopisać do `knowledge-base/README.md` komendę: `python knowledge-base/build_knowledge_base.py`.

## Gotowe, gdy

- `knowledge-base.json` jest w repo, a kluczowe fragmenty z punktu 2 są poprawne.
- Skrypt uruchomiony ponownie daje ten sam wynik (deterministyczny).
