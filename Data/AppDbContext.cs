using Microsoft.EntityFrameworkCore;
using TravelList.Models;

namespace TravelList.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Trip> Trips => Set<Trip>();
    public DbSet<Spot> Spots => Set<Spot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trip>().HasMany(t => t.Spots).WithOne(s => s.Trip).HasForeignKey(s => s.TripId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Trip>().HasData(
            new Trip { Id = 1, Title = "2028 東京自由行", Destination = "日本東京", StartDate = new DateTime(2028, 4, 1), EndDate = new DateTime(2028, 4, 5), ShareCode = "demo1234", CreatedAt = new DateTime(2028, 1, 15) }
        );

        modelBuilder.Entity<Spot>().HasData(
            // Day 1
            new Spot { Id = 1, TripId = 1, Name = "成田機場 → 飯店", Address = "東京都新宿區", DayNumber = 1, SortOrder = 0, StartTime = "14:00", EndTime = "16:00", Category = "交通", Budget = 3000, Notes = "搭 N'EX 成田特快", CreatedAt = new DateTime(2028, 1, 15) },
            new Spot { Id = 2, TripId = 1, Name = "新宿御苑", Address = "東京都新宿區内藤町11", DayNumber = 1, SortOrder = 1, StartTime = "16:30", EndTime = "18:00", Category = "景點", Budget = 500, Notes = "賞櫻名所", CreatedAt = new DateTime(2028, 1, 15) },
            new Spot { Id = 3, TripId = 1, Name = "一蘭拉麵 新宿店", Address = "東京都新宿區歌舞伎町1-22-7", DayNumber = 1, SortOrder = 2, StartTime = "18:30", EndTime = "19:30", Category = "餐廳", Budget = 1200, Notes = "必吃！", CreatedAt = new DateTime(2028, 1, 15) },
            // Day 2
            new Spot { Id = 4, TripId = 1, Name = "淺草寺", Address = "東京都台東區淺草2-3-1", DayNumber = 2, SortOrder = 0, StartTime = "09:00", EndTime = "11:00", Category = "景點", Budget = 0, Notes = "雷門打卡", CreatedAt = new DateTime(2028, 1, 15) },
            new Spot { Id = 5, TripId = 1, Name = "東京晴空塔", Address = "東京都墨田區押上1-1-2", DayNumber = 2, SortOrder = 1, StartTime = "11:30", EndTime = "13:30", Category = "景點", Budget = 2100, Notes = "展望台門票", CreatedAt = new DateTime(2028, 1, 15) },
            new Spot { Id = 6, TripId = 1, Name = "秋葉原電器街", Address = "東京都千代田區外神田", DayNumber = 2, SortOrder = 2, StartTime = "14:30", EndTime = "17:00", Category = "購物", Budget = 5000, Notes = "逛動漫周邊", CreatedAt = new DateTime(2028, 1, 15) },
            // Day 3
            new Spot { Id = 7, TripId = 1, Name = "明治神宮", Address = "東京都澀谷區代代木神園町1-1", DayNumber = 3, SortOrder = 0, StartTime = "09:00", EndTime = "10:30", Category = "景點", Budget = 0, Notes = "感受神社氛圍", CreatedAt = new DateTime(2028, 1, 15) },
            new Spot { Id = 8, TripId = 1, Name = "竹下通", Address = "東京都澀谷區神宮前1", DayNumber = 3, SortOrder = 1, StartTime = "11:00", EndTime = "12:30", Category = "購物", Budget = 3000, Notes = "原宿潮流", CreatedAt = new DateTime(2028, 1, 15) },
            new Spot { Id = 9, TripId = 1, Name = "澀谷十字路口", Address = "東京都澀谷區道玄坂2", DayNumber = 3, SortOrder = 2, StartTime = "13:00", EndTime = "14:00", Category = "景點", Budget = 0, Notes = "世界最大路口", CreatedAt = new DateTime(2028, 1, 15) }
        );
    }
}
