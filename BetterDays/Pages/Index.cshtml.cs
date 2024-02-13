using BetterDays.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BetterDays.Pages;

public class Index(ApplicationDbContext context, UserManager<IdentityUser> userManager) : PageModel
{
    public class HistoryItem
    {
        public DateOnly Date { get; init; }
        public int Productivity { get; init; }
        public int Mood { get; init; }
        public string? Comment { get; init; }
        public required List<string> DoneDeeds { get; init; }
    }

    public List<HistoryItem> Items { get; private set; } = null!;

    public const int NotesPerPage = 7;

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;

    public int TotalPages { get; private set; }

    public async Task<IActionResult> OnGet()
    {
        var userId = userManager.GetUserId(User)!;

        var allNotes =
            from day in context.DailyNotes
            where day.UserId == userId
            orderby day.Date descending
            select new HistoryItem
            {
                Date = day.Date,
                Productivity = day.Productivity,
                Mood = day.Mood,
                Comment = day.Comment,
                DoneDeeds = day.Deeds!.Select(dd => dd.Deed!.Name).ToList()
            };

        TotalPages = (await allNotes.CountAsync() + NotesPerPage - 1) / NotesPerPage;

        Items = await allNotes
            .Skip(NotesPerPage * (CurrentPage - 1))
            .Take(NotesPerPage)
            .ToListAsync();

        return Page();
    }
}
