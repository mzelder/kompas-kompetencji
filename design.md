# Design i UI

## Zasady nadrzędne

1. **Najpierw komponenty Radzen Blazor.** Zanim napiszesz własny komponent, własny HTML albo CSS, sprawdź, czy Radzen ma gotowy odpowiednik (https://blazor.radzen.com). Własny kod tylko wtedy, gdy Radzen naprawdę go nie ma.
2. **Opieraj się na tym, co już jest.** Modyfikacje i nowe funkcje budujemy na istniejących rozwiązaniach i w ich stylu: te same komponenty, układy, odstępy, ikony i sposób prezentacji komunikatów. Przed dodaniem nowego widoku znajdź podobny istniejący i wzoruj się na nim.
3. **Przejrzystość ponad efekty.** Jedno główne zadanie na ekran, mało elementów, czytelne nagłówki. Główny użytkownik to przełożony, który robi ocenę raz na jakiś czas i musi się w niej odnaleźć bez instrukcji.
4. **Desktop i laptop.** Aplikacja nie jest przeznaczona na smartfony, ale układ ma się poprawnie zawijać na węższych oknach (`RadzenStack Wrap`, `max-width: 100%`).

## Motyw: Radzen Standard Dark

- **Cała aplikacja, łącznie z ekranem logowania, używa wyłącznie motywu Radzen `standard-dark`** (`<RadzenTheme Theme="standard-dark" />` w `App.razor`). Podgląd motywu i komponentów: https://blazor.radzen.com (przełącznik motywu „Standard Dark”).
- Wygląd bierzemy z motywu bez zmian: jego font, kolory, zaokrąglenia, cienie i rozmiary. Nie nadpisujemy zmiennych `--rz-*` ani stylów komponentów Radzen, nie dodajemy własnych fontów ani palet.
- Kolory tylko przez parametry, zmienne i klasy Radzen: `Colors.Primary`, `Colors.Warning`, `ButtonStyle`, `BadgeStyle`, `AlertStyle`, `var(--rz-body-background-color)`, `var(--rz-primary)` itd. Bez kolorów wpisanych na sztywno.
- W motywie ciemnym skala `base-*` jest odwrócona: `base-50`…`base-400` to kolory **jasne**, `base-500`…`base-900` to **ciemne**. Dlatego:
  - tekst pomocniczy (podpisy, opisy): klasa `rz-color-base-300`,
  - tła: zmienne semantyczne (`--rz-body-background-color`, `--rz-base-background-color`), nie `--rz-base-100`/`-200`.
- Odstępy klasami Radzen (`rz-mb-6`, `rz-p-4`, `rz-m-0`) albo parametrem `Gap` w `RadzenStack`.

## Gotowe wzorce w projekcie

| Potrzeba | Użyj | Przykład w kodzie |
|---|---|---|
| Układ strony, nagłówek, menu | `RadzenLayout`, `RadzenHeader` z `RadzenProfileMenu`, `RadzenSidebar` z grupami `RadzenPanelMenu` (etykieta grupy: `RadzenText TextStyle.Overline`), pozycje i grupy w `Auth/RoleNavigation.cs` | `Components/Layout/MainLayout.razor` |
| Nagłówek strony (tytuł + opis) | `<PageHeader Title="..." Subtitle="..." />` | `Components/Shared/PageHeader.razor` |
| Komunikat na całą stronę (brak dostępu, błąd, nie znaleziono, pusty stan) | `<StatusMessage Icon="..." Title="..." Text="...">` + przycisk w środku | `Components/Shared/StatusMessage.razor`, `Pages/NotFound.razor` |
| Układ elementów | `RadzenStack` (`Orientation`, `Gap`, `AlignItems`, `JustifyContent`) zamiast własnych flexboxów | wszędzie |
| Tekst | `RadzenText` z `TextStyle` (H4 tytuł strony, H6 tytuł karty, Body1/Body2, Caption); tekst pomocniczy `rz-color-base-300` | `Login.razor`, `PageHeader.razor` |
| Grupowanie treści | `RadzenCard Variant="Variant.Outlined"`, element listy w karcie `Variant.Filled` | `Login.razor` |
| Formularz | `RadzenTemplateForm` + `RadzenFormField Variant="Variant.Outlined"`, walidatory Radzen w `<Helper>` | `Login.razor` |
| Komunikaty na stronie | `RadzenAlert Variant="Variant.Flat" Shade="Shade.Lighter" AllowClose="false"` | `Login.razor` |
| Komunikaty po akcji | `NotificationService.Notify(...)` | |
| Tabele | `RadzenDataGrid` z `AllowSorting`, `AllowFiltering`, `FilterMode.Simple`, `IsLoading`, `EmptyText` | `Przelozony/Dashboard.razor`, `Hr/Dashboard.razor`, `Admin/Users.razor` |
| Statusy, etykiety | `RadzenBadge Variant="Variant.Flat" Shade="Shade.Lighter"` | |
| Przyciski | `RadzenButton`: główna akcja `ButtonStyle.Primary`, poboczna `ButtonStyle.Secondary` lub `ButtonStyle.Base`, mniej ważna `Variant.Text`; przy operacjach async `IsBusy` + `BusyText` | `Login.razor` |
| Kroki procesu (kreator oceny) | `RadzenSteps` | do zastosowania w ścieżce oceny |
| Okna dialogowe | `DialogService` | |
| Ikony | Material Symbols przez `RadzenIcon Icon="..."`; ikony ról z `AppRoles.Icon(...)`; większy rozmiar klasą `icon-lg` / `icon-xl` | `Login.razor` |
| Awatar użytkownika | `<UserAvatar Login="..." />` (inicjały w kółku) | `Components/Shared/UserAvatar.razor` |

## Teksty

- Cały interfejs po polsku, z polskimi znakami.
- Krótkie, konkretne etykiety. Przyciski opisują akcję („Zaloguj się”, „Rozpocznij ocenę”), nie „OK”.
- Komunikaty błędów mówią, co się stało i co zrobić („Logowanie wygasło. Spróbuj ponownie.”).
- `PageTitle` w formacie `<Nazwa strony> – Kompas kompetencji`.

## Własny CSS

- Tylko na układ, którego Radzen nie zapewnia (szerokości, wypełnienia kontenerów). Wszystko w `wwwroot/app.css`, kolory wyłącznie przez zmienne Radzen.
- Scoped CSS (`*.razor.css`) nie działa na wnętrzu komponentów Radzen. Dla nich używaj klas w `app.css` albo `::deep`.
