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

			return View(model);
		}


		public IActionResult Trending(int page = 1)
		{
			var model = _videoRepo.GetTrendingVideosInfo(page);

			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
			{
				return PartialView("_VideoCards", model);
			}

			return View("Index", model);
		}


		public IActionResult Lastest(int page = 1)
		{
			var model = _videoRepo.GetLastestVideosInfo(page);

			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
			{
				return PartialView("_VideoCards", model);
			}

			return View("Index", model);
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
