using FluentValidation;

public class UserRegisterValidator : AbstractValidator<UserRegisterViewModel>
{
    public UserRegisterValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .Length(2, 255).WithMessage("First name must be between 2 and 255 characters.")
            .Matches(@"^[a-zA-Z\s\-']+$").WithMessage("First name can only contain letters, spaces, hyphens, and apostrophes.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Length(2, 255).WithMessage("Last name must be between 2 and 255 characters.")
            .Matches(@"^[a-zA-Z\s\-']+$").WithMessage("Last name can only contain letters, spaces, hyphens, and apostrophes.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(256);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .MaximumLength(100)
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one digit.")
            .Matches(@"[@$!%*?&]").WithMessage("Password must contain at least one special character.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Please confirm your password.")
            .Equal(x => x.Password).WithMessage("Passwords do not match.");

        RuleFor(x => x.DateOfBirth)
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Date of birth cannot be in the future.")
            .Must(BeAtLeast13YearsOld).WithMessage("You must be at least 13 years old.")
            .When(x => x.DateOfBirth.HasValue);

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[\d\s\-\(\)]{7,50}$").WithMessage("Invalid phone number format.")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Invalid gender selection.")
            .When(x => x.Gender.HasValue);

        RuleFor(x => x.ImageUrl)
            .Must(BeAValidUrl).WithMessage("Invalid URL format.")
            .MaximumLength(255)
            .When(x => !string.IsNullOrEmpty(x.ImageUrl));
    }

    private bool BeAtLeast13YearsOld(DateTime? dateOfBirth)
    {
        if (!dateOfBirth.HasValue) return true;
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Value.Year;
        if (dateOfBirth.Value.Date > today.AddYears(-age)) age--;
        return age >= 13;
    }

    private bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrEmpty(url)) return true;
        return Uri.TryCreate(url, UriKind.Absolute, out var uri)
               && uri.Scheme == "https";
    }
}