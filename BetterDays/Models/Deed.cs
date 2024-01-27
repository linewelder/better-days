using System.ComponentModel.DataAnnotations;

namespace BetterDays.Models;

public class Deed
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(20)]
    public required string Name { get; set; }
}
