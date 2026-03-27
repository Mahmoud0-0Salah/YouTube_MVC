using outTube.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace outTube.Models.JunctionTables;

public class UserSubscriber : IValidatableObject
{
    [Key]
    public int Id { get; set; }

    [MaxLength(450)]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    [MaxLength(450)]
    [NotSelfReference("UserId")]
    public string SubscriberId { get; set; }

    [ForeignKey("SubscriberId")]
    public User Subscriber { get; set; }

    public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (UserId == SubscriberId)
        {
            yield return new ValidationResult(
                "A user cannot subscribe to themselves.",
                new[] { nameof(SubscriberId) });
        }
    }
}