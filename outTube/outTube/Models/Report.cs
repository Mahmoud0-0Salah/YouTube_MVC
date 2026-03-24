using outTube.Models.Enums;
using System.ComponentModel.DataAnnotations;

public class Report
{
    [Key]
    [MaxLength(256)]
    public string ReportId { get; set; } = Guid.NewGuid().ToString();

    [MaxLength(255)]
    public string Reason { get; set; }

    [MaxLength(255)]
    public string? Description { get; set; }

    public ReportStatus Status { get; set; } = ReportStatus.Pending;


    public UserCreateReport UserCreateReport { get; set; }
    public ReviewReport? ReviewReport { get; set; }
}

