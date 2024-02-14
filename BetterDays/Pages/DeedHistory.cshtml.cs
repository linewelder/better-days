using BetterDays.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BetterDays.Pages;

public class DeedHistory(ApplicationDbContext context) : PageModel
{
    public string DeedName { get; set; } = null!;
    public List<DateOnly> HighlightedDays { get; set; } = null!;

    public async Task<IActionResult> OnGet(int id)
    {
        var deed = await context.Deeds.FindAsync(id);
        if (deed is null)
        {
            return NotFound();
        }
        DeedName = deed.Name;

        HighlightedDays = await context.DailyNotes
            .Where(dn => dn.Deeds!.Any(dd => dd.DeedId == id))
            .Select(dn => dn.Date)
            .ToListAsync();

        return Page();
    }
}
