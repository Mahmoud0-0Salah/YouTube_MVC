using System.ComponentModel.DataAnnotations;

public class CommentCreateViewModel
{
    [Required]
    public string VideoId { get; set; }

    [Required(ErrorMessage = "Comment cannot be empty.")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Comment must be between 1 and 255 characters.")]
    [NoProfanity]
    [Display(Name = "Comment")]
    public string Content { get; set; }
    public string? UserId { get; internal set; }
}