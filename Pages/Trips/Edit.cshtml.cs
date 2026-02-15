using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TravelList.Data;
using TravelList.Models;

namespace TravelList.Pages.Trips;

public class EditModel : PageModel
{
    private readonly AppDbContext _db;
    public EditModel(AppDbContext db) => _db = db;

    [BindProperty]
    public Trip Trip { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var trip = await _db.Trips.FindAsync(id);
        if (trip == null) return NotFound();
        Trip = trip;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var trip = await _db.Trips.FindAsync(Trip.Id);
        if (trip == null) return NotFound();
        trip.Title = Trip.Title;
        trip.Destination = Trip.Destination;
        trip.StartDate = Trip.StartDate;
        trip.EndDate = Trip.EndDate;
        await _db.SaveChangesAsync();
        return RedirectToPage("/Index");
    }
}
