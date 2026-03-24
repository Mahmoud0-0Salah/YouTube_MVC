using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ReviewReport
{
    [Key]
    [MaxLength(256)]
    public string ReportId { get; set; }

    [ForeignKey("ReportId")]
    public Report Report { get; set; }

    [MaxLength(256)]
    public string AdminId { get; set; }

    [ForeignKey("AdminId")]
    public User Admin { get; set; }

    public DateTime ReviewedAt { get; set; } = DateTime.UtcNow;
}