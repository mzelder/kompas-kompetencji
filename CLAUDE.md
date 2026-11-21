# Kompas kompetencji

Prototyp aplikacji wspierającej przełożonego w ocenie pracownika służby cywilnej (hackathon, wyzwanie Ministerstwa Cyfryzacji). Obecny priorytet: **ścieżka oceny okresowej** (arkusz C, potem B).

**To projekt czysto demonstracyjny i nigdy nie zostanie wdrożony.** Ewentualne wdrożenie to osobna wersja budowana od zera w innych technologiach. Wynika z tego:
- Nie kieruj się wymaganiami produkcyjnymi (utwardzanie bezpieczeństwa, skalowanie, konfiguracja wdrożenia, kompatybilność wstecz). Wystarczy, że działa poprawnie na demo.
- Nie dodawaj w UI informacji o prototypie, ograniczeniach czy przyszłym wdrożeniu.

- Treść zadania i wymagania: `podsumowanie_rozwiazania.md`, `tresci_zadania/Pełna treść zadania konkursowego.pdf`
- Zakres MVP i kolejność prac: `mvp.md` (stack z sekcji 7 jest nieaktualny, obowiązuje ten poniżej)
- Uruchomienie krok po kroku: `README.md`

## Zasady UI i stylu

@design.md

## Stack

Blazor Web App (Interactive Server), .NET 11, EF Core 11, SQL Server (LocalDB), Radzen Blazor 13 (motyw `standard-dark`).

## Komendy

```powershell
dotnet build src/KompasKompetencji.Web
dotnet run --project src/KompasKompetencji.Web --launch-profile http     # http://localhost:5166
dotnet tool restore                                                       # raz, instaluje dotnet-ef
dotnet ef migrations add <Nazwa> --project src/KompasKompetencji.Web --output-dir Data/Migrations
```

W Development migracje i konta testowe tworzą się same przy starcie (`Program.cs`).

## Konwencje

- Kod, nazwy plików, klas i tabel **po angielsku**. Teksty widoczne dla użytkownika **po polsku**.
- **Nie dodawaj komentarzy w kodzie** (ani `//`, ani `///`, ani `@* *@`, ani `/* */` w CSS) i nie zastanawiaj się, jakie komentarze dodać. Kod ma się tłumaczyć nazwami.
- Dane pracowników są fikcyjne. Nie dodawaj prawdziwych danych osobowych.
- AI nigdy nie ustala poziomu oceny. Sugerowany poziom liczy jawna, deterministyczna reguła, decyzję podejmuje przełożony.

## Architektura

### Logowanie i role
- Role: `Auth/AppRoles.cs` (`Przelozony`, `HR`, `Admin`). Tam też nazwa wyświetlana, ikona i strona startowa roli.
- Logowanie cookie. Komponent interaktywny nie może ustawić cookie, więc `Login.razor` sprawdza hasło, wydaje jednorazowy bilet (`Auth/LoginTicketStore.cs`) i robi pełne przekierowanie na `/auth/signin` (`Auth/AuthEndpoints.cs`). Wylogowanie: GET `/auth/logout` (z menu użytkownika w nagłówku).
- Konta i hasła: `Auth/UserService.cs` (hash przez `PasswordHasher`). Konta testowe: `Auth/TestAccounts.cs`, przycisk szybkiego logowania działa tylko w Development.

### Widoki per rola
- Każda rola ma folder w `Components/Pages/`: `Przelozony/`, `Hr/`, `Admin/`. `_Imports.razor` folderu nadaje `[Authorize(Roles = ...)]` wszystkim stronom w środku. **Nowa strona roli trafia do jej folderu**, bez własnego atrybutu.
- Menu boczne dla roli: `Auth/RoleNavigation.cs`. Nowa strona w menu to jedna pozycja tutaj.
- `Routes.razor` używa `AuthorizeRouteView`: niezalogowany → `/login?returnUrl=...`, zła rola → „Brak dostępu”. `/` przekierowuje do panelu roli.
- Layouty: `MainLayout` (RadzenLayout z nagłówkiem i menu) dla zalogowanych, `AuthLayout` (bez menu) dla logowania.
- Wspólne komponenty: `Components/Shared/`.

### Dane
- `Data/AppDbContext.cs`, encje w `Data/`, migracje w `Data/Migrations/`. Do bazy z komponentów przez `IDbContextFactory<AppDbContext>` (krótko żyjący kontekst na operację) albo przez serwis.
- Model organizacji:
  - `Employee`: każda osoba w urzędzie (imię, nazwisko, stanowisko, komórka organizacyjna). Flaga `IsSupervisor` oznacza, że może być czyimś przełożonym. Przełożony też jest pracownikiem i ma własnych przełożonych.
  - `EmployeeSupervisor`: powiązanie wiele-do-wielu pracownik → przełożony (klucz złożony, check constraint „nie można być swoim przełożonym”). Pracownik może mieć kilku przełożonych.
  - `AppUser.EmployeeId` (opcjonalne, unikalne): konto w systemie powiązane z osobą. Konto testowe `przelozony` jest powiązane z Ewą Wiśniewską.
- Logika struktury: `Organization/OrganizationService.cs` (odczyt i zmiany) i `Organization/OrganizationChart.cs` (migawka w pamięci: przełożeni i podwładni danej osoby, lista przełożonych możliwych do przypisania bez tworzenia cyklu).
- Dane demo (8 osób, 8 powiązań) są w `HasData` w `AppDbContext`.
