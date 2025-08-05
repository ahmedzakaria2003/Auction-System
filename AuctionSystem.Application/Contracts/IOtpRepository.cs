using AuctionSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Contracts
{
    public interface IOtpRepository :  IGenericRepository<Otp>
    {
        Task<Otp?> GetLatestValidOtpAsync(string email);
    }


}
