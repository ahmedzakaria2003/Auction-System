using AuctionSystem.Application.Contracts;
using AuctionSystem.Application.Services.Contracts;
using AuctionSystem.Domain.Entities;

public class OtpService : IOtpService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public OtpService(IUnitOfWork unitOfWork, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task SaveOtpAsync(string userEmail, string otp)
    {
        var otpEntity = new Otp
        {
            UserEmail = userEmail,
            OtpCode = otp,
            ExpirationDate = DateTime.UtcNow.AddMinutes(5)
        };

        await _unitOfWork.OtpRepository.AddAsync(otpEntity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<string> GetOtpAsync(string userEmail)
    {
        var otp = await _unitOfWork.OtpRepository.GetLatestValidOtpAsync(userEmail);
        return otp?.OtpCode;
    }

    public async Task SendOtpToEmail(string userEmail, string otp)
    {
        var subject = "Your Verification Code (OTP)";
        var body = $@"
        <html>
            <body style='font-family: Arial, sans-serif; color: #333;'>
                <h2 style='color: #4CAF50;'>Verification Code</h2>
                <p>Dear user,</p>
                <p>We received a request to verify your email address. Please use the following One-Time Password (OTP) to proceed:</p>
                <p style='font-size: 20px; font-weight: bold; color: #4CAF50;'>{otp}</p>
                <p>This code is valid for <strong>5 minutes</strong>.</p>
                <p>If you did not request this, you can safely ignore this email.</p>
                <br />
                <p>Best regards,<br />Auction System Team</p>
            </body>
        </html>";

        await _emailService.SendEmailAsync(userEmail, subject, body);
    }
}
