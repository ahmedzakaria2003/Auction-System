using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.DTOS.SellerFeedbackDTO
{
    public class AddFeedbackRequest
    {
        [Required]
        public Guid AuctionId { get; set; }
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
        [StringLength(1000, ErrorMessage = "Comment can't be longer than 1000 characters.")]

        public string Comment { get; set; } = string.Empty; 
    }
}
