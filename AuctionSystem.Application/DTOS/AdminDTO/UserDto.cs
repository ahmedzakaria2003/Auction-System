using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.DTOS.AdminDTO
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = default!; 
        public string Email { get; set; } = default!; 
        public bool IsBanned { get; set; } // Indicates if the user is banned
        public string UserType { get; set; } = default!;

    }
}
