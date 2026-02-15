using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TravelList.Data;
using TravelList.Models;

namespace TravelList.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    public IndexModel(AppDbContext db) => _db = db;

    public List<Trip> Trips { get; set; } = [];

    public async Task OnGetAsync()
    {
        Trips = await _db.Trips
            .Include(t => t.Spots)
            .OrderByDescending(t => t.StartDate)
            .ToListAsync();
    }
}
