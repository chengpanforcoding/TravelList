using System.ComponentModel.DataAnnotations;

namespace TravelList.Models;

public class Trip
{
    public int Id { get; set; }

    [Required(ErrorMessage = "請輸入旅行名稱")]
    [Display(Name = "旅行名稱")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "請輸入目的地")]
    [Display(Name = "目的地")]
    public string Destination { get; set; } = string.Empty;

    [Required]
    [Display(Name = "開始日期")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required]
    [Display(Name = "結束日期")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [Display(Name = "分享碼")]
    public string ShareCode { get; set; } = Guid.NewGuid().ToString("N")[..8];

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public List<Spot> Spots { get; set; } = [];

    public int TotalDays => (EndDate - StartDate).Days + 1;
}
