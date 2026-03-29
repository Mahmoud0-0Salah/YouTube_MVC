using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ourTube.Repositories.Interfaces;
using ourTube.ViewModels;
using outTube.Data;
using outTube.Models;

namespace outTube.Controllers
{
    public class ChannelController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IVideoRepo _videoRepo;

        public ChannelController(UserManager<User> userManager, ApplicationDbContext context, IVideoRepo videoRepo)
        {
            _userManager = userManager;
            _context = context;
            _videoRepo = videoRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string id, int page = 1)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _context.Users
                .Include(u => u.Subscribers)
                .Include(u => u.Videos)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            // Get current logged-in user id (may be null if not authenticated)
            string currentUserId = null;
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                currentUserId = currentUser?.Id;
            }

            var videos = _videoRepo.GetYourVideosInfo(id, page);

            var model = new ChannelPageViewModel
            {
                UserId = user.Id,
                ChannelName = user.FirstName + " " + user.LastName,
                Handle = "@" + (user.UserName ?? "").ToLower(),
                ImageUrl = user.ImageUrl ?? string.Empty,
                SubscriberCount = user.Subscribers?.Count ?? 0,
                VideoCount = user.Videos?.Count ?? 0,
                JoinedDate = user.CreatedAt,
                IsOwnChannel = currentUserId == user.Id,
                IsSubscribed = currentUserId != null && user.Subscribers != null && user.Subscribers.Any(s => s.SubscriberId == currentUserId),
                Videos = videos
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_VideoCards", videos);
            }

            return View(model);
        }
    }
}
