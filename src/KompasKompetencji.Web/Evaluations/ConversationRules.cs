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

    public static ConversationStatus ResolveStatus(DateTime newAt, DateTime? currentAt, ConversationStatus requested) =>
        currentAt == newAt ? requested : ConversationStatus.Scheduled;

    public static string? Validate(DateTime newAt, DateTime? currentAt, ConversationStatus status, DateOnly dueDate, DateTime now)
    {
        var dateChanged = currentAt != newAt;

        if (dateChanged && newAt < now)
        {
            return "Termin rozmowy nie może być w przeszłości.";
        }
        if (!IsOverdue(dueDate, now) && DateOnly.FromDateTime(newAt) > dueDate)
        {
            return $"Rozmowa musi się odbyć najpóźniej w dniu oceny ({EvaluationText.Date(dueDate)}).";
        }
        if (status == ConversationStatus.Held && newAt > now)
        {
            return "Rozmowę można oznaczyć jako odbytą dopiero po jej terminie.";
        }
        return null;
    }
}
