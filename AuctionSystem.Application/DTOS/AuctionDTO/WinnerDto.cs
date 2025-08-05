using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.DTOS.AuctionDTO
{
    public class WinnerDto
    {
        public string WinnerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public decimal WinningAmount { get; set; }
        public string AuctionTitle { get; set; } = string.Empty;
    }


}
