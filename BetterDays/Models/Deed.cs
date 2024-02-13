using System.ComponentModel.DataAnnotations;

namespace BetterDays.Models;

public class Deed
{
    [Key]
    public int Id { get; set; }

    [StringLength(36)]
    public required string UserId { get; set; }

    [Required, MaxLength(20)]
    public required string Name { get; set; }
}
