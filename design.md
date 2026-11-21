# Design i UI

## Zasady nadrzędne

1. **Najpierw komponenty Radzen Blazor.** Zanim napiszesz własny komponent, własny HTML albo CSS, sprawdź, czy Radzen ma gotowy odpowiednik (https://blazor.radzen.com). Własny kod tylko wtedy, gdy Radzen naprawdę go nie ma.
2. **Opieraj się na tym, co już jest.** Modyfikacje i nowe funkcje budujemy na istniejących rozwiązaniach i w ich stylu: te same komponenty, układy, odstępy, ikony i sposób prezentacji komunikatów. Przed dodaniem nowego widoku znajdź podobny istniejący i wzoruj się na nim.
3. **Przejrzystość ponad efekty.** Jedno główne zadanie na ekran, mało elementów, czytelne nagłówki. Główny użytkownik to przełożony, który robi ocenę raz na jakiś czas i musi się w niej odnaleźć bez instrukcji.
4. **Desktop i laptop.** Aplikacja nie jest przeznaczona na smartfony, ale układ ma się poprawnie zawijać na węższych oknach (`RadzenStack Wrap`, `max-width: 100%`).

## Motyw i kolory

- Motyw Radzen `material` (`App.razor`). Nie zmieniaj motywu i nie nadpisuj stylów komponentów Radzen bez potrzeby.
- Kolory tylko przez zmienne i klasy Radzen: `Colors.Primary`, `Colors.Warning`, `var(--rz-base-100)`, `rz-color-base-600` itd. Bez kolorów wpisanych na sztywno.
- Odstępy klasami Radzen (`rz-mb-6`, `rz-p-4`, `rz-m-0`) albo parametrem `Gap` w `RadzenStack`.

## Gotowe wzorce w projekcie

| Potrzeba | Użyj | Przykład w kodzie |
|---|---|---|
| Układ strony, nagłówek, menu | `RadzenLayout`, `RadzenHeader`, `RadzenSidebar`, `RadzenPanelMenu` | `Components/Layout/MainLayout.razor` |
| Nagłówek strony (tytuł + opis) | `<PageHeader Title="..." Subtitle="..." />` | `Components/Shared/PageHeader.razor` |
| Układ elementów | `RadzenStack` (`Orientation`, `Gap`, `AlignItems`, `JustifyContent`) zamiast własnych flexboxów | wszędzie |
| Tekst | `RadzenText` z `TextStyle` (H4 tytuł strony, H6 tytuł karty, Body1/Body2, Caption) | `Login.razor` |
| Grupowanie treści | `RadzenCard Variant="Variant.Outlined"` | `Login.razor` |
| Formularz | `RadzenTemplateForm` + `RadzenFormField Variant="Variant.Outlined"`, walidatory Radzen w `<Helper>` | `Login.razor` |
| Komunikaty na stronie | `RadzenAlert Variant="Variant.Flat" Shade="Shade.Lighter" AllowClose="false"` | `Login.razor`, `Hr/Dashboard.razor` |
| Komunikaty po akcji | `NotificationService.Notify(...)` | |
| Tabele | `RadzenDataGrid` z `AllowSorting`, `AllowFiltering`, `FilterMode.Simple`, `IsLoading` | `Przelozony/Dashboard.razor`, `Admin/Users.razor` |
| Statusy, etykiety | `RadzenBadge Variant="Variant.Flat" Shade="Shade.Lighter"` | `MainLayout.razor` |
| Przyciski | `RadzenButton`: główna akcja `ButtonStyle.Primary`, poboczna `Secondary`/`Variant.Text`; przy operacjach async `IsBusy` + `BusyText` | `Login.razor` |
| Kroki procesu (kreator oceny) | `RadzenSteps` | do zastosowania w ścieżce oceny |
| Okna dialogowe | `DialogService` | |
| Ikony | Material Symbols przez `RadzenIcon Icon="..."`; ikony ról z `AppRoles.Icon(...)` | |

## Teksty

- Cały interfejs po polsku, z polskimi znakami.
- Krótkie, konkretne etykiety. Przyciski opisują akcję („Zaloguj się”, „Rozpocznij ocenę”), nie „OK”.
- Komunikaty błędów mówią, co się stało i co zrobić („Logowanie wygasło. Spróbuj ponownie.”).
- `PageTitle` w formacie `<Nazwa strony> – Kompas kompetencji`.

## Własny CSS

- Tylko gdy Radzen nie wystarcza. Globalne klasy w `wwwroot/app.css`.
- Scoped CSS (`*.razor.css`) nie działa na wnętrzu komponentów Radzen. Dla nich używaj klas w `app.css` albo `::deep`.
