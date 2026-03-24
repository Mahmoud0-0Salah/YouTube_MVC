using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserSubscriber
{
    [MaxLength(256)]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    [MaxLength(256)]
    public string SubscriberId { get; set; }

    [ForeignKey("SubscriberId")]
    public User Subscriber { get; set; }

    public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
}