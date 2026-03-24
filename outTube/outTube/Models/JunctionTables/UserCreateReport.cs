using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserCreateReport
{
    [Key]
    [MaxLength(450)]
    public string ReportId { get; set; }

    [ForeignKey("ReportId")]
    public Report Report { get; set; }

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

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}