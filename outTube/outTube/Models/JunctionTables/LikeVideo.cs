using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class LikeVideo
{
    [Key]
    public int Id { get; set; }

    [MaxLength(450)]
    public string VideoId { get; set; }

    [ForeignKey("VideoId")]
    public Video Video { get; set; }

    [MaxLength(450)]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}