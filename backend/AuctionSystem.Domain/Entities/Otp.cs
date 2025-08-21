using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Domain.Entities
{
    public class Otp : BaseEntity
    {
        public string UserEmail { get; set; } 
        public string OtpCode { get; set; }  
        public DateTime ExpirationDate { get; set; }  
    }

}
