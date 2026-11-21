# Design i UI

## Zasady nadrzędne

1. **Najpierw komponenty Radzen Blazor.** Zanim napiszesz własny komponent, własny HTML albo CSS, sprawdź, czy Radzen ma gotowy odpowiednik (https://blazor.radzen.com). Własny kod tylko wtedy, gdy Radzen naprawdę go nie ma.
2. **Opieraj się na tym, co już jest.** Modyfikacje i nowe funkcje budujemy na istniejących rozwiązaniach i w ich stylu: te same komponenty, układy, odstępy, ikony i sposób prezentacji komunikatów. Przed dodaniem nowego widoku znajdź podobny istniejący i wzoruj się na nim.
3. **Przejrzystość ponad efekty.** Jedno główne zadanie na ekran, mało elementów, czytelne nagłówki. Główny użytkownik to przełożony, który robi ocenę raz na jakiś czas i musi się w niej odnaleźć bez instrukcji.
4. **Desktop i laptop.** Aplikacja nie jest przeznaczona na smartfony, ale układ ma się poprawnie zawijać na węższych oknach (`RadzenStack Wrap`, `max-width: 100%`).

## Kierunek wizualny

Minimalistyczny dashboard SaaS (inspiracja: Crisply, Linear, Vercel). Monochromatyczny, dużo bieli i przestrzeni, cienkie szare obramowania, czerń jako jedyny mocny akcent. Kolor pojawia się tylko semantycznie (statusy, ostrzeżenia, wydarzenia) i zawsze w wersji pastelowej.

Ten wygląd uzyskujemy **przez zmienne motywu Radzen** (`--rz-*`), nadpisane w jednym miejscu na początku `wwwroot/app.css`, a nie przez własne komponenty.

### Typografia
- Font: Inter (Google Fonts), fallback `system-ui, sans-serif`, ustawiony przez `--rz-text-font-family`.
- Hierarchia budowana rozmiarem, wagą i odcieniem szarości, nie kolorem:
  - tytuł strony: `TextStyle.H4`, waga 600 (24–28px),
  - tytuł karty lub sekcji: `TextStyle.H6`, waga 600 (18px),
  - treść: `Body1`/`Body2` (14px), kolor bazowy prawie czarny,
  - opisy i podpowiedzi: `Caption` lub `Body2` w kolorze `rz-color-base-600` (12–13px),
  - etykiety grup w menu: 11px, WIELKIE LITERY, `letter-spacing: .05em`, kolor `base-500` (np. „OCENY”, „ADMINISTRACJA”),
  - duże liczby KPI: 32–36px, waga 500.
- Nagłówek typu hero (np. ekran logowania) może mieć dwie linie: pierwszą czarną, drugą szarą.

### Paleta (mapowana na zmienne Radzen)
| Rola | Wartość | Zmienna/klasa Radzen |
|---|---|---|
| Tło aplikacji, karty | `#FFFFFF` | `--rz-base-background-color`, tło kart |
| Tło subtelne (hover, aktywny item menu) | `#F7F7F8` | `--rz-base-50` / `--rz-base-100` |
| Tło wyciszone (disabled, pigułki) | `#F3F4F6` | `--rz-base-200` |
| Obramowania | `#E5E7EB`, mocniejsze `#D1D5DB` | `--rz-base-300` / `--rz-base-400` |
| Tekst główny / pomocniczy / najsłabszy | `#111827` / `#6B7280` / `#9CA3AF` | `--rz-text-color`, `rz-color-base-600`, `rz-color-base-500` |
| Akcent (główny przycisk, aktywna zakładka, przełącznik ON, aktywna strona paginacji) | `#0A0A0A` | `--rz-primary` (+ odcienie `-light`, `-lighter`, `-dark`) |
| Akcje destrukcyjne, spadki, linia „teraz” | `#B42318` na `#FEF3F2` | `Colors.Danger`, `Shade.Lighter` |
| Info / zaplanowane | `#2563EB` na `#EFF6FF` | `--rz-info`, `BadgeStyle.Info` |
| Sukces / ukończone | `#16A34A` na `#F0FDF4` | `--rz-success`, `BadgeStyle.Success` |
| Ostrzeżenie | `#A16207` na `#FEFCE8` | `--rz-warning`, `BadgeStyle.Warning` |

Pastelowe bloki (np. kategorie kompetencji, wydarzenia): żółty `#FAF8DC` z tekstem `#8A7A0B`, zielony `#EAF3EC` z tekstem `#3F7A4F`. Bez gradientów, jaskrawych kolorów i mocnych cieni.

### Kształty, odstępy, cienie
- Zaokrąglenia: karty 12px, przyciski i pola 8px, badge w kształcie pigułki (`--rz-border-radius` i zmienne konkretnych komponentów).
- Zamiast cieni obramowanie 1px (`RadzenCard Variant="Variant.Outlined"`). Cień tylko na tooltipach, menu i dialogach, i to delikatny: `0 1px 2px rgba(0,0,0,.04), 0 4px 16px rgba(0,0,0,.06)`.
- Siatka 4/8px. Padding karty 24px (`rz-p-6`), odstęp między kartami 24px (`Gap="1.5rem"`), między sekcjami formularza 32px.
- Nagłówek sekcji oddziela cienka linia 1px (`<hr>` z `--rz-base-300` albo `RadzenStack` z obramowaniem dolnym).
- Przejścia 150ms na tło, obramowanie i kolor. Najechanie na wiersz lub pozycję menu zmienia tło na subtelne.

### Układ i wzorce wizualne
- **Sidebar** (~240px, białe tło, prawa krawędź 1px): pozycje to ikona i etykieta 14px, aktywna ma subtelne tło i zaokrąglenie 8px, grupy oddzielone etykietą w WIELKICH LITERACH. Na dole panel użytkownika (avatar, imię, rola).
- **Header** (~64px, dolna krawędź 1px): po lewej opcjonalne wyszukiwanie, po prawej pomoc i menu użytkownika (`RadzenProfileMenu`).
- **Tytuł strony** w jednej linii z zakładkami (`RadzenTabs`), jeśli widok je ma. Aktywna zakładka ma czarny tekst i czarne podkreślenie 2px.
- **Przyciski**: główny czarny z białym tekstem (`ButtonStyle.Primary`), poboczny biały z szarym obramowaniem (`Variant.Outlined`, `ButtonStyle.Base`), destrukcyjny jako `ButtonStyle.Danger` z `Variant.Outlined` lub `Variant.Text`. Wysokość 36px.
- **Pola formularza**: `Variant.Outlined`, wysokość 36–40px, etykieta nad polem, pole zablokowane na tle `base-200`. Fokus to wyraźny, ciemny ring.
- **Tabele** (`RadzenDataGrid`): nagłówek 13px w kolorze pomocniczym, cienkie linie wierszy, subtelne tło przy najechaniu, statusy jako badge z pastelowym tłem. Pod tabelą wybór liczby wierszy i paginacja, w której aktywna strona to czarny kwadrat.
- **Listy elementów** (np. oceny do wykonania): każdy element w osobnej karcie z obramowaniem, tytuł 15px/500, opis w kolorze pomocniczym z ucięciem (`text-overflow: ellipsis`), data po prawej, akcje (`RadzenSplitButton` / menu „⋮”) po prawej na dole.
- **KPI i wykresy** (`RadzenChart`): duża liczba z badge zmiany (np. czerwone „↘ 12%”) i podpisem pod spodem. Słupki jasnoszare, wybrany lub najechany słupek czarny, przerywana siatka, tooltip jako biała karta z cieniem.
- **Wiersz ustawień**: tytuł (14px/500) i opis w kolorze pomocniczym po lewej, akcja (`RadzenSwitch`, przycisk) wyrównana do prawej.
- **Ikony**: Material Symbols w wariancie outlined, jednolita grubość, 18–20px.

## Motyw i kolory

- Motyw Radzen `material` (`App.razor`). Nie zmieniaj motywu. Kierunek wizualny realizujemy wyłącznie przez nadpisanie zmiennych `--rz-*` w jednym bloku `:root` na początku `wwwroot/app.css`. Stylów pojedynczych komponentów Radzen nie nadpisuj bez potrzeby.
- Kolory tylko przez zmienne i klasy Radzen: `Colors.Primary`, `Colors.Warning`, `var(--rz-base-100)`, `rz-color-base-600` itd. Wartości hex z tabeli palety wpisujemy wyłącznie w tym bloku `:root`, a nigdzie indziej.
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
