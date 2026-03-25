using System.ComponentModel.DataAnnotations;

public class FutureDateAttribute : ValidationAttribute
{
    public FutureDateAttribute()
    {
        ErrorMessage = "Date cannot be in the future.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime date)
        {
            if (date.Date > DateTime.Today)
                return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}