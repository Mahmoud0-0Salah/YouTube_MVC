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
            Video video = videoRepository.GetByCondition(v => v.VideoId == id).FirstOrDefault();
            if (video == null)
            {
                return NotFound();
            }
            var user = await userManager.GetUserAsync(User);
            WatchVideo watchVideo = new WatchVideo()
            {
                VideoId = video.VideoId,
                UserId = user.Id,
                WatchedAt = DateTime.Now
            };
            watchVideoRepo.Create(watchVideo);
            watchVideoRepo.Save();
            VideoGetViewModel model = new VideoGetViewModel()
            {
                Id = video.VideoId,
                Title = video.Title,
                Description = video.Description ?? string.Empty,
                Channel = video.User != null ? $"{video.User.FirstName} {video.User.LastName}" : "Unknown User",
                Views = watchVideoRepo.GetByCondition(wv => wv.VideoId == video.VideoId).Count(),
                Time = video.CreatedAt.ToString("yyyy-MM-dd"),
                Thumb = video.ThumbnailUrl,
                Avatar = user.ImageUrl,
                Duration = video.Duration.ToString(@"hh\:mm\:ss"),
                VideoUrl = video.VideoUrl
            };
            return View(model);
        }


        public IActionResult Create()
        {
            return View(new VideoCreateViewModel());
        }
        [HttpPost]
        [RequestSizeLimit(500_000_000)]
        public async Task<IActionResult> Create(VideoCreateViewModel model)
        {
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

        public IActionResult Delete(string id)
        {
            Video video = videoRepository.GetByCondition(v => v.VideoId == id).FirstOrDefault();
            if (video == null)
            {
                return NotFound();
            }
            videoRepository.Delete(video);
            videoRepository.Save();
            return RedirectToAction("Index", "Home");
        }
    }
}
