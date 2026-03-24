using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class WatchVideo
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(450)]
    public string VideoId { get; set; }

    [ForeignKey("VideoId")]
    public Video Video { get; set; }

    [Required]
    [MaxLength(450)]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    public DateTime WatchedAt { get; set; } = DateTime.UtcNow;
}