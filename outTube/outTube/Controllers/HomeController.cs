using Microsoft.AspNetCore.Mvc;
using ourTube.Repositories.Interfaces;
using ourTube.ViewModels;
using ourTube.ViewModels.Video;
using outTube.Models;
using System.Diagnostics;

namespace outTube.Controllers
{
    public class HomeController : Controller
    {
		private readonly IVideoRepo _videoRepo;

		public HomeController(IVideoRepo videoRepo)
		{
			_videoRepo = videoRepo;
		}
		public IActionResult Index(int page = 1)
		{
			var model = _videoRepo.GetVideosInfo(page);

			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
			{
				return PartialView("_VideoCards", model);
			}

			return View("VideosGrid", model);
		}


		public IActionResult Trending(int page = 1)
		{
			var model = _videoRepo.GetTrendingVideosInfo(page);

			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
			{
				return PartialView("_VideoCards", model);
			}

			return View("VideosGrid", model);
		}


		public IActionResult Lastest(int page = 1)
		{
			var model = _videoRepo.GetLastestVideosInfo(page);

			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
			{
				return PartialView("_VideoCards", model);
			}

			return View("VideosGrid", model);
		}


		public IActionResult Search(string query, int page = 1)
		{
			if (string.IsNullOrWhiteSpace(query))
			{
				return RedirectToAction("Index");
			}

			var videos = _videoRepo.SearchVideos(query, page);
			var channels = page == 1 ? _videoRepo.SearchChannels(query) : new List<ChannelSearchViewModel>();

			var model = new SearchViewModel
			{
				Query = query,
				Videos = videos,
				Channels = channels
			};

			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
			{
				return PartialView("_VideoCards", videos);
			}

			return View(model);
		}

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
