# Design i UI

## Zasady nadrzędne

1. **Najpierw komponenty Radzen Blazor.** Zanim napiszesz własny komponent, własny HTML albo CSS, sprawdź, czy Radzen ma gotowy odpowiednik (https://blazor.radzen.com). Własny kod tylko wtedy, gdy Radzen naprawdę go nie ma.
2. **Opieraj się na tym, co już jest.** Modyfikacje i nowe funkcje budujemy na istniejących rozwiązaniach i w ich stylu: te same komponenty, układy, odstępy, ikony i sposób prezentacji komunikatów. Przed dodaniem nowego widoku znajdź podobny istniejący i wzoruj się na nim.
3. **Przejrzystość ponad efekty.** Jedno główne zadanie na ekran, mało elementów, czytelne nagłówki. Główny użytkownik to przełożony, który robi ocenę raz na jakiś czas i musi się w niej odnaleźć bez instrukcji.
4. **Desktop i laptop.** Aplikacja nie jest przeznaczona na smartfony, ale układ ma się poprawnie zawijać na węższych oknach (`RadzenStack Wrap`, `max-width: 100%`).

## Motyw: Radzen Standard Dark

- **Cała aplikacja, łącznie z ekranem logowania, używa wyłącznie motywu Radzen `standard-dark`** (`<RadzenTheme Theme="@AppTheme.Name" />` w `App.razor`, nazwa motywu w `Components/AppTheme.cs`). Podgląd motywu i komponentów: https://blazor.radzen.com (przełącznik motywu „Standard Dark”).
- Wygląd bierzemy z motywu bez zmian: jego font, kolory, zaokrąglenia, cienie i rozmiary. Nie nadpisujemy zmiennych `--rz-*` ani stylów komponentów Radzen, nie dodajemy własnych fontów ani palet.
- Jedyny wyjątek: motyw ma dekoracyjną grafikę tła (kolorowe rozmyte plamy po prawej). Wyłącza ją reguła `html body { background-image: none; }` w `app.css`. Tło to jednolity kolor motywu.
- `RadzenTheme` jest renderowany **statycznie, bez `@rendermode`**. Gdyby był interaktywny, Blazor po połączeniu obwodu podmieniałby `<link>` do CSS motywu i przy każdym F5 lub przejściu z logowania strona na chwilę traciłaby style (białe tło). Motyw w `ThemeService` dla komponentów interaktywnych ustawia `Routes.razor`. `<meta name="color-scheme" content="dark">` w `App.razor` sprawia, że przed załadowaniem CSS przeglądarka maluje ciemne tło.
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
| Tabele | `RadzenDataGrid` z `AllowSorting`, `IsLoading`, `EmptyText`. Bez wiersza filtrów (`AllowFiltering`), bo zajmuje miejsce: zamiast niego jedno pole `<SearchBox>` nad tabelą | `Przelozony/Dashboard.razor`, `Hr/Dashboard.razor`, `Admin/Users.razor` |
| Lista z zaznaczaniem (klik zaznacza, ponowny klik odznacza) i panel szczegółów | `RadzenDataGrid` z `SelectionMode.Single`, `@bind-Value`, `RowSelect` + `RowClick` (Radzen sam nie odznacza po drugim kliknięciu; `RowClick` przychodzi przed `RowSelect`); szczegóły w osobnej `RadzenCard` pod listami | `Hr/Employees.razor` |
| Układ kolumn | `RadzenRow` + `RadzenColumn Size="12" SizeMD="6"` | `Hr/Employees.razor` |
| Wyszukiwanie w liście | `<SearchBox Value ValueChanged Placeholder>` nad tabelą; filtrowanie w kodzie bez rozróżniania wielkości liter i polskich znaków (`Organization/EmployeeSearch.cs`); podpis z liczbą wyników „Wyniki wyszukiwania: X z Y” | `Shared/SearchBox.razor`, `Hr/Employees.razor` |
| Widok na jeden ekran (bez przewijania strony) | Korzeń strony `<div class="fill-page">`; wiersz z listami `RadzenRow class="fill-row"`, kolumny `fill-column`, karty `fill-card`, stos `fill-stack`, tabela `fill-grid` (przewija się w środku, nagłówek zostaje). Panel na dole `details-card` ma stałą wysokość, więc listy nie skaczą przy zaznaczaniu | `Hr/Employees.razor`, `wwwroot/app.css` |
| Pary etykieta–wartość w jednym rzędzie | `<DetailItem Label Value />`; długie wartości (listy nazwisk) z `Fill="true"` skracają się wielokropkiem, pełna treść w podpowiedzi | `Shared/DetailItem.razor`, `Hr/PersonDetails.razor` |
| Nagłówek karty z akcją | Tytuł i podpis w `RadzenStack class="card-heading"` (podpis `rz-text-truncate`), przycisk z `class="no-shrink"`, żeby się nie łamał | `Hr/Employees.razor` |
| Statusy, etykiety | `RadzenBadge Variant="Variant.Flat" Shade="Shade.Lighter"` | |
| Przyciski | `RadzenButton`: główna akcja `ButtonStyle.Primary`, poboczna `ButtonStyle.Secondary` lub `ButtonStyle.Base`, mniej ważna `Variant.Text`; przy operacjach async `IsBusy` + `BusyText` | `Login.razor` |
| Kroki procesu (kreator oceny) | `RadzenSteps` | do zastosowania w ścieżce oceny |
| Okna dialogowe | `DialogService.OpenAsync<Komponent>(tytuł, parametry, new DialogOptions { Width = ... })`; komponent dialogu zamyka się `DialogService.Close(wynik)`; przyciski na dole po prawej: „Anuluj” (`Variant.Text`, `ButtonStyle.Base`) i akcja główna; potwierdzenia przez `DialogService.Confirm` | `Hr/AddEmployeeDialog.razor`, `Hr/AssignSupervisorDialog.razor` |
| Wybór wielu pozycji z listy | `RadzenListBox` z `Multiple`, `AllowFiltering`, `Template` (nazwa + podpis) | `Hr/AssignSupervisorDialog.razor` |
| Ikony | Material Symbols przez `RadzenIcon Icon="..."`; ikony ról z `AppRoles.Icon(...)`; większy rozmiar klasą `icon-md` (1.75rem, przy tytule karty) / `icon-lg` (2.5rem) / `icon-xl` (3rem) | `Login.razor` |
| Awatar osoby | `<UserAvatar Name="..." />` (inicjały w kółku: „Anna Kowalska” → AK, login → dwie pierwsze litery) | `Components/Shared/UserAvatar.razor` |

## Teksty

- Cały interfejs po polsku, z polskimi znakami.
- Krótkie, konkretne etykiety. Przyciski opisują akcję („Zaloguj się”, „Rozpocznij ocenę”), nie „OK”.
- Komunikaty błędów mówią, co się stało i co zrobić („Logowanie wygasło. Spróbuj ponownie.”).
- `PageTitle` w formacie `<Nazwa strony> – Kompas kompetencji`.

## Własny CSS

- Tylko na układ, którego Radzen nie zapewnia (szerokości, wypełnienia kontenerów). Wszystko w `wwwroot/app.css`, kolory wyłącznie przez zmienne Radzen.
- Scoped CSS (`*.razor.css`) nie działa na wnętrzu komponentów Radzen. Dla nich używaj klas w `app.css` albo `::deep`.
