using BetterDays.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BetterDays.Pages;

public class DeedHistory(ApplicationDbContext context, UserManager<IdentityUser> userManager) : PageModel
{
    public class DeedInfo
    {
        public required string Name { get; set; }
        public required List<DateOnly> HighlightedDays { get; set; }
    }

    private const int WeeksShown = 4;

    /// <summary>
    /// Must be on Monday.
    /// </summary>
    public DateOnly RangeStart { get; set; }
    public DateOnly RangeEnd { get; set; }
    public DateOnly Today { get; set; }

    public List<DeedInfo> Deeds { get; set; } = null!;

    public async Task<IActionResult> OnGet()
    {
        Today = DateOnly.FromDateTime(DateTime.Today);

        var rangeStart = Today.AddDays(-7 * WeeksShown);
        var firstWeekStart = rangeStart.AddDays(-(((int)rangeStart.DayOfWeek - 1) % 7));

        RangeStart = firstWeekStart;
        RangeEnd = Today;

        var userId = userManager.GetUserId(User)!;
        var query =
            from deed in context.Deeds
            where deed.UserId == userId
            select new DeedInfo
            {
                Name = deed.Name,
                HighlightedDays =
                    (from doneDeed in deed.Dates
                    let date = doneDeed.DailyNote.Date
                    where RangeStart <= date && date <= RangeEnd
                    orderby date
                    select date).ToList()
            };

        Deeds = await query.ToListAsync();
        return Page();
    }
}
