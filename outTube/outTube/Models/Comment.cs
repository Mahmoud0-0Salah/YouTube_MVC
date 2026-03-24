using System.ComponentModel.DataAnnotations;

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