using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ourTube.Repositories;
using ourTube.Repositories.Interfaces;
using outTube.Models;
using System.Numerics;

namespace ourTube.Controllers
{
	[Authorize]
	public class SubscriptionController : Controller
	{
		private readonly IUserSubscriberRepo _userSubscriberRepo;
		private readonly UserManager<User> _userManager;
		public SubscriptionController(IUserSubscriberRepo userSubscriberRepo,UserManager<User> userManager)
		{
			_userSubscriberRepo = userSubscriberRepo;
			_userManager = userManager;
		}
		public IActionResult GetSubscripedVideos(int page=1)
		{
			string userId = _userManager.GetUserId(User);
			var model = _userSubscriberRepo.GetLikedVideosInfo(userId,page);

			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
			{
				return PartialView("_VideoCards", model);
			}

			return View("VideosGrid", model);
		}
	}
}
