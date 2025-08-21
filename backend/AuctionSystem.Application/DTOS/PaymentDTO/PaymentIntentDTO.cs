using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.DTOS.PaymentDTO
{
   public class PaymentIntentDTO
    {
        public string ClientSecret { get; set; } = null!;
        public string PaymentIntentId { get; set; } = null!;
    }
}
