using System.Globalization;
using KompasKompetencji.Web.Data;

namespace KompasKompetencji.Web.Evaluations;

public static class EvaluationText
{
    private static readonly CultureInfo Polish = CultureInfo.GetCultureInfo("pl-PL");

    public static string Status(EvaluationStatus? status) => status switch
    {
        null or EvaluationStatus.NotStarted => "Nierozpoczęta",
        EvaluationStatus.CriteriaSet => "Kryteria ustalone",
        EvaluationStatus.InProgress => "Ocena w toku",
        EvaluationStatus.ReadyForConversation => "Gotowa do rozmowy",
        EvaluationStatus.Completed => "Zakończona",
        _ => status.ToString()!,
    };

    public static string Conversation(ConversationStatus status) => status switch
    {
        ConversationStatus.Scheduled => "Zaplanowana",
        ConversationStatus.Held => "Odbyta",
        _ => status.ToString(),
    };

    public static string Sheet(EvaluationSheet sheet) => $"Arkusz {sheet}";

    public static string SheetDescription(EvaluationSheet sheet) => sheet switch
    {
        EvaluationSheet.B => "Dla osób kierujących zespołem: kryteria obejmują też zarządzanie ludźmi.",
        EvaluationSheet.C => "Dla osób, które nie kierują zespołem.",
        _ => sheet.ToString(),
    };

    public static string SheetSuggestionReason(Employee employee) => employee.IsSupervisor
        ? "Pracownik kieruje zespołem, dlatego podpowiadamy arkusz B."
        : "Pracownik nie kieruje zespołem, dlatego podpowiadamy arkusz C.";

    public static string Stage(EvaluationStage stage) => stage switch
    {
        EvaluationStage.JobDescriptionReview => "Opis stanowiska",
        EvaluationStage.StartingConversation => "Rozmowa rozpoczynająca",
        EvaluationStage.Assessment => "Ocena pracownika",
        EvaluationStage.Completion => "Zakończenie",
        _ => stage.ToString(),
    };

    public static string StageNumber(EvaluationStage stage) => stage switch
    {
        EvaluationStage.JobDescriptionReview => "0",
        EvaluationStage.StartingConversation => "1",
        EvaluationStage.Assessment => "2",
        EvaluationStage.Completion => "3",
        _ => string.Empty,
    };

    public static string StageDescription(EvaluationStage stage) => stage switch
    {
        EvaluationStage.JobDescriptionReview => "Przejrzyj opis stanowiska pracownika i zatwierdź arkusz oceny.",
        EvaluationStage.StartingConversation => "W ciągu 30 dni od początku cyklu przeprowadź z pracownikiem rozmowę rozpoczynającą: ustalcie 5 kryteriów obowiązkowych, do 3 dodatkowych, oczekiwania i cele.",
        EvaluationStage.Assessment => "Przez cały okres oceny zapisuj notatki. Około 2 miesiące przed terminem oceny przeprowadź rozmowę oceniającą, najpóźniej w dniu oceny, i uzupełnij część II arkusza: poziomy kryteriów, uzasadnienie i wnioski do IPRZ.",
        EvaluationStage.Completion => "Zapoznaj pracownika z oceną i zamknij cykl. Od tego dnia zaczyna się nowy cykl oceny.",
        _ => string.Empty,
    };

    public static string Date(DateOnly date) => date.ToString("d MMMM yyyy", Polish);

    public static string Date(DateTime dateTime) => dateTime.ToString("d MMMM yyyy, HH:mm", Polish);

    public static string RelativeToToday(DateOnly date, DateOnly today)
    {
        var days = date.DayNumber - today.DayNumber;
        return days switch
        {
            0 => "dziś",
            1 => "jutro",
            -1 => "wczoraj",
            > 1 => $"za {days} dni",
            _ => $"{-days} dni temu",
        };
    }
}
