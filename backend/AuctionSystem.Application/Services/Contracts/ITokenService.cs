using AuctionSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Services.Contracts
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user, IEnumerable<string> roles);
        Task<ApplicationUser> ValidateRefreshTokenAsync(string refreshToken);
    }
}
