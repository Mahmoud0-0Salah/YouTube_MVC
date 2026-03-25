using System.ComponentModel.DataAnnotations;

public class SafeUrlAttribute : ValidationAttribute
{
    private readonly string[] _allowedSchemes;

    public SafeUrlAttribute(string[]? allowedSchemes = null)
    {
        _allowedSchemes = allowedSchemes ?? new[] { "https" };
        ErrorMessage = $"URL must use one of the following schemes: {string.Join(", ", _allowedSchemes)}";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string urlString)
        {
            if (string.IsNullOrWhiteSpace(urlString))
                return ValidationResult.Success;

            if (!Uri.TryCreate(urlString, UriKind.Absolute, out var uri))
                return new ValidationResult("Invalid URL format.");

            if (!_allowedSchemes.Contains(uri.Scheme.ToLowerInvariant()))
                return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}