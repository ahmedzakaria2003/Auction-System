using AuctionSystem.Application.Contracts;
using AuctionSystem.Application.DTOS.AccountDTO;
using AuctionSystem.Application.Services.Contracts;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOtpService _otpService;
    private readonly ITokenService _tokenService;

    public AuthenticationService(UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork , IOtpService otpService , ITokenService tokenService)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _otpService = otpService;
        _tokenService = tokenService;

       
    }

    public async Task<LoginResultDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            throw new UnauthorizedException("Invalid Email or password");

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = await _tokenService.GenerateTokenAsync(user, roles);
        var refreshToken = Guid.NewGuid().ToString();

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpirationDate = DateTime.UtcNow.AddDays(7)
        };

        await _unitOfWork.refreshToken.AddAsync(refreshTokenEntity);
        await _unitOfWork.SaveChangesAsync();

        return new LoginResultDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            UserName = user.UserName,
            Role = roles.FirstOrDefault(),
            UserId = user.Id
        };
    }

    public async Task<RegisterResultDto> RegisterAsync(RegisterDto registerDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            throw new BadRequestException("Email is already registered.");
        }

        if (registerDto.UserType != "Seller" && registerDto.UserType != "Bidder")
            throw new BadRequestException("Invalid user type");

        var user = new ApplicationUser
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            Address = registerDto.Address,
            FullName = registerDto.FullName,
            UserType = registerDto.UserType,
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
            throw new BadRequestException(result.Errors.Select(e => e.Description).ToList());

        await _userManager.AddToRoleAsync(user, registerDto.UserType);  // "Seller" أو "Bidder"

        var token = await _tokenService.GenerateTokenAsync(user, new[] { registerDto.UserType });

        return new RegisterResultDto
        {
            UserId = user.Id,
            UserName = user.UserName,
            Token = token
        };
    }

    public async Task<RefreshTokenResultDto> RefreshTokenAsync(string refreshToken)
    {
        var tokenEntity = await _unitOfWork.refreshToken.GetByTokenAsync(refreshToken);
        if (tokenEntity == null || tokenEntity.ExpirationDate < DateTime.UtcNow)
            throw new UnauthorizedException("Invalid or expired refresh token");

        var user = await _userManager.FindByIdAsync(tokenEntity.UserId.ToString());
        if (user == null)
            throw new NotFoundException("User not found");

        var roles = await _userManager.GetRolesAsync(user);
        var newAccessToken = await _tokenService.GenerateTokenAsync(user, roles);
        var newRefreshToken = Guid.NewGuid().ToString();

        tokenEntity.Token = newRefreshToken;
        tokenEntity.ExpirationDate = DateTime.UtcNow.AddDays(7);
        await _unitOfWork.refreshToken.UpdateAsync(tokenEntity);
        await _unitOfWork.SaveChangesAsync();

        return new RefreshTokenResultDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task SendOtpAsync(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null)
            throw new NotFoundException("Email not found");

        var otp = GenerateOtp();
        await _otpService.SendOtpToEmail(userEmail, otp);
        await _otpService.SaveOtpAsync(userEmail, otp);
    }

    public async Task<bool> VerifyOtpAsync(string userEmail, string otp)
    {
        var savedOtp = await _otpService.GetOtpAsync(userEmail);
        if (savedOtp!= otp)
        {
            throw new BadRequestException("Otp Not Match");
        }

        return savedOtp == otp;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
    {
      
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            throw new NotFoundException("User not found.");

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, resetToken, dto.NewPassword);
        if (!result.Succeeded)
            throw new BadRequestException("Failed to reset password. Please try again.");
        return true;
    }

    public async Task<bool> LogoutAsync(string refreshToken)
    {
        var token = await _unitOfWork.refreshToken.GetByTokenAsync(refreshToken);
        if (token == null)
            throw new NotFoundException("Refresh token not found.");

        await _unitOfWork.refreshToken.DeleteAsync(token);
     var result=   await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new BadRequestException("Failed to delete refresh token. Please try again.");

        return true;
    }

    // Helper
    private string GenerateOtp()
    {
        Random random = new();
        return random.Next(100000, 999999).ToString();
    }
}
