using System.ComponentModel.DataAnnotations;

namespace BetterDays.Models;

public class DailyNote
{
    [Key]
    public int Id { get; set; }

    [StringLength(36)]
    public required string UserId { get; set; }

    public DateOnly Date { get; set; }

    [MaxLength(255)]
    public string? Comment { get; set; }

    [Range(1, 5)]
    public int Productivity { get; set; }

    [Range(1, 5)]
    public int Mood { get; set; }

    public ICollection<DoneDeed>? Deeds { get; set; }
}
