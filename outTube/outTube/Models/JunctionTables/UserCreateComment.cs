using outTube.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace outTube.Models.JunctionTables;

public class UserCreateComment
{
    [Key]
    [MaxLength(450)]
    public string CommentId { get; set; }

    [ForeignKey("CommentId")]
    public Comment Comment { get; set; }

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

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}