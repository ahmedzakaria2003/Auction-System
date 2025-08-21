using System.ComponentModel.DataAnnotations;

public class VerifyOtpDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "OTP is required")]
    public string Otp { get; set; } = default!;
}
