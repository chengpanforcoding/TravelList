using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TravelList.Data;
using TravelList.Models;

namespace TravelList.Pages.Trips;

public class CreateModel : PageModel
{
    private readonly AppDbContext _db;
    public CreateModel(AppDbContext db) => _db = db;

    [BindProperty]
    public Trip Trip { get; set; } = new() { StartDate = DateTime.Today.AddDays(30), EndDate = DateTime.Today.AddDays(34) };

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        Trip.CreatedAt = DateTime.Now;
        Trip.ShareCode = Guid.NewGuid().ToString("N")[..8];
        _db.Trips.Add(Trip);
        await _db.SaveChangesAsync();
        return RedirectToPage("Itinerary", new { id = Trip.Id });
    }
}
