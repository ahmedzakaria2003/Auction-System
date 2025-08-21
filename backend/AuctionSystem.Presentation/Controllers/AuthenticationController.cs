using Auction_System.Controllers;
using AuctionSystem.Application.DTOS.AccountDTO;
using AuctionSystem.Application.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuctionSystem.WebApi.Controllers
{
    
    public class AuthenticationController : ApiBaseController
    {
        private readonly IServiceManager _serviceManager;

        public AuthenticationController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _serviceManager.AuthenticationService.LoginAsync(loginDto);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _serviceManager.AuthenticationService.RegisterAsync(registerDto);
            return Ok(result);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            await _serviceManager.AuthenticationService.ResetPasswordAsync(dto);
            return Ok("Password reset successfully");
        }

        [HttpPost("send-otp")]
        public async Task<ActionResult> SendOtp([FromBody] SendOtpDto dto)
        {
            await _serviceManager.AuthenticationService.SendOtpAsync(dto.Email);
            return Ok("OTP sent successfully");
        }

        [HttpPost("verify-otp")]
        public async Task<ActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            await _serviceManager.AuthenticationService.VerifyOtpAsync(dto.Email, dto.Otp);
            return Ok("OTP verified successfully");
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var result = await _serviceManager.AuthenticationService.RefreshTokenAsync(refreshToken);
            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout([FromBody] string refreshToken)
        {
            await _serviceManager.AuthenticationService.LogoutAsync(refreshToken);
            return Ok("Logged out successfully");
        }



    }
}
