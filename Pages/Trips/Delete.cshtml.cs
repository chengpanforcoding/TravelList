using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TravelList.Data;
using TravelList.Models;

namespace TravelList.Pages.Trips;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _db;
    public DeleteModel(AppDbContext db) => _db = db;

    public Trip Trip { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var trip = await _db.Trips.Include(t => t.Spots).FirstOrDefaultAsync(t => t.Id == id);
        if (trip == null) return NotFound();
        Trip = trip;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var trip = await _db.Trips.FindAsync(id);
        if (trip == null) return NotFound();
        _db.Trips.Remove(trip);
        await _db.SaveChangesAsync();
        return RedirectToPage("/Index");
    }
}
