using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class CreateAuctionDto
{
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string Title { get; set; } = default!;

    [Required]
    [StringLength(1000, MinimumLength = 10)]
    public string Description { get; set; } = default!;

    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "Starting price must be greater than 0.")]
    public decimal StartingPrice { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime StartTime { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime EndTime { get; set; }

    [Required]
    public Guid CategoryId { get; set; }

    public List<IFormFile>? Images { get; set; } = new();
}
