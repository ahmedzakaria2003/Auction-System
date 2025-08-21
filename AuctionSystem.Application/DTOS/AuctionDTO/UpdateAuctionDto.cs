using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AuctionSystem.Application.DTOS.AuctionProfile
{
    public class UpdateAuctionDto
    {
        [StringLength(100, MinimumLength = 5)]
        public string? Title { get; set; }

        [StringLength(1000, MinimumLength = 10)]
        public string? Description { get; set; }


       

        public Guid? CategoryId { get; set; }

        public List<IFormFile> Images { get; set; } = new();
    }
}
