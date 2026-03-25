using FluentValidation;

public class CommentCreateValidator : AbstractValidator<CommentCreateViewModel>
{
    public CommentCreateValidator()
    {
        RuleFor(x => x.VideoId)
            .NotEmpty().WithMessage("Video ID is required.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Comment cannot be empty.")
            .Length(1, 255).WithMessage("Comment must be between 1 and 255 characters.")
            .Must(NotContainHtml).WithMessage("Comment cannot contain HTML tags.")
            .Must(NotBeOnlyWhitespace).WithMessage("Comment cannot be only whitespace.")
            .Must(NotContainProfanity).WithMessage("Comment contains inappropriate language.");
    }

    private bool NotContainHtml(string? value)
    {
        if (string.IsNullOrEmpty(value)) return true;
        return !System.Text.RegularExpressions.Regex.IsMatch(value, "<.*?>");
    }

    private bool NotBeOnlyWhitespace(string? value)
    {
        return !string.IsNullOrWhiteSpace(value);
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
}