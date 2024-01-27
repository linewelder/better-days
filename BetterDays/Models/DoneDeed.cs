namespace BetterDays.Models;

public class DoneDeed
{
    public DateOnly DailyNoteDate { get; set; }

    public int DeedId { get; set; }

    public Deed? Deed { get; set; }
}
