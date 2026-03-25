using System.ComponentModel.DataAnnotations;

public class NotSelfReferenceAttribute : ValidationAttribute
{
    private readonly string _otherProperty;

    public NotSelfReferenceAttribute(string otherProperty)
    {
        _otherProperty = otherProperty;
        ErrorMessage = "Cannot reference yourself.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherProperty);

        if (otherPropertyInfo == null)
            return new ValidationResult($"Unknown property: {_otherProperty}");

        var otherValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);

        if (value != null && value.Equals(otherValue))
            return new ValidationResult(ErrorMessage);

        return ValidationResult.Success;
    }
}