using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BetterDays.Data;
using BetterDays.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BetterDays.Pages;

public class CreateDeed(ApplicationDbContext context, UserManager<IdentityUser> userManager) : PageModel
{
    [BindProperty, DisplayName("Name")]
    [Required, MaxLength(20)]
    public string NewDeed { get; set; } = null!;

    public void OnGet() {}

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var userId = userManager.GetUserId(User)!;

        var nameTrimmed = NewDeed.Trim();
        if (await context.Deeds.AnyAsync(d => d.Name == nameTrimmed))
        {
            ModelState.AddModelError(nameof(NewDeed), "Name must be unique");
        }

        var newDeed = new Deed
        {
            Name = nameTrimmed,
            UserId = userId
        };
        if (!TryValidateModel(newDeed))
        {
            return Page();
        }

        context.Deeds.Add(newDeed);
        await context.SaveChangesAsync();

        return RedirectToPage(nameof(CreateNote));
    }
}
