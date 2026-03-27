using outTube.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace outTube.Models.JunctionTables;

public class ReviewReport : IValidatableObject
{
    [Key]
    [MaxLength(450)]
    public string ReportId { get; set; }

    [ForeignKey("ReportId")]
    public Report Report { get; set; }

    [Required(ErrorMessage = "Admin ID is required.")]
    [MaxLength(450)]
    public string AdminId { get; set; }

    [ForeignKey("AdminId")]
    public User Admin { get; set; }

    public DateTime ReviewedAt { get; set; } = DateTime.UtcNow;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ReviewedAt > DateTime.UtcNow.AddMinutes(1))
        {
            yield return new ValidationResult(
                "Review date cannot be in the future.",
                new[] { nameof(ReviewedAt) });
        }
    }
}