namespace ourTube.ViewModels.Report;

using outTube.Models.Enums;
using System.ComponentModel.DataAnnotations;

public class ReportReviewViewModel
{
    [Required]
    public string ReportId { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    [Display(Name = "Status")]
    public ReportStatus Status { get; set; }

    public string? Reason { get; set; }
    public string? Description { get; set; }
    public string? VideoTitle { get; set; }
    public string? ReporterName { get; set; }
    public DateTime? ReportedAt { get; set; }
}