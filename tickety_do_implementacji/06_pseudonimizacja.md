# 06. Pseudonimizacja: dane pracowników nie trafiają do modelu

Źródło wymagania: `mvp.md` sekcja 5 („Bezpieczeństwo AI”). Wymaga ticketu 04.

## Cel

Przed wysłaniem rozmowy do zewnętrznego modelu imiona i nazwiska pracowników z bazy są zamieniane na pseudonimy („Pracownik A”, „Pracownik B”). Element do opisu zabezpieczeń na oddanie.

## Zakres

- Nowy `DelegatingChatClient` (np. `PseudonymizingChatClient`) w łańcuchu **za** `KnowledgeBaseChatClient`, bezpośrednio przed klientem modelu.
- Źródło nazwisk: tabela `Employees` (`Employee.FirstName`, `Employee.LastName`) przez `IDbContextFactory<AppDbContext>` albo `Organization/OrganizationService.cs`. Wczytywać przy każdym zapytaniu albo z krótkim cache.
- Zamiana w treści wiadomości użytkownika i w historii: pełne imię i nazwisko, samo nazwisko oraz samo imię (z uwagą na odmianę: „Anny Kowalskiej”, „Annie”). Na start wystarczy dopasowanie bez rozróżniania wielkości liter i polskich znaków plus najczęstsze końcówki; dokładniejszą odmianę opisać jako ograniczenie.
- W odpowiedzi modelu pseudonimy zamieniać z powrotem na prawdziwe imiona, żeby użytkownik widział naturalny tekst. Uwaga na strumieniowanie: pseudonim może przyjść w kilku kawałkach, więc trzeba buforować do granicy słowa.
- Ostrzeżenie w czacie „Nie wpisuj imion ani nazwisk pracowników.” zostaje jako dodatkowa warstwa.

## Testy

- Testy jednostkowe zamiany w obie strony (w projekcie z ticketu 05), na danych demo z `AppDbContext.HasData` (fikcyjne osoby).
- W logach `Trace` sprawdzić, że do modelu nie trafia żadne nazwisko z bazy.

## Gotowe, gdy

- Zapytanie „Jak przygotować rozmowę z Anną Kowalską?” dochodzi do modelu jako „… z Pracownikiem A”, a odpowiedź w czacie znowu zawiera „Anna Kowalska”.
- Ograniczenia (odmiana, nazwiska spoza bazy) są opisane w `do_rozwazenia.md`.
