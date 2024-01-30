using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BetterDays.Data;
using BetterDays.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BetterDays.Pages;

public class CreateNote(ApplicationDbContext context) : PageModel
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

    /**
     * If it is after midnight, the note is most likely to be
     * about the day that has just ended.
     */
    private static DateOnly GetPerceivedToday()
    {
        var now = DateTime.Now;
        var stillNotSleeping = now.Hour < 10;
        return stillNotSleeping
            ? DateOnly.FromDateTime(now - TimeSpan.FromDays(1))
            : DateOnly.FromDateTime(now);
    }

    public async Task PopulatePage()
    {
        NewNote = new NewDailyNote
        {
            Date = GetPerceivedToday()
        };
        Deeds = await context.Deeds
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    public async Task<IActionResult> OnGet()
    {
        await PopulatePage();
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var date = NewNote.Date;
        if (await context.DailyNotes.AnyAsync(dn => dn.Date == date))
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
            Date = date,
            Comment = NewNote.Comment?.Trim(),
            Productivity = NewNote.Productivity,
            Mood = NewNote.Mood,
            Deeds = NewNote.DoneDeedIds?
                .Select(id => new DoneDeed
                {
                    DailyNoteDate = date,
                    DeedId = id
                })
                .ToList()
        };
        if (!TryValidateModel(newNote))
        {
            await PopulatePage();
            return Page();
        }

        context.Add(newNote);
        await context.SaveChangesAsync();

        return RedirectToPage(nameof(Index));
    }
}
