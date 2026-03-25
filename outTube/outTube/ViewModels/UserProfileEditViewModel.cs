using outTube.Models.Enums;
using System.ComponentModel.DataAnnotations;

public class UserProfileEditViewModel
{
    [Required]
    public string UserId { get; set; }

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

    [Phone(ErrorMessage = "Invalid phone number.")]
    [StringLength(50)]
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
    [StringLength(255)]
    [Display(Name = "Profile Image URL")]
    public string? ImageUrl { get; set; }

    [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" })]
    [MaxFileSize(5)] // 5 MB
    [Display(Name = "Profile Image")]
    public IFormFile? ImageFile { get; set; }
}