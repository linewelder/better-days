using BetterDays.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BetterDays.Pages;

public class Index(ApplicationDbContext context) : PageModel
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

    private async Task PopulatePage()
    {
        Items = await context.DailyNotes
            .Include(day => day.Deeds!)
            .ThenInclude(dd => dd.Deed)
            .Select(day => new HistoryItem
            {
                Date = day.Date,
                Productivity = day.Productivity,
                Mood = day.Mood,
                Comment = day.Comment,
                DoneDeeds = day.Deeds!.Select(dd => dd.Deed!.Name).ToList()
            })
            .OrderByDescending(day => day.Date)
            .ToListAsync();
    }

    public async Task<IActionResult> OnGet()
    {
        await PopulatePage();
        return Page();
    }
}
