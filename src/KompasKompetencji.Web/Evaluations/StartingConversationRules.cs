using KompasKompetencji.Web.Data;

namespace KompasKompetencji.Web.Evaluations;

public enum StartingConversationStep
{
    Preparation,
    ReadyToConfirm,
    Confirmed,
}

public static class StartingConversationRules
{
    public const int DaysFromCycleStart = 30;

    public static DateOnly Deadline(DateOnly cycleStart) => cycleStart.AddDays(DaysFromCycleStart);

    public static bool IsOverdue(DateOnly cycleStart, DateTime now) => Deadline(cycleStart) < DateOnly.FromDateTime(now);

    public static DateTime? LatestAllowed(DateOnly cycleStart, DateTime now) =>
        IsOverdue(cycleStart, now) ? null : Deadline(cycleStart).ToDateTime(new TimeOnly(23, 59));

    public static DateTime Suggest(DateOnly cycleStart, DateTime now)
    {
        var today = DateOnly.FromDateTime(now);
        var tomorrowAtTen = today.AddDays(1).ToDateTime(new TimeOnly(10, 0));
        if (IsOverdue(cycleStart, now))
        {
            return tomorrowAtTen;
        }

        var deadline = Deadline(cycleStart);
        var day = NextWorkingDay(today.AddDays(7));
        if (day > deadline)
        {
            day = PreviousWorkingDay(deadline);
            if (day < today)
            {
                day = deadline;
            }
        }

        var suggestion = day.ToDateTime(new TimeOnly(10, 0));
        return suggestion < now ? tomorrowAtTen : suggestion;
    }

    public static string? Validate(DateTime newAt, DateTime? currentAt, ConversationStatus status, DateOnly cycleStart, DateTime now)
    {
        if (!IsOverdue(cycleStart, now) && DateOnly.FromDateTime(newAt) > Deadline(cycleStart))
        {
            return $"Rozmowa rozpoczynająca musi się odbyć w ciągu {DaysFromCycleStart} dni od początku cyklu, czyli najpóźniej {EvaluationText.Date(Deadline(cycleStart))}.";
        }
        return ConversationRules.ValidateStatus(newAt, currentAt, status, now);
    }

    public static StartingConversationStep Step(Evaluation? openEvaluation) =>
        openEvaluation?.PartIConfirmedAt is not null ? StartingConversationStep.Confirmed
        : openEvaluation?.FindConversation(ConversationType.Starting)?.Status == ConversationStatus.Held ? StartingConversationStep.ReadyToConfirm
        : StartingConversationStep.Preparation;

    private static DateOnly NextWorkingDay(DateOnly day) => day.DayOfWeek switch
    {
        DayOfWeek.Saturday => day.AddDays(2),
        DayOfWeek.Sunday => day.AddDays(1),
        _ => day,
    };

    private static DateOnly PreviousWorkingDay(DateOnly day) => day.DayOfWeek switch
    {
        DayOfWeek.Saturday => day.AddDays(-1),
        DayOfWeek.Sunday => day.AddDays(-2),
        _ => day,
    };
}
