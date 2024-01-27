using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BetterDays.Data;
using BetterDays.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BetterDays.Pages;

public class Index(ApplicationDbContext context) : PageModel
{
    public class NewDailyNote
    {
        [Required]
        public DateOnly Date { get; init; }

        public string? Comment { get; init; }

        [DisplayName("Deeds Done")]
        public List<int>? DoneDeedIds { get; init; }
    }

    public class HistoryItem
    {
        public DateOnly Date { get; init; }
        public string? Comment { get; init; }
        public required List<string> DoneDeeds { get; init; }
    }

    [BindProperty]
    public NewDailyNote NewNote { get; set; } = null!;

    public List<Deed> Deeds { get; set; } = null!;

    public List<HistoryItem> Items { get; private set; } = null!;

    private async Task PopulatePage()
    {
        NewNote = new NewDailyNote
        {
            Date = DateOnly.FromDateTime(DateTime.Today)
        };
        Deeds = await context.Deeds.ToListAsync();

        Items = await context.DailyNotes
            .Include(day => day.Deeds!)
            .ThenInclude(dd => dd.Deed)
            .Select(day => new HistoryItem
            {
                Date = day.Date,
                Comment = day.Comment,
                DoneDeeds = day.Deeds!.Select(dd => dd.Deed!.Name).ToList()
            })
            .OrderByDescending(day => day.Date)
            .ToListAsync();
    }

    public async Task<IActionResult> OnGet()
    {
        if (!User.Identity?.IsAuthenticated ?? false)
        {
            return Redirect("/Identity/Account/Login");
        }

        await PopulatePage();
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!User.Identity?.IsAuthenticated ?? false)
        {
            return Redirect("/Identity/Account/Login");
        }

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

        await PopulatePage();
        return Page();
    }
}
