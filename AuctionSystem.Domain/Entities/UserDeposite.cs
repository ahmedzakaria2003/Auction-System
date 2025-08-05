using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Domain.Entities
{
    public class UserDeposite : BaseEntity
    {
        public Guid UserId { get; set; }
        public string PaymentIntentId { get; set; } = null!;
        public bool IsPaid { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }
        public ApplicationUser? User { get; set; }
        public Guid AuctionId { get; set; }
        public Auction? Auction { get; set; }

    }
}
