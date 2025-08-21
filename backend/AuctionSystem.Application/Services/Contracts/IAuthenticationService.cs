using AuctionSystem.Application.DTOS.AccountDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Services.Contracts
{
    public interface IAuthenticationService
    {
    
            Task<LoginResultDto> LoginAsync(LoginDto loginDto);
            Task<RegisterResultDto> RegisterAsync(RegisterDto registerDto);
            Task<RefreshTokenResultDto> RefreshTokenAsync(string refreshToken);

            Task SendOtpAsync(string userEmail);

            Task<bool> VerifyOtpAsync(string userEmail, string otp);

            Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);

               Task<bool> LogoutAsync(string refreshToken);




    }
}
