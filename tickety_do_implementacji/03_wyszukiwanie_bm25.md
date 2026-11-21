# 03. Wczytanie bazy i wyszukiwarka BM25

Decyzje: D2, D10, D11 (`README.md` w tym folderze). Wymaga ticketu 02. Robić razem z ticketem 05.

## Cel

Aplikacja wczytuje `knowledge-base/knowledge-base.json` przy starcie i dla zapytania zwraca najlepiej pasujące fragmenty.

## Dostęp do pliku

- W `KompasKompetencji.Web.csproj` dodać plik jako Content z linkiem, kopiowany do wyjścia:
  `<Content Include="..\..\knowledge-base\knowledge-base.json" Link="Assistant\knowledge-base.json" CopyToOutputDirectory="PreserveNewest" />`
- Ścieżkę dobrać tak, żeby działała przy `dotnet run`, przy F5 w Visual Studio i w Dockerze (`docker-compose.yml`). Sprawdzić, czy kontekst budowania Dockera obejmuje `knowledge-base/`.

## Klasy (propozycja, w `Assistant/KnowledgeBase/`)

- `KnowledgeChunk`: rekord `Id`, `Source`, `Location`, `Title`, `Text`.
- `KnowledgeBase`: wczytuje JSON raz (singleton) i udostępnia listę fragmentów.
- `TextNormalizer`:
  - małe litery, bez polskich znaków (jak `Normalize` w `MockChatClient` i `Organization/EmployeeSearch.cs`; rozważyć wspólną klasę zamiast trzeciej kopii);
  - podział na słowa;
  - usunięcie słów pomijalnych (lista: i, w, na, z, do, o, się, że, jest, nie, to, od, po, przez, dla, jak, czy, co, ile, jaki, jaka, jakie…).
- `Synonyms`: krótka lista rozwijająca zapytanie, np. odwołanie / odwołać → sprzeciw, IPRZ → indywidualny program rozwoju zawodowego, arkusz C → niekierujący zespołem. Listę poszerzać na podstawie testów z ticketu 05.
- `Bm25Retriever`:
  - klasyczne BM25 (k1 = 1.2, b = 0.75), indeks budowany raz przy starcie;
  - `Search(string query, int count = 5)` zwraca fragmenty z wynikiem malejąco;
  - indeksować `Title` i `Text` (tytuł zawiera np. „art. 83”, co pomaga przy pytaniach o numer przepisu).
- Rejestracja w `Program.cs` jako singletony.

## Zapytanie z rozmowy (D10)

Tekst do wyszukiwania = dwa ostatnie pytania użytkownika z historii (`ChatRole.User`), połączone. Dzięki temu dopytanie „a ile to dni?” korzysta z kontekstu poprzedniego pytania.

## Gotowe, gdy

- Aplikacja startuje i wczytuje bazę (błąd przy braku pliku ma czytelny komunikat w logu).
- Testy z ticketu 05 przechodzą.
