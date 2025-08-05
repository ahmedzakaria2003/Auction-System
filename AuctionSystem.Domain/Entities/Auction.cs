using AuctionSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Domain.Entities
{
   public class Auction : BaseEntity
    {
      
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal StartingPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Guid CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; } = default!; // Navigation property 

        public Guid? WinnerId { get; set; }
        public ApplicationUser Winner { get; set; } = default!; // Navigation property
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = default!; // Navigation property

       
        public ICollection<Bid> Bids { get; set; } = [];
        public bool IsCanceled { get; set; } = false;

        public List<AuctionImage> Images { get; set; } = new();

        public bool IsPaid { get; set; } 
        public string? PaymentIntentId { get; set; }
        public AuctionStatus Status
        {
            get
            {
                if (IsCanceled)
                    return AuctionStatus.canceled;

                return EndTime > DateTime.UtcNow
                    ? AuctionStatus.open
                    : AuctionStatus.closed;
            }
        }

    }


}
