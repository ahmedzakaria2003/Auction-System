using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }  

        public string Token { get; set; }  
        public DateTime ExpirationDate { get; set; }  
    }

}
