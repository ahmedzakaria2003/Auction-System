using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.DTOS.AccountDTO
{
    public class LoginResultDto
    {

        public string AccessToken { get; set; } = default!; 
        public string RefreshToken { get; set; } = default!; 
        public Guid UserId { get; set; } 
        public string UserName { get; set; } = default!; 
        public string Role { get; set; } = default!; 

    }
}
