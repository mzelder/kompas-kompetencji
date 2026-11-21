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
        ConversationStatus.AcceptedByEmployee => "Zaakceptowana przez pracownika",
        ConversationStatus.Held => "Odbyta",
        _ => status.ToString(),
    };

    public static string Sheet(EvaluationSheet sheet) => $"Arkusz {sheet}";

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
