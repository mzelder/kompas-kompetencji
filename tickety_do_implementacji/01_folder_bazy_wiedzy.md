# 01. Folder bazy wiedzy i źródła

Decyzje: D3, D4, D7 (`README.md` w tym folderze).

## Cel

Cała baza wiedzy w jednym folderze `knowledge-base/` w katalogu głównym repo.

## Docelowa struktura

```
knowledge-base/
  README.md                  krótki opis: co tu jest, jak przebudować bazę
  sources/                   dokumenty źródłowe (oryginały, czytelne nazwy)
    ustawa-o-sluzbie-cywilnej-2026-poz-590.pdf
    rozporzadzenie-oceny-okresowe-2016.pdf
    rozporzadzenie-pierwsza-ocena-2015.pdf
    poradnik-oceny-okresowe.pdf
    poradnik-pierwsza-ocena-2016.pdf
    arkusz-oceny-b.docx
    arkusz-oceny-c.docx
    arkusz-pierwszej-oceny.docx
    model-kompetencji.xlsx
    osp/                     przykładowe opisy stanowisk pracy z OSP_word.zip
  build_knowledge_base.py    skrypt (ticket 02)
  knowledge-base.json        wynik skryptu (ticket 02)
```

## Kroki

1. Skopiować źródła z `tresci_zadania/pliki_dodatkowe/` do `knowledge-base/sources/` pod czytelnymi nazwami. `tresci_zadania/` zostaje bez zmian, bo to oryginalne materiały konkursowe.
   - **Wyjątek:** ustawę pobraliśmy sami do `tresci_zadania/pliki_dodatkowe/Ustawa o służbie cywilnej/`. Przenieść ją do `knowledge-base/sources/` i usunąć tamten folder, bo nie należy do materiałów konkursowych.
2. **Duplikat rozporządzenia 2016:** są dwa pliki o różnych sumach MD5:
   - `Ocena okresowa w służbie cywilnej/Rozporządzenie o ocenach okresowych z 2016 r. (3).pdf`
   - `Kompetencje wspierające analizę OSP + przykładowe OSP/Rozporządzenie o ocenach okresowych z 2016 r. (4).pdf`

   Porównać treść, np. `pdftotext` i `diff`. Jeśli różnią się tylko metadanymi, zostawić jeden. Jeśli treścią (np. inna wersja albo załączniki), zapytać użytkownika, którą wersję brać.
3. **OSP:** rozpakować `OSP_word.zip` (3 pliki `.docx`) do `sources/osp/`. **Przed dodaniem przejrzeć je pod kątem prawdziwych danych osobowych** (imiona, nazwiska, e-maile, telefony). Zasada z `CLAUDE.md`: nie dodajemy prawdziwych danych osobowych. Jeśli są, zapytać użytkownika, czy je usunąć z tekstu w skrypcie (ticket 02), czy pominąć dany plik.
4. Pominąć `Kompas Kompetencji_ Ścieżki Oceny.png` (grafika, nie tekst) i plik blokady `.~lock.Model_kompetencji.xlsx#`.
5. Napisać `knowledge-base/README.md`: lista źródeł z pochodzeniem (dla ustawy: Dz.U. 2026 poz. 590, adres API Sejmu, data sprawdzenia 2026-11-21) i komenda przebudowy bazy.
6. Dopisać `knowledge-base/` do `CLAUDE.md` (sekcja o asystencie AI) i ewentualnie do `README.md`.

## Gotowe, gdy

- `knowledge-base/sources/` zawiera wszystkie źródła z D3 i nic poza nimi.
- Decyzja w sprawie duplikatu rozporządzenia i danych w OSP jest podjęta i zapisana w `knowledge-base/README.md`.
