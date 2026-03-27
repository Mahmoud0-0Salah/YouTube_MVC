using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ourTube.Hubs;
using ourTube.Repositories.Interfaces;
using outTube.Models;
using outTube.Models.JunctionTables;

namespace ourTube.Controllers
{
	[Authorize]
	public class LikeController : Controller
	{

		private readonly ILikeRepo _likeRepo;

		private readonly IHubContext<LikeHub> _likeHub;
		private readonly UserManager<User> _userManager;
		public LikeController(ILikeRepo likeRepo,IHubContext<LikeHub> likeHub,UserManager<User> userManager)
		{
			_likeRepo = likeRepo;
			_likeHub = likeHub;
			_userManager = userManager;
		}
		public async Task<IActionResult> LikeToggle(string videoId)
		{
			string userId = _userManager.GetUserId(User);
			LikeVideo? Like = _likeRepo.GetByCondition(l => l.UserId == userId && l.VideoId ==videoId).SingleOrDefault();
			if ((Like == null))
				_likeRepo.Create(new LikeVideo { UserId = userId, VideoId = videoId, CreatedAt = DateTime.Now });
			else
				_likeRepo.Delete(Like);

			_likeRepo.Save();

			var likeCount = _likeRepo
			 .GetByCondition(l => l.VideoId == videoId)
			 .Count();

			await  _likeHub.Clients.All.SendAsync("ReceiveLike", videoId, likeCount);


			return Ok(new { likes = likeCount });
		}
	}
}
