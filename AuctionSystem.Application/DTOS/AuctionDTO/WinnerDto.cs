using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.DTOS.AuctionDTO
{
    public class WinnerDto
    {
        public string WinnerName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public decimal WinningAmount { get; set; }
        public string AuctionTitle { get; set; } = string.Empty;
    }


}
