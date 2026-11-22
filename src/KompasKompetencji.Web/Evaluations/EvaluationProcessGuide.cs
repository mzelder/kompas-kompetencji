namespace KompasKompetencji.Web.Evaluations;

public sealed record StageGuide(EvaluationStage Stage, string When, string Who, string Icon, IReadOnlyList<string> Steps);

public static class EvaluationProcessGuide
{
    public static IReadOnlyList<StageGuide> Stages { get; } =
    [
        new(EvaluationStage.JobDescriptionReview, "na początku cyklu", "Ty", "description",
        [
            "Przeczytaj opis stanowiska pracownika.",
            "Zatwierdź arkusz oceny: B dla osób kierujących zespołem, C dla pozostałych. Aplikacja podpowie właściwy.",
            "Bez zatwierdzonego arkusza nie wyznaczysz rozmów.",
        ]),
        new(EvaluationStage.StartingConversation, "w ciągu 30 dni od początku cyklu", "Ty i pracownik", "forum",
        [
            "Omówcie główne obowiązki pracownika.",
            "Wybierzcie kryteria oceny: 5 obowiązkowych i do 3 dodatkowych.",
            "Ustalcie, czego oczekujesz. Pracownik potwierdza, że to zna.",
        ]),
        new(EvaluationStage.Assessment, "przez cały okres oceny, do terminu oceny", "Ty i pracownik", "visibility",
        [
            "Obserwuj pracę i zapisuj konkretne przykłady w notatkach.",
            "Na bieżąco mów pracownikowi, co idzie dobrze, a co poprawić.",
            "Około 2 miesiące przed terminem oceny pracownik pojawi się na Twoim pulpicie. Wyznacz rozmowę oceniającą, najpóźniej na dzień oceny. Zobaczysz ją w kalendarzu.",
            "Na rozmowie porozmawiajcie o wynikach pracy, rozwoju i szkoleniach.",
            "Oceń każde kryterium punktami: 1, 3, 5, 7 albo 9. Aplikacja policzy średnią i podpowie poziom, a asystent AI pomoże w zasadach. Decyzja należy do Ciebie.",
            "Ocena jest negatywna, gdy średnia to 4 pkt lub mniej albo któreś kryterium ma 1 pkt.",
            "Dopisz uzasadnienie i wnioski do rozwoju pracownika.",
        ]),
        new(EvaluationStage.Completion, "po rozmowie oceniającej", "Ty i pracownik", "task_alt",
        [
            "Wpisz datę sporządzenia oceny i pobierz arkusz w PDF. Podpisz go.",
            "Pokaż pracownikowi ocenę. Pracownik podpisuje arkusz, że się z nią zapoznał.",
            "Powiedz, że w ciągu 7 dni może złożyć sprzeciw.",
            "Wpisz datę zapoznania pracownika z oceną i zamknij ocenę.",
            "Od daty sporządzenia oceny zaczyna się nowy cykl. Kolejna ocena za 24 miesiące.",
        ]),
    ];
}
