namespace KompasKompetencji.Web.Evaluations;

public sealed record StageGuide(
    EvaluationStage Stage,
    string When,
    string Who,
    string Icon,
    IReadOnlyList<string> ProcedureSteps,
    string MergeNote,
    IReadOnlyList<string> Steps,
    string EndsWith);

public sealed record ProcessNote(string Title, string When, string Who, string Icon, IReadOnlyList<string> Steps);

public static class EvaluationProcessGuide
{
    public static string CycleSummary =>
        $"Ocena okresowa odbywa się co {EvaluationSchedule.CycleMonths} miesiące. Cykl zaczyna się w dniu zatrudnienia albo w dniu sporządzenia poprzedniej oceny, " +
        $"a termin oceny wypada {EvaluationSchedule.CycleMonths} miesiące później. Całą ocenę prowadzisz na stronie pracownika w zakładce „Moi pracownicy”.";

    public static string MappingSummary =>
        "Rozporządzenie opisuje przygotowanie i pięć etapów oceny (I–V). W aplikacji są cztery etapy, od 0 do 3, i każdy kończy się jedną Twoją czynnością. " +
        "Ocenianie, rozmowa oceniająca i ocena na piśmie (etapy II–IV) są połączone w jeden Etap 2 „Ocena pracownika”. Przy każdym etapie widzisz, które etapy procedury obejmuje.";

    public static IReadOnlyList<StageGuide> Stages { get; } =
    [
        new(EvaluationStage.JobDescriptionReview, "na początku cyklu", "Ty", "description",
        ["Przygotowanie"],
        "W procedurze to część przygotowania do rozmowy rozpoczynającej. W aplikacji jest osobnym etapem, bo od arkusza zależą kryteria obowiązkowe omawiane na rozmowie, a bez arkusza nie wyznaczysz żadnej rozmowy.",
        [
            "Na początku cyklu pracownik pojawi się na Twojej liście „Do zrobienia” z przyciskiem „Zatwierdź arkusz”.",
            "Przeczytaj opis stanowiska pracy (OSP): cel, zadania i wymagane kompetencje.",
            "Zatwierdź arkusz oceny: B dla osób kierujących zespołem, C dla pozostałych. Aplikacja podpowie właściwy, ale wybór należy do Ciebie.",
        ],
        "„Zatwierdź arkusz”."),
        new(EvaluationStage.StartingConversation, $"w ciągu {StartingConversationRules.DaysFromCycleStart} dni od początku cyklu", "Ty i pracownik", "forum",
        ["I. Rozmowa rozpoczynająca"],
        "Odpowiada etapowi I procedury. Przygotowanie propozycji, rozmowa i zatwierdzenie części I to kroki wewnątrz jednego etapu.",
        [
            "Wyznacz termin rozmowy rozpoczynającej. Zobaczysz ją w kalendarzu rozmów.",
            "Przed rozmową przygotuj propozycję części I arkusza: 5 kryteriów obowiązkowych wynika z arkusza, a do 3 dodatkowych wybierasz sam. Aplikacja podpowie je na podstawie opisu stanowiska.",
            "Dopisz oczekiwania wobec pracownika i ewentualnie cele. Propozycję możesz zapisać i poprawiać.",
            "Na rozmowie omówcie obowiązki, kryteria i oczekiwania. Po rozmowie oznacz ją jako odbytą.",
            "Zatwierdź część I z datą zapoznania pracownika. Data zastępuje podpis pracownika, od niej liczy się okres oceny, a część I staje się tylko do odczytu.",
        ],
        "„Zatwierdź część I” po odbytej rozmowie, z datą zapoznania pracownika."),
        new(EvaluationStage.Assessment, "od zatwierdzenia części I do terminu oceny", "Ty i pracownik", "visibility",
        ["II. Ocenianie", "III. Rozmowa oceniająca", "IV. Ocena na piśmie"],
        "W procedurze to trzy osobne etapy. W aplikacji są jednym, bo wszystkie robisz Ty, na tej samej stronie pracownika i w tym samym okresie. Rozmowa oceniająca ma własny termin, więc jest kartą w tym etapie, a nie osobnym krokiem na pasku.",
        [
            "Ocenianie: przez cały okres zapisuj w notatkach konkretne sytuacje, co pracownik zrobił i z jakim skutkiem. Asystent AI przypisze notatkę do kryterium i oznaczy, czy jest pozytywna, neutralna czy negatywna. Zawsze możesz to poprawić.",
            "Na bieżąco mów pracownikowi, co idzie dobrze, a co poprawić.",
            "Rozmowa oceniająca: około 2 miesiące przed terminem oceny pracownik pojawi się na liście „Do zrobienia”. Wyznacz rozmowę w karcie „Rozmowa oceniająca” nad notatkami, najpóźniej na dzień oceny.",
            "Na rozmowie porozmawiajcie o wynikach pracy, rozwoju i szkoleniach. Po rozmowie oznacz ją jako odbytą: wtedy odblokuje się ocena kryteriów.",
            "Ocena na piśmie: przy każdym kryterium wybierz dla każdego wskaźnika opis zachowania, który pasuje do tego, co obserwowałeś, albo zaznacz brak podstaw do oceny. Przycisk „Notatki” pokaże notatki do kryterium, a asystent AI może podpowiedzieć opisy na ich podstawie.",
            "Poziom kryterium wylicza się sam z Twoich ocen wskaźników: to średnia z ocenionych wskaźników, zaokrąglona do najbliższego poziomu (połówka w górę). Nie zmienia się go ręcznie.",
            "Gdy ocenisz wskaźniki wszystkich kryteriów, zobaczysz średnią, poziom ogólny i wynik. Ocena jest negatywna, gdy średnia to 4 pkt lub mniej albo któreś kryterium ma 1 pkt.",
        ],
        "„Przejdź do zakończenia”, gdy wszystkie kryteria mają poziom."),
        new(EvaluationStage.Completion, "niezwłocznie po ocenie na piśmie", "Ty i pracownik", "task_alt",
        ["V. Zakończenie"],
        "Odpowiada etapowi V procedury. Ma dwa kroki, bo Twój podpis i podpis pracownika nie muszą być tego samego dnia.",
        [
            "Wpisz datę sporządzenia oceny i ją zapisz: nie wcześniej niż odbyta rozmowa oceniająca i nie później niż dziś. Do zamknięcia oceny możesz ją poprawić.",
            "Pobierz arkusz w PDF i podpisz go.",
            "Pokaż pracownikowi ocenę. Pracownik podpisuje arkusz, że się z nią zapoznał.",
            "Powiedz, że w ciągu 7 dni może złożyć sprzeciw do dyrektora generalnego. Pouczenie jest w arkuszu PDF.",
            "Wpisz datę zapoznania pracownika z oceną i zamknij ocenę. Zakończoną ocenę i jej arkusz znajdziesz w zakładce „Oceny”.",
        ],
        $"„Zamknij ocenę” z datą zapoznania pracownika. Od daty sporządzenia zaczyna się nowy cykl, a pracownik wraca do Etapu 0. Kolejna ocena za {EvaluationSchedule.CycleMonths} miesiące."),
    ];

    public static ProcessNote AfterCompletion { get; } = new(
        "Sprzeciw", "po zamknięciu oceny, poza aplikacją", "Pracownik i dyrektor generalny", "gavel",
        [
            "Pracownik może w ciągu 7 dni od zapoznania się z oceną złożyć sprzeciw do dyrektora generalnego, który rozpatruje go w ciągu 14 dni.",
            "Sprzeciwu nie prowadzi się w aplikacji. Nowy cykl zaczyna się niezależnie od niego.",
        ]);
}
