using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ourTube.Repositories.Interfaces;
using outTube.ViewModels.Video;
using OurTube.Repositories.Interfaces;
using outTube.Models;
using System.Security.Claims;
using TagLib;
using outTube.Models.JunctionTables;
using ourTube.ViewModels.Video;
using Microsoft.AspNetCore.Authorization;

namespace OurTube.Controllers
{
    public class VideoController : Controller
    {
        private readonly IVideoRepo videoRepository;
        private readonly IWatchVideoRepo watchVideoRepo;
        private readonly UserManager<User> userManager;
        private readonly IWebHostEnvironment webHost;

        public VideoController(IVideoRepo videoRepository,
                               IWatchVideoRepo watchVideoRepo,
                               UserManager<User> userManager,
                               IWebHostEnvironment webHost)
        {
            this.videoRepository = videoRepository;
            this.watchVideoRepo = watchVideoRepo;
            this.userManager = userManager;
            this.webHost = webHost;
        }

        public IActionResult Index()
        {
            List<Video> videos = videoRepository.GetAll().ToList();
            return View(videos);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var user = await userManager.GetUserAsync(User);
            VideoDetailsViewModel video = videoRepository.GetVideoDetails(id, user?.Id);
            if (video == null)
            {
                return NotFound();
            }
            return View(video);
        }


        [Authorize]
        public IActionResult Create()
        {
            return View(new VideoCreateViewModel());
        }

        [Authorize]
        [HttpPost]
        [RequestSizeLimit(500_000_000)]
        public async Task<IActionResult> Create(VideoCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Video video = new Video();
            video.VideoId = Guid.NewGuid().ToString();
            video.CreatedAt = DateTime.Now;
            string videoName = $"{video.VideoId}{Path.GetExtension(model.Source.FileName)}";
            string thumnailName = $"{video.VideoId}{Path.GetExtension(model.Thumbnail.FileName)}";

            string videosDir = Path.Combine(webHost.WebRootPath, "videos");
            if (!Directory.Exists(videosDir)) Directory.CreateDirectory(videosDir);

            string thumbDir = Path.Combine(webHost.WebRootPath, "images", "VideosThumnails");
            if (!Directory.Exists(thumbDir)) Directory.CreateDirectory(thumbDir);

            string videoPath = Path.Combine(videosDir, videoName);
            string thumbnailPath = Path.Combine(thumbDir, thumnailName);

            using (var stream = new FileStream(videoPath, FileMode.Create))
            {
                await model.Source.CopyToAsync(stream);
            }
            using (var stream = new FileStream(thumbnailPath, FileMode.Create))
            {
                await model.Thumbnail.CopyToAsync(stream);
            }
            video.Title = model.Title;
            video.Description = model.Description;
            video.VideoUrl = $"/videos/{videoName}";
            video.ThumbnailUrl = $"/images/VideosThumnails/{thumnailName}";
            video.Duration = TagLib.File.Create(videoPath).Properties.Duration;
            video.Visible = model.Visible;
            video.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            videoRepository.Create(video);
            videoRepository.Save();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult YourVideos(int page = 1)
        {
            if (page < 1)
            {
                return BadRequest();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = videoRepository.GetYourVideosInfo(userId, page);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_YourVideoCards", model);
            }

            return View(model);
        }

        [Authorize]
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Video video = videoRepository.GetByCondition(v => v.VideoId == id && v.UserId == userId).FirstOrDefault();
            if (video == null)
            {
                return NotFound();
            }
            videoRepository.Delete(video);
            videoRepository.Save();

            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] ReportCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid data provided." });
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null) return Unauthorized(new { message = "You must be logged in to report a video." });

            bool isCreated = videoRepository.CreateReport(model, user.Id);
            if (!isCreated)
            {
                return BadRequest(new { message = "You have already reported this video." });
            }

            if(videoRepository.Save() > 0)
            {
                return Ok(new { message = "Report submitted successfully." });
            }

            return BadRequest(new { message = "Failed to submit report. Please try again." });
        }


		[Authorize]
		public IActionResult YourHistory(int page = 1)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var model = watchVideoRepo.GetWatchedVideosInfo(userId, page);

			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
			{
				return PartialView("_VideoCards", model);
			}

			return View(model);
		}
	}
}
