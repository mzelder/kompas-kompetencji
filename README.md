# Kompas kompetencji

Placeholder aplikacji do oceny kompetencji pracowników służby cywilnej.

**Stack:** Blazor Web App (Interactive Server), .NET 11, Entity Framework Core 11, SQL Server, [Radzen Blazor](https://blazor.radzen.com) (komponenty UI, motyw `standard-dark`).

Zakres funkcjonalny MVP opisuje [mvp.md](mvp.md).

**Konwencja:** kod, nazwy plików, klas i tabel są po angielsku. Teksty widoczne dla użytkownika (UI) są po polsku. W kodzie nie piszemy komentarzy.

## Struktura

```
KompasKompetencji.slnx              solucja
dotnet-tools.json                   narzędzia lokalne (dotnet-ef)
docker-compose.yml                  opcjonalny SQL Server w Dockerze
src/KompasKompetencji.Web/
  Program.cs                        konfiguracja aplikacji i bazy
  appsettings.json                  connection string (domyślnie LocalDB)
  Components/                       komponenty i strony Blazor
  Data/AppDbContext.cs              kontekst EF Core
  Data/Employee.cs                  encja przykładowa (dane demo)
  Data/Migrations/                  migracje bazy danych
  Assistant/                        asystent AI: klient modelu, RAG, odmowa, pseudonimizacja
tests/KompasKompetencji.Tests/      testy xUnit (wyszukiwarka, asystent)
knowledge-base/                     baza wiedzy asystenta: źródła, skrypt, knowledge-base.json
```

## Uruchomienie krok po kroku

### 1. Zainstaluj wymagane narzędzia

| Narzędzie | Skąd | Sprawdzenie |
|---|---|---|
| .NET SDK 11 | https://dotnet.microsoft.com/download | `dotnet --list-sdks` pokazuje `11.x` |
| SQL Server: **LocalDB** (zalecane na Windows) | instalator SQL Server Express lub Visual Studio (workload „ASP.NET and web development”) | `sqllocaldb info` pokazuje `MSSQLLocalDB` |
| *albo* Docker Desktop | https://www.docker.com/products/docker-desktop | `docker --version` |
| IDE (opcjonalnie) | Visual Studio 2026, Rider albo VS Code z C# Dev Kit | |

Wystarczy jedna z opcji bazy: LocalDB **albo** Docker.

### 2. Sklonuj repozytorium

```powershell
git clone <adres-repozytorium>
cd kompas-kompetencji
```

### 3. Przywróć zależności i narzędzia

```powershell
dotnet restore
dotnet tool restore
```

`dotnet tool restore` instaluje lokalnie `dotnet-ef` w wersji zgodnej z projektem (z pliku `dotnet-tools.json`).

### 4. Przygotuj bazę danych

**Opcja A: LocalDB (domyślna, nic nie trzeba konfigurować)**

Connection string w `src/KompasKompetencji.Web/appsettings.json` wskazuje już na `(localdb)\MSSQLLocalDB`. Upewnij się tylko, że instancja działa:

```powershell
sqllocaldb start MSSQLLocalDB
```

**Opcja B: SQL Server w Dockerze**

```powershell
docker compose up -d
```

Następnie ustaw connection string do kontenera (zapisywany poza repozytorium, w user secrets):

```powershell
cd src/KompasKompetencji.Web
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Default" "Server=localhost,1433;Database=KompasKompetencji;User Id=sa;Password=Kompas_Dev_123!;TrustServerCertificate=True"
cd ../..
```

### 5. Uruchom aplikację

```powershell
dotnet run --project src/KompasKompetencji.Web --launch-profile https
```

Przy pierwszym uruchomieniu w trybie Development aplikacja **sama tworzy bazę `KompasKompetencji` i stosuje migracje** (wraz z danymi demo). Nie trzeba ręcznie uruchamiać `database update`.

Jeśli przeglądarka ostrzega o certyfikacie, zaufaj certyfikatowi deweloperskiemu (jednorazowo):

```powershell
dotnet dev-certs https --trust
```

Albo uruchom bez HTTPS: `--launch-profile http`.

### 6. Otwórz w przeglądarce

- HTTPS: https://localhost:7252
- HTTP: http://localhost:5166

Na stronie startowej powinien być zielony komunikat **„Połączono z SQL Server”**, tabela Radzen (`RadzenDataGrid`, z sortowaniem i filtrowaniem) z trzema pracownikami demo oraz przycisk **„Test powiadomienia”**, który pokazuje powiadomienie Radzen. Czerwony komunikat oznacza problem z połączeniem z bazą (patrz niżej).

### Uruchomienie z Visual Studio

Otwórz `KompasKompetencji.slnx`, ustaw `KompasKompetencji.Web` jako projekt startowy i naciśnij **F5**. Baza powstanie automatycznie (krok 4 nadal obowiązuje, jeśli używasz Dockera).

## Asystent AI (Claude API)

Bez klucza asystent działa w trybie bez modelu: wyszukuje fragmenty bazy wiedzy tak samo jak z modelem i pokazuje najlepiej pasujący fragment ze źródłem. Żeby podłączyć prawdziwy model:

1. Utwórz klucz API w Claude Console: https://platform.claude.com (Settings → API keys). Doładowanie kredytów: Settings → Billing.
2. Zapisz go w user-secrets (nie trafia do repozytorium):

   ```powershell
   dotnet user-secrets set "Assistant:ApiKey" "<klucz>" --project src/KompasKompetencji.Web
   ```

3. Uruchom aplikację ponownie.

Model jest w `appsettings.json` (`Assistant:Model`, domyślnie `claude-haiku-5-5`). Połączenie idzie przez oficjalny pakiet `Anthropic` i jego `IChatClient`. W Development logi `Microsoft.Extensions.AI` (poziom `Trace` w `appsettings.Development.json`) pokazują każde zapytanie do modelu i odpowiedź, już po pseudonimizacji.

Jak działa pytanie do asystenta:

1. Pytanie o wystawienie poziomu oceny konkretnej osobie dostaje stałą odmowę, bez wywołania modelu.
2. Wyszukiwarka BM25 wybiera 5 fragmentów z `knowledge-base/knowledge-base.json` (zapytanie z dwóch ostatnich pytań użytkownika).
3. Imiona i nazwiska pracowników z bazy są zamieniane na „Pracownik A”, „Pracownik B”…
4. Model dostaje prompt systemowy z ponumerowanymi fragmentami i odpowiada, podając numery [n].
5. W odpowiedzi pseudonimy wracają na imiona i nazwiska, a pod odpowiedzią pojawiają się źródła, które model przywołał.

Baza wiedzy i jej przebudowa: [knowledge-base/README.md](knowledge-base/README.md).

## Testy

```powershell
dotnet test
```

Testy nie wołają API. Sprawdzają m.in., czy dla 24 pytań w stylu jury (`tests/KompasKompetencji.Tests/retrieval-questions.json`) właściwy fragment jest w pierwszej piątce wyników wyszukiwarki, odmowę przy pytaniach o poziom oceny i pseudonimizację. Jeśli aplikacja jest uruchomiona (np. z Visual Studio), build może nie nadpisać plików w `bin`; wtedy zatrzymaj aplikację albo użyj `dotnet test --artifacts-path <inny folder>`.

## Praca z bazą danych (migracje)

Po zmianie encji lub `AppDbContext` dodaj migrację:

```powershell
dotnet ef migrations add NazwaMigracji --project src/KompasKompetencji.Web --output-dir Data/Migrations
```

Zastosowanie migracji ręcznie (np. poza trybem Development):

```powershell
dotnet ef database update --project src/KompasKompetencji.Web
```

Wyczyszczenie bazy od zera:

```powershell
dotnet ef database drop --project src/KompasKompetencji.Web --force
```

## Rozwiązywanie problemów

| Objaw | Rozwiązanie |
|---|---|
| `A network-related or instance-specific error…` | LocalDB nie działa: `sqllocaldb start MSSQLLocalDB`; w Dockerze sprawdź `docker ps` i odczekaj kilkanaście sekund po starcie kontenera. |
| `dotnet ef` nie jest rozpoznawane | Uruchom `dotnet tool restore` w katalogu głównym repozytorium. |
| Błąd o wersji frameworka `net11.0` | Zainstaluj .NET SDK 11. |
| Port zajęty | Zmień `applicationUrl` w `src/KompasKompetencji.Web/Properties/launchSettings.json`. |

## Wdrożenie demo na Azure (branch `test`)

Układ: App Service (Linux, plan B1, WebSockety włączone) i Azure SQL Database w darmowej ofercie (serverless, auto-pauza po wyczerpaniu limitu).

Aplikacja działa w środowisku `Demo` (`appsettings.Demo.json`):
- `Demo:ResetDatabaseOnStartup`: przy każdym starcie usuwa wszystkie tabele i wgrywa migracje z danymi demo. Bazy nie usuwa, bo na Azure SQL `EnsureDeleted` skasowałby darmową bazę.
- `Demo:ShowTestAccounts`: przyciski szybkiego logowania kontami testowymi na stronie logowania.

Te same flagi są włączone w `appsettings.Development.json`.

**Automatycznie:** każdy push na branch `test` uruchamia `.github/workflows/deploy-test.yml` (testy, publikacja, wdrożenie na App Service `kompas-kompetencji-demo`). GitHub loguje się do Azure tożsamością `oidc-msi-b83b` przez OIDC (federacja tylko dla brancha `test`), bez sekretów w repozytorium. Ręcznie: zakładka Actions → „Run workflow”.

**Ręcznie, skryptami:**

1. Zainstaluj Azure CLI (`winget install Microsoft.AzureCLI`) i zaloguj się: `az login`.
2. Raz utwórz zasoby (nazwy aplikacji i serwera SQL muszą być unikalne w całym Azure):
   ```powershell
   ./deploy/azure-provision.ps1 -AppName kompas-demo-xyz -SqlServerName kompas-sql-xyz -SqlAdminPassword '<silne hasło>' -AnthropicApiKey '<klucz albo pomiń>'
   ```
3. Wdróż (także każdą kolejną wersję):
   ```powershell
   ./deploy/azure-deploy.ps1 -AppName kompas-demo-xyz
   ```
4. Aplikacja: `https://<AppName>.azurewebsites.net`. Darmowa baza usypia się po godzinie bez ruchu, więc pierwsze wejście po przerwie może trwać do minuty.

Aplikacja jest publikowana jako self-contained (`linux-x64`), więc nie zależy od tego, czy App Service ma już środowisko .NET 11. Bez klucza `Assistant__ApiKey` asystent działa na `MockChatClient`.
