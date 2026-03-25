using System.ComponentModel.DataAnnotations;

public class BanUserViewModel
{
    [Required(ErrorMessage = "User ID is required.")]
    [Display(Name = "User to Ban")]
    public string UserId { get; set; }

    // Display fields
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public int ReportCount { get; set; }
}