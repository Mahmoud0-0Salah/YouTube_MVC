using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using outTube.Models;
using outTube.Models.Enums;
using outTube.Services;

namespace outTube.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly UserManager<User> _userManager;

        public AdminController(IAdminService adminService, UserManager<User> userManager)
        {
            _adminService = adminService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var stats = await _adminService.GetDashboardStatsAsync();
            return View(stats);
        }

        public async Task<IActionResult> Users()
        {
            var users = await _adminService.GetAllUsersAsync();
            return View(users);
        }

        public async Task<IActionResult> Videos()
        {
            var videos = await _adminService.GetAllVideosAsync();
            return View(videos);
        }

        public async Task<IActionResult> Reports(ReportStatus status = ReportStatus.Pending)
        {
            var reports = await _adminService.GetReportsByStatusAsync(status);
            ViewBag.CurrentStatus = status;
            return View(reports);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleVisibility(string videoId)
        {
            await _adminService.ToggleVideoVisibilityAsync(videoId);
            return RedirectToAction(nameof(Videos));
        }

        [HttpPost]
        public async Task<IActionResult> BanUser(string userId, string reason)
        {
            var adminId = _userManager.GetUserId(User);
            await _adminService.BanUserAsync(userId, adminId, reason);
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> UnbanUser(string userId)
        {
            await _adminService.UnbanUserAsync(userId);
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> ReviewReport(string reportId, ReportStatus status)
        {
            var adminId = _userManager.GetUserId(User);
            await _adminService.ReviewReportAsync(reportId, adminId, status);
            return RedirectToAction(nameof(Reports));
        }
    }
}
