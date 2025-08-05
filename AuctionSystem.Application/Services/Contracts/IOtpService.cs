using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Services.Contracts
{
    public interface IOtpService
    {
        Task SaveOtpAsync(string userEmail, string otp);
        Task<string> GetOtpAsync(string userEmail);
        Task SendOtpToEmail(string userEmail, string otp);
    }
}
