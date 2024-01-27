using System.ComponentModel.DataAnnotations;

namespace BetterDays.Models;

public class DailyNote
{
    [Key]
    public DateOnly Date { get; set; }

    [MaxLength(255)]
    public string? Comment { get; set; }

    public ICollection<DoneDeed>? Deeds { get; set; }
}
