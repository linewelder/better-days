using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BetterDays.Data;
using BetterDays.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BetterDays.Pages;

public class CreateDeed(ApplicationDbContext context) : PageModel
{
    [BindProperty, DisplayName("Name")]
    [Required, MaxLength(20)]
    public string NewDeed { get; set; } = null!;

    public void OnGet() {}

    public async Task<IActionResult> OnPost()
    {
        var nameTrimmed = NewDeed.Trim();
        if (await context.Deeds.AnyAsync(d => d.Name == nameTrimmed))
        {
            ModelState.AddModelError(nameof(NewDeed), "Name must be unique");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var newDeed = new Deed
        {
            Name = nameTrimmed
        };
        if (!TryValidateModel(newDeed))
        {
            return Page();
        }

        context.Deeds.Add(newDeed);
        await context.SaveChangesAsync();

        return RedirectToPage(nameof(Index));
    }
}
