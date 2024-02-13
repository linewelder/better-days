namespace BetterDays;

public static class Utils
{
    /**
     * The hour at which the next day starts.
     */
    public const int PerceivedDayBoundary = 6;

    /**
     * If it is after midnight, the note is most likely to be
     * about the day that has just ended.
     */
    public static DateOnly PerceivedToday {
        get {
            var today = DateTime.Now;
            var stillNotSleeping = today.Hour < PerceivedDayBoundary;
            if (stillNotSleeping)
            {
                today -= TimeSpan.FromDays(1);
            }

            return DateOnly.FromDateTime(today);
        }
    }
}
