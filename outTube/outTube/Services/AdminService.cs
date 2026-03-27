using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using outTube.Data;
using outTube.Models;
using outTube.Models.Enums;
using outTube.Models.JunctionTables;

namespace outTube.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public AdminService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<AdminDashboardStats> GetDashboardStatsAsync()
        {
            return new AdminDashboardStats
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalVideos = await _context.Videos.IgnoreQueryFilters().CountAsync(),
                TotalViews = await _context.WatchVideos.CountAsync(),
                PendingReports = await _context.Reports.CountAsync(r => r.Status == ReportStatus.Pending),
                TopVideos = await _context.Videos
                    .OrderByDescending(v => v.Views.Count)
                    .Take(5)
                    .Select(v => new VideoStats
                    {
                        Title = v.Title,
                        ViewCount = v.Views.Count,
                        ThumbnailUrl = v.ThumbnailUrl
                    })
                    .ToListAsync()
            };
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.BansReceived)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Video>> GetAllVideosAsync()
        {
            return await _context.Videos
                .IgnoreQueryFilters()
                .Include(v => v.User)
                .Include(v => v.Views)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Report>> GetReportsByStatusAsync(ReportStatus status)
        {
            return await _context.Reports
                .IgnoreQueryFilters()
                .Include(r => r.UserCreateReport)
                    .ThenInclude(uc => uc.Video)
                        .ThenInclude(v => v.User) // Publisher
                .Include(r => r.UserCreateReport)
                    .ThenInclude(uc => uc.User) // Reporter
                .Where(r => r.Status == status)
                .OrderByDescending(r => r.UserCreateReport.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> ToggleVideoVisibilityAsync(string videoId)
        {
            var video = await _context.Videos.FindAsync(videoId);
            if (video == null) return false;

            video.Visible = !video.Visible;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BanUserAsync(string userId, string adminId, string reason)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var alreadyBanned = await _context.BannedUsers.AnyAsync(b => b.UserId == userId);
            if (alreadyBanned) return false;

            var ban = new BannedUser
            {
                UserId = userId,
                AdminId = adminId,
                Reason = reason,
                BannedAt = DateTime.UtcNow
            };

            // Set lockout to prevent login
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            await _userManager.UpdateSecurityStampAsync(user);
            await _userManager.UpdateAsync(user);

            _context.BannedUsers.Add(ban);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnbanUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var ban = await _context.BannedUsers.FirstOrDefaultAsync(b => b.UserId == userId);
            if (ban == null) return false;

            // Clear lockout
            await _userManager.SetLockoutEndDateAsync(user, null);
            await _userManager.UpdateSecurityStampAsync(user);
            await _userManager.UpdateAsync(user);

            _context.BannedUsers.Remove(ban);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReviewReportAsync(string reportId, string adminId, ReportStatus status)
        {
            var report = await _context.Reports.FindAsync(reportId);
            if (report == null) return false;

            report.Status = status;

            var review = new ReviewReport
            {
                ReportId = reportId,
                AdminId = adminId,
                ReviewedAt = DateTime.UtcNow
            };

            _context.ReviewReports.Add(review);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
