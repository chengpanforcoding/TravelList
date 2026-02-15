using System.ComponentModel.DataAnnotations;

namespace TravelList.Models;

public class Spot
{
    public int Id { get; set; }

    public int TripId { get; set; }
    public Trip? Trip { get; set; }

    [Required(ErrorMessage = "請輸入景點名稱")]
    [Display(Name = "景點名稱")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "地址")]
    public string? Address { get; set; }

    [Display(Name = "第幾天")]
    public int DayNumber { get; set; } = 1;

    [Display(Name = "排序")]
    public int SortOrder { get; set; }

    [Display(Name = "開始時間")]
    public string? StartTime { get; set; }

    [Display(Name = "結束時間")]
    public string? EndTime { get; set; }

    [Display(Name = "分類")]
    public string Category { get; set; } = "景點";

    [Display(Name = "預算 (TWD)")]
    public decimal Budget { get; set; }

    [Display(Name = "備註")]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string GoogleMapsUrl =>
        string.IsNullOrWhiteSpace(Address)
            ? ""
            : $"https://www.google.com/maps/search/?api=1&query={Uri.EscapeDataString(Address)}";

    public static readonly string[] Categories = ["景點", "餐廳", "住宿", "交通", "購物", "其他"];

    public static string CategoryIcon(string category) => category switch
    {
        "景點" => "bi-camera",
        "餐廳" => "bi-cup-hot",
        "住宿" => "bi-house-door",
        "交通" => "bi-train-front",
        "購物" => "bi-bag",
        "其他" => "bi-three-dots",
        _ => "bi-geo-alt"
    };

    public static string CategoryColor(string category) => category switch
    {
        "景點" => "#0061f2",
        "餐廳" => "#e65100",
        "住宿" => "#6900c7",
        "交通" => "#0097a7",
        "購物" => "#d81b60",
        "其他" => "#718096",
        _ => "#0061f2"
    };
}
