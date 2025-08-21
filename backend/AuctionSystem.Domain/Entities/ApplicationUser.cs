using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid> 
    {
        public ICollection<Auction> CreatedAuctions { get; set; } = [];
        public ICollection<Auction> WonAuctions { get; set; } = [];
        public ICollection<Bid> Bids { get; set; } = [];
        public ICollection<SellerFeedback> SellerFeedbacks { get; set; } = [];
        public string UserType { get; set; } = default!;
        public bool IsBanned { get; set; } = false;
        public string FullName { get; set; } = default!;  
        public string Address { get; set; } = default!; 
    }
}
