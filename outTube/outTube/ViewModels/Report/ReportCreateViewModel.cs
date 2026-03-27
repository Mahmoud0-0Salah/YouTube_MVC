using System.ComponentModel.DataAnnotations;

public class ReportCreateViewModel
{
    [Required]
    public string VideoId { get; set; }

    [Required(ErrorMessage = "Reason is required.")]
    [StringLength(255, MinimumLength = 5, ErrorMessage = "Reason must be between 5 and 255 characters.")]
    [Display(Name = "Reason")]
    public string Reason { get; set; }

    [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
    [Display(Name = "Description")]
    public string? Description { get; set; }
}