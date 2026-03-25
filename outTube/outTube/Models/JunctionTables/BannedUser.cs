using outTube.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BannedUser : IValidatableObject
{
    [Key]
    public int Id { get; set; }

    [MaxLength(450)]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    [MaxLength(450)]
    [NotSelfReference("UserId")]
    public string AdminId { get; set; }

    [ForeignKey("AdminId")]
    public User Admin { get; set; }

    public DateTime BannedAt { get; set; } = DateTime.UtcNow;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (UserId == AdminId)
        {
            yield return new ValidationResult(
                "An admin cannot ban themselves.",
                new[] { nameof(UserId) });
        }
    }
}