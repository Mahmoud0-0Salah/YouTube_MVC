using System.ComponentModel.DataAnnotations;

public class NoProfanityAttribute : ValidationAttribute
{
    private static readonly HashSet<string> ProfanityList = new(StringComparer.OrdinalIgnoreCase)
    {
        "badword1", "badword2", "badword3"
    };

    public NoProfanityAttribute()
    {
        ErrorMessage = "Content contains inappropriate language.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string text)
        {
            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                if (ProfanityList.Contains(word.Trim()))
                    return new ValidationResult(ErrorMessage);
            }
        }

        return ValidationResult.Success;
    }
}