using BetterDays.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BetterDays.Pages;

public class DeleteNote(ApplicationDbContext context, UserManager<IdentityUser> userManager) : PageModel
{
    public DateOnly Date { get; set; }

    public IActionResult OnGet(string date)
    {
        if (!DateOnly.TryParse(date, out var parsedDate))
        {
            return NotFound();
        }

        Date = parsedDate;
        return Page();

    }

    public async Task<IActionResult> OnPost(string date)
    {
        if (!DateOnly.TryParse(date, out var parsedDate))
        {
            return NotFound();
        }
        Date = parsedDate;

        var userId = userManager.GetUserId(User);
        var note = await context.DailyNotes.FirstOrDefaultAsync(
            dn => dn.UserId == userId && dn.Date == Date);
        if (note is null)
        {
            return NotFound();
        }

        context.DailyNotes.Remove(note);
        await context.SaveChangesAsync();

        return RedirectToPage(nameof(Index));
    }
}
