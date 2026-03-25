using System.ComponentModel.DataAnnotations;

public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSizeInMB;

    public MaxFileSizeAttribute(int maxFileSizeInMB)
    {
        _maxFileSizeInMB = maxFileSizeInMB;
        ErrorMessage = $"File size cannot exceed {_maxFileSizeInMB} MB.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            if (file.Length > _maxFileSizeInMB * 1024 * 1024)
                return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}