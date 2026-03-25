using System.ComponentModel.DataAnnotations;

public class VideoCreateViewModel
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 255 characters.")]
    [NoProfanity]
    [Display(Name = "Title")]
    public string Title { get; set; }

    [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
    [NoProfanity]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Video file is required.")]
    [AllowedExtensions(new[] { ".mp4", ".avi", ".mkv", ".mov", ".webm" })]
    [MaxFileSize(500)] // 500 MB
    [Display(Name = "Video File")]
    public IFormFile VideoFile { get; set; }

    [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png", ".webp" })]
    [MaxFileSize(5)] // 5 MB
    [Display(Name = "Thumbnail")]
    public IFormFile? ThumbnailFile { get; set; }

    [Display(Name = "Visible")]
    public bool Visible { get; set; } = true;
}