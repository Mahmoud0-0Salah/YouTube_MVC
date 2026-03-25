using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ourTube.Repositories.Interfaces;
using outTube.ViewModels.Video;
using OurTube.Repositories.Interfaces;
using outTube.Models;
using System.Security.Claims;
using TagLib;

namespace OurTube.Controllers
{
    public class VideoController : Controller
    {
        private readonly IVideoRepo videoRepository;
        private readonly IWebHostEnvironment webHost;

        public VideoController(IVideoRepo videoRepository, IWebHostEnvironment webHost)
        {
            this.videoRepository = videoRepository;
            this.webHost = webHost;
        }

        public IActionResult Index()
        {
            List<Video> videos = videoRepository.GetAll();
            return View(videos);
        }

        public IActionResult Details(string id)
        {
            Video video = videoRepository.GetByCondition(v => v.VideoId == id).FirstOrDefault();
            if (video == null)
            {
                return NotFound();
            }
            return View(video);
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
    }
}
