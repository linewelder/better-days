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

        // Create Users

        var testUser = new IdentityUser
        {
            Email = "test@better-days",
            UserName = "test@better-days",
            EmailConfirmed = true
        };

        var anotherUser = new IdentityUser
        {
            Email = "new@better-days",
            UserName = "new@better-days",
            EmailConfirmed = true
        };

        foreach (var user in new[] { testUser, anotherUser })
        {
            var result = await userManager.CreateAsync(user, "Pa$$w0rd");
            if (!result.Succeeded)
            {
                throw new Exception(result.ToString());
            }
        }

        // Create Deeds

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

        // Create Notes

        context.DailyNotes.Add(new DailyNote
        {
            Id = 1,
            UserId = testUser.Id,
            Date = new DateOnly(2024, 1, 24),
            Productivity = 1,
            Mood = 2,
            Deeds = new List<DoneDeed>
            {
                new() { DeedId = 2 }
            }
        });

        context.DailyNotes.Add(new DailyNote
        {
            Id = 2,
            UserId = testUser.Id,
            Date = new DateOnly(2024, 1, 25),
            Productivity = 4,
            Mood = 5,
            Deeds = new List<DoneDeed>
            {
                new() { DeedId = 1 },
                new() { DeedId = 2 }
            }
        });

        context.DailyNotes.Add(new DailyNote
        {
            Id = 3,
            UserId = testUser.Id,
            Date = new DateOnly(2024, 1, 27),
            Comment = "This is a test comment",
            Productivity = 3,
            Mood = 4,
            Deeds = new List<DoneDeed>
            {
                new() { DeedId = 1 },
            }
        });

        await context.SaveChangesAsync();
    }
}
