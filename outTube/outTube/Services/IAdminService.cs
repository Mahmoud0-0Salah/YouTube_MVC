using outTube.Models;
using outTube.Models.Enums;

namespace outTube.Services
{
    public interface IAdminService
    {
        Task<AdminDashboardStats> GetDashboardStatsAsync();
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<Video>> GetAllVideosAsync();
        Task<IEnumerable<Report>> GetReportsByStatusAsync(ReportStatus status);
        Task<bool> ToggleVideoVisibilityAsync(string videoId);
        Task<bool> BanUserAsync(string userId, string adminId, string reason);
        Task<bool> UnbanUserAsync(string userId);
        Task<bool> ReviewReportAsync(string reportId, string adminId, ReportStatus status);
    }

    public class AdminDashboardStats
    {
        public int TotalUsers { get; set; }
        public int TotalVideos { get; set; }
        public int TotalViews { get; set; }
        public int PendingReports { get; set; }
        public IEnumerable<VideoStats> TopVideos { get; set; }
    }

    public class VideoStats
    {
        public string Title { get; set; }
        public int ViewCount { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}
