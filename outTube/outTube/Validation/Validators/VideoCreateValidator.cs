using FluentValidation;
using outTube.ViewModels.Video;

public class VideoCreateValidator : AbstractValidator<VideoCreateViewModel>
{
    public VideoCreateValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(3, 255).WithMessage("Title must be between 3 and 255 characters.")
            .Must(NotContainHtml).WithMessage("Title cannot contain HTML tags.")
            .Must(NotContainProfanity).WithMessage("Title contains inappropriate language.");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Description cannot exceed 255 characters.")
            .Must(NotContainHtml).WithMessage("Description cannot contain HTML tags.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Source)
            .NotNull().WithMessage("Video file is required.")
            .Must(BeAValidVideoFile).WithMessage("Only .mp4, .avi, .mkv, .mov, .webm files are allowed.")
            .Must(BeWithinVideoSizeLimit).WithMessage("Video file cannot exceed 500 MB.");

        RuleFor(x => x.Thumbnail)
            .Must(BeAValidImageFile).WithMessage("Only .jpg, .jpeg, .png, .webp files are allowed.")
            .Must(BeWithinImageSizeLimit).WithMessage("Thumbnail cannot exceed 5 MB.")
            .When(x => x.Thumbnail != null);
    }

    private bool NotContainHtml(string? value)
    {
        if (string.IsNullOrEmpty(value)) return true;
        return !System.Text.RegularExpressions.Regex.IsMatch(value, "<.*?>");
    }

    private bool NotContainProfanity(string? value)
    {
        if (string.IsNullOrEmpty(value)) return true;
        var profanityList = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "badword1", "badword2"
        };
        var words = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return !words.Any(w => profanityList.Contains(w.Trim()));
    }

    private bool BeAValidVideoFile(IFormFile? file)
    {
        if (file == null) return false;
        var allowedExtensions = new[] { ".mp4", ".avi", ".mkv", ".mov", ".webm" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return allowedExtensions.Contains(extension);
    }

    private bool BeWithinVideoSizeLimit(IFormFile? file)
    {
        if (file == null) return false;
        return file.Length <= 500 * 1024 * 1024; // 500 MB
    }

    private bool BeAValidImageFile(IFormFile? file)
    {
        if (file == null) return true;
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return allowedExtensions.Contains(extension);
    }

    private bool BeWithinImageSizeLimit(IFormFile? file)
    {
        if (file == null) return true;
        return file.Length <= 5 * 1024 * 1024; // 5 MB
    }
}