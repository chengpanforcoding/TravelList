using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using TravelList.Data;
using TravelList.Models;

var builder = WebApplication.CreateBuilder(args);

// 設定 Port（Render 使用 PORT 環境變數）
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddRazorPages();

// 資料庫路徑：生產環境使用 /data 持久化目錄
var dbPath = builder.Environment.IsDevelopment()
    ? "travellist.db"
    : "/data/travellist.db";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// Rate Limiting 防止流量攻擊
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // 全域限流：每個 IP 每分鐘最多 60 次請求
    options.AddPolicy("fixed", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 60,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRateLimiter();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets().RequireRateLimiting("fixed");

// === Spot API 端點 ===

app.MapPost("/api/trips/{tripId}/spots", async (int tripId, Spot spot, AppDbContext db) =>
{
    spot.TripId = tripId;
    spot.CreatedAt = DateTime.Now;
    var maxOrder = await db.Spots.Where(s => s.TripId == tripId && s.DayNumber == spot.DayNumber).MaxAsync(s => (int?)s.SortOrder) ?? -1;
    spot.SortOrder = maxOrder + 1;
    db.Spots.Add(spot);
    await db.SaveChangesAsync();
    return Results.Ok(new { spot.Id, spot.Name, spot.DayNumber, spot.SortOrder, spot.GoogleMapsUrl });
});

app.MapPut("/api/spots/{id}", async (int id, Spot updated, AppDbContext db) =>
{
    var spot = await db.Spots.FindAsync(id);
    if (spot == null) return Results.NotFound();
    spot.Name = updated.Name;
    spot.Address = updated.Address;
    spot.DayNumber = updated.DayNumber;
    spot.StartTime = updated.StartTime;
    spot.EndTime = updated.EndTime;
    spot.Category = updated.Category;
    spot.Budget = updated.Budget;
    spot.Notes = updated.Notes;
    await db.SaveChangesAsync();
    return Results.Ok(spot);
});

app.MapDelete("/api/spots/{id}", async (int id, AppDbContext db) =>
{
    var spot = await db.Spots.FindAsync(id);
    if (spot == null) return Results.NotFound();
    db.Spots.Remove(spot);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapPost("/api/spots/reorder", async (SpotReorderRequest[] items, AppDbContext db) =>
{
    foreach (var item in items)
    {
        var spot = await db.Spots.FindAsync(item.Id);
        if (spot != null)
        {
            spot.DayNumber = item.DayNumber;
            spot.SortOrder = item.SortOrder;
        }
    }
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();

public record SpotReorderRequest(int Id, int DayNumber, int SortOrder);
