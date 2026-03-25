using outTube.Models.Enums;
using System.ComponentModel.DataAnnotations;

public class UserRegisterViewModel
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 255 characters.")]
    [RegularExpression(@"^[a-zA-Z\s\-']+$", ErrorMessage = "First name can only contain letters, spaces, hyphens, and apostrophes.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 255 characters.")]
    [RegularExpression(@"^[a-zA-Z\s\-']+$", ErrorMessage = "Last name can only contain letters, spaces, hyphens, and apostrophes.")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(256, ErrorMessage = "Email cannot exceed 256 characters.")]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm password is required.")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; }

    [Phone(ErrorMessage = "Invalid phone number.")]
    [StringLength(50, ErrorMessage = "Phone number cannot exceed 50 characters.")]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Gender")]
    public Gender? Gender { get; set; }

    [FutureDate]
    [MinimumAge(13)]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateTime? DateOfBirth { get; set; }

    [SafeUrl]
    [StringLength(255, ErrorMessage = "Image URL cannot exceed 255 characters.")]
    [Display(Name = "Profile Image URL")]
    public string? ImageUrl { get; set; }
}