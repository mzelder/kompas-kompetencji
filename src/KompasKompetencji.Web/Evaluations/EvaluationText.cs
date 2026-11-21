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

    public static string ConversationTypeName(ConversationType type) => type switch
    {
        ConversationType.Starting => "rozmowa rozpoczynająca",
        ConversationType.Assessment => "rozmowa oceniająca",
        _ => type.ToString(),
    };

    public static string ConversationTypeTitle(ConversationType type)
    {
        var name = ConversationTypeName(type);
        return char.ToUpper(name[0], Polish) + name[1..];
    }

    public static string ConversationDateTitle(ConversationType type) => type switch
    {
        ConversationType.Starting => "Termin rozmowy rozpoczynającej",
        ConversationType.Assessment => "Termin rozmowy oceniającej",
        _ => type.ToString(),
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
        EvaluationStage.Assessment => "Ocenianie",
        EvaluationStage.AssessmentConversation => "Rozmowa oceniająca",
        EvaluationStage.WrittenEvaluation => "Ocena na piśmie",
        EvaluationStage.Completion => "Zakończenie",
        _ => stage.ToString(),
    };

    public static string StageNumber(EvaluationStage stage) => stage switch
    {
        EvaluationStage.JobDescriptionReview => "0",
        EvaluationStage.StartingConversation => "I",
        EvaluationStage.Assessment => "II",
        EvaluationStage.AssessmentConversation => "III",
        EvaluationStage.WrittenEvaluation => "IV",
        EvaluationStage.Completion => "V",
        _ => string.Empty,
    };

    public static string StageDescription(EvaluationStage stage) => stage switch
    {
        EvaluationStage.JobDescriptionReview => "Przejrzyj opis stanowiska pracownika i zatwierdź arkusz oceny.",
        EvaluationStage.StartingConversation => "Przygotuj propozycję kryteriów, odbądź rozmowę rozpoczynającą w ciągu 30 dni od początku cyklu, a po rozmowie zatwierdź część I arkusza.",
        EvaluationStage.Assessment => "Przez cały okres oceny obserwuj realizację kryteriów i zapisuj notatki.",
        EvaluationStage.AssessmentConversation => "Około 2 miesiące przed terminem oceny wyznacz rozmowę oceniającą, najpóźniej w dniu oceny.",
        EvaluationStage.WrittenEvaluation => "Uzupełnij część II arkusza: poziomy kryteriów, uzasadnienie i wnioski do IPRZ.",
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
