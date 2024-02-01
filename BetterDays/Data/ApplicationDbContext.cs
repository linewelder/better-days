using BetterDays.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BetterDays.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public required DbSet<DailyNote> DailyNotes { get; init; }
    public required DbSet<Deed> Deeds { get; init; }
    public required DbSet<DoneDeed> DoneDeeds { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<DailyNote>()
            .HasIndex(dn => new { dn.UserId, dn.Date })
            .IsUnique();

        builder.Entity<DoneDeed>()
            .HasKey(dd => new { dd.DailyNoteId, dd.DeedId });
    }
}
