using BetterDays.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BetterDays.Data;

public static class DbSeeder
{
    public static async Task SeedData(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        if (await context.Deeds.AnyAsync())
        {
            return;
        }

        var result = await userManager.CreateAsync(new IdentityUser
        {
            Email = "test@better-days",
            UserName = "test@better-days",
            EmailConfirmed = true
        }, "Pa$$w0rd");
        if (!result.Succeeded)
        {
            throw new Exception(result.ToString());
        }

        context.Deeds.Add(new Deed
        {
            Id = 1,
            Name = "Dishes"
        });

        context.Deeds.Add(new Deed
        {
            Id = 2,
            Name = "Vacuum"
        });

        context.DailyNotes.Add(new DailyNote
        {
            Date = new DateOnly(2024, 1, 24),
            Deeds = new List<DoneDeed>
            {
                new() { DeedId = 2 }
            }
        });

        context.DailyNotes.Add(new DailyNote
        {
            Date = new DateOnly(2024, 1, 25),
            Deeds = new List<DoneDeed>
            {
                new() { DeedId = 1 },
                new() { DeedId = 2 }
            }
        });

        context.DailyNotes.Add(new DailyNote
        {
            Date = new DateOnly(2024, 1, 27),
            Comment = "This is a test comment",
            Deeds = new List<DoneDeed>
            {
                new() { DeedId = 1 },
            }
        });

        await context.SaveChangesAsync();
    }
}
