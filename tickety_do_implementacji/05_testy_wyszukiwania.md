# 05. Projekt testów i zestaw pytań

Decyzja: D12 (`README.md` w tym folderze). Robić razem z ticketem 03.

## Cel

Automatycznie sprawdzać, czy wyszukiwarka BM25 znajduje właściwe fragmenty dla pytań w stylu jury. Testy działają bez wywołań API, w sekundach i za darmo.

## Projekt

- `tests/KompasKompetencji.Tests/` (xUnit, `net11.0`), referencja do `src/KompasKompetencji.Web`, dodany do `KompasKompetencji.slnx`.
- Plik `knowledge-base.json` dostępny w testach tak samo jak w aplikacji (Content z linkiem w `.csproj` testów).
- Komenda w `README.md` i `CLAUDE.md`: `dotnet test`.

## Zestaw pytań

Plik `tests/KompasKompetencji.Tests/retrieval-questions.json`, ok. 20 pozycji:

```json
[
  {
    "question": "Ile dni ma pracownik na sprzeciw od oceny okresowej?",
    "expectedChunkIds": ["ustawa-art-83"]
  }
]
```

- `expectedChunkIds`: fragmenty, z których co najmniej jeden musi być w pierwszej piątce wyników.
- Pytania przygotowuje Claude na podstawie dokumentów; **użytkownik przegląda, czy brzmią jak pytania jury**.
- Pokrycie tematów (propozycja):
  - termin i częstotliwość oceny okresowej (24 miesiące), kto ją sporządza;
  - sprzeciw: termin, do kogo, kto rozpatruje;
  - kryteria obowiązkowe i dodatkowe w arkuszu C oraz B, ile dodatkowych;
  - skala ocen i znaczenie poziomów;
  - ocena negatywna i jej skutki, ponowna ocena;
  - IPRZ (indywidualny program rozwoju zawodowego);
  - pierwsza ocena: kogo dotyczy, kiedy, skala, czym różni się od okresowej;
  - rozmowa oceniająca;
  - kompetencje z modelu (np. „co oznacza kompetencja współpraca”);
  - OSP (np. „jakie kompetencje są w opisie stanowiska starszego specjalisty”).
- Kilka pytań sformułowanych innymi słowami niż przepisy („nie zgadzam się z oceną, co mogę zrobić?”), żeby sprawdzać synonimy.
- Kilka pytań w odmianie („sprzeciwu”, „ocenie okresowej”), żeby sprawdzać brak stemmera.

## Testy

1. **Trafność wyszukiwania:**
   - dla każdego pytania z zestawu oczekiwany fragment jest w pierwszej piątce (`[Theory]` z danymi z JSON, żeby było widać, które pytanie nie przechodzi);
   - dodatkowo test zbiorczy wypisujący odsetek trafień (recall@5).
2. **Normalizacja:** „Sprzeciw”, „SPRZECIW”, „sprzeciw” i wersja bez polskich znaków dają ten sam wynik.
3. **Odmowa przy poziomie oceny** (ticket 04): `EvaluationLevelGuard` rozpoznaje pytania o wystawienie poziomu i nie reaguje na pytania o zasady skali.
4. **Wczytanie bazy:** każdy fragment ma niepuste `Id`, `Title` i `Text`, a `Id` są unikalne.

## Gotowe, gdy

- `dotnet test` przechodzi.
- Pytania, które nie przechodzą mimo poprawek, są opisane w `do_rozwazenia.md` z pomysłem na rozwiązanie (synonimy, stemmer, embeddingi).
