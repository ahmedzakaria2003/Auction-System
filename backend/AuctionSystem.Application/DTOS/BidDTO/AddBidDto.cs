using System.ComponentModel.DataAnnotations;

public class AddBidDto
{
    [Required]
    public Guid AuctionId { get; set; }


    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Bid amount must be greater than zero.")]
    public decimal Amount { get; set; }

}
