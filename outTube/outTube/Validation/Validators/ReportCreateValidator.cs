using FluentValidation;

public class ReportCreateValidator : AbstractValidator<ReportCreateViewModel>
{
    public ReportCreateValidator()
    {
        RuleFor(x => x.VideoId)
            .NotEmpty().WithMessage("Video ID is required.");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason is required.")
            .Length(5, 255).WithMessage("Reason must be between 5 and 255 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Description cannot exceed 255 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}