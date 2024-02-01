using System.ComponentModel.DataAnnotations;

namespace BetterDays.Models;

public class DoneDeed
{
    public int DailyNoteId { get; set; }

    public int DeedId { get; set; }

    public Deed? Deed { get; set; }
}
