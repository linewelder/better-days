using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BetterDays.Data;
using BetterDays.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BetterDays.Pages;

public class EditNote(ApplicationDbContext context, UserManager<IdentityUser> userManager) : PageModel
{
    public class EditedDailyNote
    {
        [StringLength(255)]
        public string? Comment { get; init; }

        public int Productivity { get; init; }

        public int Mood { get; init; }

        [DisplayName("Deeds Done")]
        public List<int>? DoneDeedIds { get; init; }
    }

    public DateOnly Date { get; set; }

    [BindProperty]
    public EditedDailyNote Note { get; set; } = null!;

    public List<Deed> Deeds { get; set; } = null!;

    public async Task PopulatePage()
    {
        var userId = userManager.GetUserId(User)!;

        Deeds = await context.Deeds
            .Where(d => d.UserId == userId)
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    public async Task<IActionResult> OnGet(string date)
    {
        if (!DateOnly.TryParse(date, out var parsedDate))
        {
            return NotFound();
        }
        Date = parsedDate;

        var note = await context.DailyNotes
            .Include(dn => dn.Deeds)
            .FirstOrDefaultAsync(dn => dn.Date == Date);
        if (note is null)
        {
            return NotFound();
        }

        Note = new EditedDailyNote
        {
            Comment = note.Comment,
            DoneDeedIds = note.Deeds!
                .Select(dd => dd.DeedId)
                .ToList(),
            Mood = note.Mood,
            Productivity = note.Productivity
        };

        await PopulatePage();
        return Page();
    }

    private async Task<bool> CheckDeedsAsync(DailyNote note)
    {
        foreach (var doneDeed in note.Deeds!)
        {
            var deed = await context.Deeds.FindAsync(doneDeed.DeedId);
            if (deed is not null && deed.UserId == note.UserId)
            {
                continue;
            }

            ModelState.AddModelError(
                "Note.DoneDeedIds", $"Deed with ID {doneDeed.DeedId} does not belong to the user");
            return false;
        }

        return true;
    }

    public async Task<IActionResult> OnPost(string date)
    {
        if (!DateOnly.TryParse(date, out var parsedDate))
        {
            return NotFound();
        }
        Date = parsedDate;

        var userId = userManager.GetUserId(User);
        var note = await context.DailyNotes
            .Include(dn => dn.Deeds)
            .FirstOrDefaultAsync(dn => dn.UserId == userId && dn.Date == Date);
        if (note is null)
        {
            return NotFound();
        }

        note.Comment = Note.Comment;
        note.Productivity = Note.Productivity;
        note.Mood = Note.Mood;
        note.Deeds = (Note.DoneDeedIds ?? [])
            .Select(id => new DoneDeed { DeedId = id })
            .ToList();
        if (!TryValidateModel(note))
        {
            await PopulatePage();
            return Page();
        }

        if (!await CheckDeedsAsync(note))
        {
            await PopulatePage();
            return Page();
        }

        await context.SaveChangesAsync();
        return RedirectToPage(nameof(Index));
    }
}
