using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TravelList.Data;
using TravelList.Models;

namespace TravelList.Pages.Trips;

public class ItineraryModel : PageModel
{
    private readonly AppDbContext _db;
    public ItineraryModel(AppDbContext db) => _db = db;

    public Trip Trip { get; set; } = new();
    public Dictionary<int, List<Spot>> SpotsByDay { get; set; } = [];
    public string ShareUrl { get; set; } = "";

    public async Task<IActionResult> OnGetAsync(int? id, string? code)
    {
        Trip? trip = null;
        if (id.HasValue)
            trip = await _db.Trips.Include(t => t.Spots).FirstOrDefaultAsync(t => t.Id == id.Value);
        else if (!string.IsNullOrEmpty(code))
            trip = await _db.Trips.Include(t => t.Spots).FirstOrDefaultAsync(t => t.ShareCode == code);

        if (trip == null) return NotFound();
        Trip = trip;

        for (int day = 1; day <= Trip.TotalDays; day++)
        {
            SpotsByDay[day] = Trip.Spots
                .Where(s => s.DayNumber == day)
                .OrderBy(s => s.SortOrder)
                .ToList();
        }

        ShareUrl = $"{Request.Scheme}://{Request.Host}/Trips/Itinerary?code={Trip.ShareCode}";
        return Page();
    }
}
