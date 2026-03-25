using System.ComponentModel.DataAnnotations;

public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;

    public AllowedExtensionsAttribute(string[] extensions)
    {
        _extensions = extensions;
        ErrorMessage = $"Only the following file extensions are allowed: {string.Join(", ", extensions)}";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_extensions.Contains(extension))
                return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}