namespace KompasKompetencji.Web.Evaluations;

public sealed record StageGuide(EvaluationStage Stage, string When, string Who, string Icon, IReadOnlyList<string> Steps);

public static class EvaluationProcessGuide
{
    public static string CycleSummary =>
        $"Ocena okresowa odbywa się co {EvaluationSchedule.CycleMonths} miesiące. Cykl zaczyna się w dniu zatrudnienia albo w dniu sporządzenia poprzedniej oceny, " +
        $"a termin oceny wypada {EvaluationSchedule.CycleMonths} miesiące później. Całą ocenę prowadzisz na stronie pracownika w zakładce „Moi pracownicy”.";

    public static IReadOnlyList<StageGuide> Stages { get; } =
    [
        new(EvaluationStage.JobDescriptionReview, "na początku cyklu", "Ty", "description",
        [
            "Na początku cyklu pracownik pojawi się na Twojej liście „Do zrobienia” z przyciskiem „Zatwierdź arkusz”.",
            "Przeczytaj opis stanowiska pracy (OSP): cel, zadania i wymagane kompetencje.",
            "Zatwierdź arkusz oceny: B dla osób kierujących zespołem, C dla pozostałych. Aplikacja podpowie właściwy, ale wybór należy do Ciebie.",
            "Bez zatwierdzonego arkusza nie wyznaczysz żadnej rozmowy.",
        ]),
        new(EvaluationStage.StartingConversation, $"w ciągu {StartingConversationRules.DaysFromCycleStart} dni od początku cyklu", "Ty i pracownik", "forum",
        [
            "Wyznacz termin rozmowy rozpoczynającej. Zobaczysz ją w kalendarzu rozmów.",
            "Przed rozmową przygotuj propozycję części I arkusza: 5 kryteriów obowiązkowych wynika z arkusza, a do 3 dodatkowych wybierasz sam. Aplikacja podpowie je na podstawie opisu stanowiska.",
            "Dopisz oczekiwania wobec pracownika i ewentualnie cele. Propozycję możesz zapisać i poprawiać.",
            "Na rozmowie omówcie obowiązki, kryteria i oczekiwania. Po rozmowie oznacz ją jako odbytą.",
            "Zatwierdź część I z datą zapoznania pracownika. Od tej daty liczy się okres oceny, a część I staje się tylko do odczytu.",
        ]),
        new(EvaluationStage.Assessment, "przez cały okres oceny, do terminu oceny", "Ty i pracownik", "visibility",
        [
            "Przez cały okres zapisuj w notatkach konkretne sytuacje: co pracownik zrobił i z jakim skutkiem. Asystent AI przypisze notatkę do kryterium i oznaczy, czy jest pozytywna, neutralna czy negatywna. Zawsze możesz to poprawić.",
            "Na bieżąco mów pracownikowi, co idzie dobrze, a co poprawić.",
            "Około 2 miesiące przed terminem oceny pracownik pojawi się na liście „Do zrobienia”. Wyznacz rozmowę oceniającą, najpóźniej na dzień oceny.",
            "Na rozmowie porozmawiajcie o wynikach pracy, rozwoju i szkoleniach. Po rozmowie oznacz ją jako odbytą: wtedy odblokuje się ocena kryteriów.",
            "Przy każdym kryterium wybierz dla każdego wskaźnika opis zachowania, który pasuje do tego, co obserwowałeś, albo zaznacz brak podstaw do oceny. Asystent AI może podpowiedzieć opisy na podstawie notatek.",
            "Poziom kryterium wylicza się sam z Twoich ocen wskaźników: to średnia z ocenionych wskaźników, zaokrąglona do najbliższego poziomu (połówka w górę). Nie zmienia się go ręcznie.",
            "Gdy ocenisz wskaźniki wszystkich kryteriów, zobaczysz średnią, poziom ogólny i wynik. Ocena jest negatywna, gdy średnia to 4 pkt lub mniej albo któreś kryterium ma 1 pkt. Potem przejdź do zakończenia.",
        ]),
        new(EvaluationStage.Completion, "po ocenie kryteriów, do terminu oceny", "Ty i pracownik", "task_alt",
        [
            "Wpisz datę sporządzenia oceny i pobierz arkusz w PDF. Podpisz go.",
            "Pokaż pracownikowi ocenę. Pracownik podpisuje arkusz, że się z nią zapoznał.",
            "Powiedz, że w ciągu 7 dni może złożyć sprzeciw do dyrektora generalnego.",
            "Wpisz datę zapoznania pracownika z oceną i zamknij ocenę. Zakończoną ocenę i jej arkusz znajdziesz w zakładce „Oceny”.",
            $"Od daty sporządzenia oceny zaczyna się nowy cykl. Kolejna ocena za {EvaluationSchedule.CycleMonths} miesiące.",
        ]),
    ];
}
