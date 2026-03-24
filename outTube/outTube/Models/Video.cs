
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Video
{
    [Key]
    [MaxLength(256)]
    public string VideoId { get; set; } = Guid.NewGuid().ToString();

    [MaxLength(256)]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(255)]
    public string Title { get; set; }

    [MaxLength(255)]
    public string? Description { get; set; }

    [MaxLength(255)]
    public string VideoUrl { get; set; }

    [MaxLength(255)]
    public string? ThumbnailUrl { get; set; }

    public bool Visible { get; set; } = true;

    public TimeSpan Duration { get; set; }

    
    public ICollection<LikeVideo> Likes { get; set; } = new List<LikeVideo>();
    public ICollection<WatchVideo> Views { get; set; } = new List<WatchVideo>();
    public ICollection<UserCreateComment> Comments { get; set; } = new List<UserCreateComment>();
    public ICollection<UserCreateReport> Reports { get; set; } = new List<UserCreateReport>();
}