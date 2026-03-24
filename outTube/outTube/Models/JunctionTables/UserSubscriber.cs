using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserSubscriber
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(450)]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    [Required]
    [MaxLength(450)]
    public string SubscriberId { get; set; }

    [ForeignKey("SubscriberId")]
    public User Subscriber { get; set; }

    public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
}