using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BannedUser
{
    [Key]
    public int Id { get; set; }

    [MaxLength(450)]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    [MaxLength(450)]
    public string AdminId { get; set; }

    [ForeignKey("AdminId")]
    public User Admin { get; set; }

    public DateTime BannedAt { get; set; } = DateTime.UtcNow;
}