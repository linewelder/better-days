using BetterDays.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BetterDays.Pages;

public class DeleteNote(ApplicationDbContext context) : PageModel
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

        var note = await context.DailyNotes.FindAsync(Date);
        if (note is null)
        {
            return NotFound();
        }

        context.DailyNotes.Remove(note);
        await context.SaveChangesAsync();

        return RedirectToPage(nameof(Index));
    }
}
