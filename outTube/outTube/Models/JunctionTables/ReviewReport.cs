using outTube.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace outTube.Models.JunctionTables;

public class ReviewReport
{
    [Key]
    [MaxLength(450)]
    public string ReportId { get; set; }

    [ForeignKey("ReportId")]
    public Report Report { get; set; }

    [Required]
    [MaxLength(450)]
    public string AdminId { get; set; }

    [ForeignKey("AdminId")]
    public User Admin { get; set; }

    public DateTime ReviewedAt { get; set; } = DateTime.UtcNow;
}