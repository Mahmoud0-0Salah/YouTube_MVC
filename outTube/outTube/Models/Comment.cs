using outTube.Models.JunctionTables;
using System.ComponentModel.DataAnnotations;

namespace outTube.Models;

public class Comment
{
    [Key]
    [MaxLength(450)]
    public string CommentId { get; set; } = Guid.NewGuid().ToString();

    [MaxLength(255)]
    public string Content { get; set; }

    public DateTime? UpdatedAt { get; set; }

 
    public UserCreateComment UserCreateComment { get; set; }
}