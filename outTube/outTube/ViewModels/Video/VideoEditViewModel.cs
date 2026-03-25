using System.ComponentModel.DataAnnotations;

public class VideoEditViewModel
{
    [Required]
    public string VideoId { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 255 characters.")]
    [NoProfanity]
    [Display(Name = "Title")]
    public string Title { get; set; }

    [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
    [NoProfanity]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [SafeUrl]
    [Display(Name = "Current Thumbnail")]
    public string? CurrentThumbnailUrl { get; set; }

    [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png", ".webp" })]
    [MaxFileSize(5)]
    [Display(Name = "New Thumbnail")]
    public IFormFile? ThumbnailFile { get; set; }

    [Display(Name = "Visible")]
    public bool Visible { get; set; }
}