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

        string?[] comments =
        [
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, " +
            "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
            "Ut enim ad minim veniam, quis nostrud exercitation ullamco " +
            "laboris nisi ut aliquip ex ea commodo consequat.",
            null,
            "Duis aute irure dolor in reprehenderit in voluptate velit esse " +
            "cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat " +
            "cupidatat non proident, sunt in culpa qui officia deserunt " +
            "mollit anim id est laborum."
        ];

        var startingDate = new DateOnly(2024, 1, 24);
        for (var i = 0; i < 10; i++)
        {
            var deeds = new List<DoneDeed>();
            if (i % 2 == 0)
            {
                deeds.Add(new DoneDeed { DeedId = 1 });
            }
            if (i % 4 > 2)
            {
                deeds.Add(new DoneDeed { DeedId = 2 });
            }

            context.DailyNotes.Add(new DailyNote
            {
                Id = i + 1,
                UserId = testUser.Id,
                Date = startingDate.AddDays(i),
                Comment = comments[i % comments.Length],
                Productivity = i % 5,
                Mood = (i + 1) % 5,
                Deeds = deeds
            });
        }

        await context.SaveChangesAsync();
    }
}
