using KompasKompetencji.Web.Data;

namespace KompasKompetencji.Web.Evaluations;

public static class ConversationRules
{
    public static bool IsOverdue(DateOnly dueDate, DateTime now) => dueDate < DateOnly.FromDateTime(now);

    public static DateTime? LatestAllowed(DateOnly dueDate, DateTime now) =>
        IsOverdue(dueDate, now) ? null : dueDate.ToDateTime(new TimeOnly(23, 59));

    public static DateTime Suggest(DateOnly dueDate, DateTime now)
    {
        var today = DateOnly.FromDateTime(now);
        var day = IsOverdue(dueDate, now) ? today.AddDays(1)
            : dueDate < today.AddDays(7) ? dueDate
            : today.AddDays(7);
        var suggestion = day.ToDateTime(new TimeOnly(10, 0));
        return suggestion < now ? now.Date.AddDays(1).AddHours(10) : suggestion;
    }

    public static bool IsPastDue(DateTime? conversationAt, ConversationStatus? status, DateTime now) =>
        conversationAt is DateTime at && at < now && status != ConversationStatus.Held;

    public static ConversationStatus StatusForNewDate(DateTime newAt, DateTime now) =>
        newAt < now ? ConversationStatus.Held : ConversationStatus.Scheduled;

    public static bool CanBeScheduled(DateTime conversationAt, DateTime? currentAt, DateTime now) =>
        conversationAt >= now || conversationAt == currentAt;

    public static bool CanBeHeld(DateTime conversationAt, DateTime now) => conversationAt <= now;

    public static string? Validate(DateTime newAt, DateTime? currentAt, ConversationStatus status, DateOnly dueDate, DateTime now)
    {
        if (!IsOverdue(dueDate, now) && DateOnly.FromDateTime(newAt) > dueDate)
        {
            return $"Rozmowa musi się odbyć najpóźniej w dniu oceny ({EvaluationText.Date(dueDate)}).";
        }
        if (status == ConversationStatus.Held && !CanBeHeld(newAt, now))
        {
            return "Rozmowę można oznaczyć jako odbytą dopiero po jej terminie.";
        }
        if (status == ConversationStatus.Scheduled && !CanBeScheduled(newAt, currentAt, now))
        {
            return "Termin w przeszłości: oznacz rozmowę jako odbytą albo wybierz przyszły termin.";
        }
        return null;
    }
}
