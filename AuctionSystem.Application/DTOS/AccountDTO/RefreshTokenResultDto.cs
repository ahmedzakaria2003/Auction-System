using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.DTOS.AccountDTO
{
    public class RefreshTokenResultDto
    {

        public string AccessToken { get; set; } = default!; 
        public string RefreshToken { get; set; } = default!; 
    }
}
