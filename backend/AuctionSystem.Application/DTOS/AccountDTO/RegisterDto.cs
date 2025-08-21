using System.ComponentModel.DataAnnotations;

public class RegisterDto
{
    [Required(ErrorMessage = "Username is required")]
    [RegularExpression(@"^[\p{L}\p{N}]+$", ErrorMessage = "Username can only contain letters or digits.")]

    public string UserName { get; set; } = default!;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; } = default!;

    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = default!;

    [Required(ErrorMessage = "User type is required (Seller or Bidder)")]
    [RegularExpression("^(Seller|Bidder)$", ErrorMessage = "UserType must be either 'Seller' or 'Bidder'")]
    public string UserType { get; set; } = default!;

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    public string PhoneNumber { get; set; } = default!;

    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; } = default!;

    [Required(ErrorMessage = "Full name is required")]
    public string FullName { get; set; } = default!;
}
