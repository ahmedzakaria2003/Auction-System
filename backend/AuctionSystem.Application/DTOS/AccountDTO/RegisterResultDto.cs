using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.DTOS.AccountDTO
{

    public class RegisterResultDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = default!;
        public string Token { get; set; } = default!;


    }
}
