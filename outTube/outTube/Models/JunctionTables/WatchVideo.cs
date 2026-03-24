using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class WatchVideo
{
    [MaxLength(256)]
    public string VideoId { get; set; }

    [ForeignKey("VideoId")]
    public Video Video { get; set; }

    [MaxLength(256)]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    public DateTime WatchedAt { get; set; } = DateTime.UtcNow;
}