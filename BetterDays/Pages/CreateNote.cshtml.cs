using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BetterDays.Data;
using BetterDays.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BetterDays.Pages;

public class CreateNote(ApplicationDbContext context, UserManager<IdentityUser> userManager) : PageModel
{
    public class NewDailyNote
    {
        [Required]
        public DateOnly Date { get; init; }

        [StringLength(255)]
        public string? Comment { get; init; }

        public int Productivity { get; init; }

        public int Mood { get; init; }

        [DisplayName("Deeds Done")]
        public List<int>? DoneDeedIds { get; init; }
    }

    [BindProperty]
    public NewDailyNote NewNote { get; set; } = null!;

    public List<Deed> Deeds { get; set; } = null!;

    public async Task PopulatePage()
    {
        var userId = userManager.GetUserId(User)!;

        NewNote = new NewDailyNote();
        Deeds = await context.Deeds
            .Where(d => d.UserId == userId)
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    public async Task<IActionResult> OnGet()
    {
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

    public async Task<IActionResult> OnPost()
    {
        var userId = userManager.GetUserId(User)!;
        var date = NewNote.Date;
        if (await context.DailyNotes.AnyAsync(dn => dn.UserId == userId &&  dn.Date == date))
        {
            ModelState.AddModelError("", "There already is a note for today");
        }

        if (!ModelState.IsValid)
        {
            await PopulatePage();
            return Page();
        }

        var newNote = new DailyNote
        {
            UserId = userId,
            Date = date,
            Comment = NewNote.Comment?.Trim(),
            Productivity = NewNote.Productivity,
            Mood = NewNote.Mood,
            Deeds = (NewNote.DoneDeedIds ?? [])
                .Select(id => new DoneDeed { DeedId = id })
                .ToList()
        };
        if (!TryValidateModel(newNote))
        {
            await PopulatePage();
            return Page();
        }

        if (!await CheckDeedsAsync(newNote))
        {
            await PopulatePage();
            return Page();
        }

        context.Add(newNote);
        await context.SaveChangesAsync();

        return RedirectToPage(nameof(Index));
    }
}
