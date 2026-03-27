using Microsoft.EntityFrameworkCore;
using ourTube.ViewModels;
using ourTube.ViewModels.Video;
using OurTube.Repositories;
using outTube.Data;
using outTube.Models;
using outTube.Models.JunctionTables;

namespace ourTube.Repositories.Interfaces
{
	public class LikeRepo : Repository<LikeVideo>, ILikeRepo
	{
		public LikeRepo(ApplicationDbContext context) : base(context)
		{
		}

		private static VideoGetViewModel GetVideoGetViewModel(Video video)
		{
			return new VideoGetViewModel
			{
				Id = video.VideoId,
				Title = video.Title,
				Channel = video.User.FirstName + " " + video.User.LastName,
				Description = video.Description ?? string.Empty,
				Duration = video.Duration.ToString(@"hh\:mm\:ss"),
				Time = video.CreatedAt.ToString("MMM dd, yyyy"),
				Thumb = video.ThumbnailUrl,
				Avatar = video.User.ImageUrl,
				VideoUrl = video.VideoUrl,
				Views = video.Views.Count
			};
		}

		public PaginatedList<VideoGetViewModel> GetLikedVideosInfo(string useerId, int page = 1, int pageSize = 8)
		{
			List<VideoGetViewModel> items = GetByCondition(l=>l.UserId == useerId)
			   .Include(l=>l.Video)
			   .ThenInclude(v=>v.User)
			   .Where(Video => Video.Video.Visible)
			   .Select(l => GetVideoGetViewModel(l.Video))
			   .Skip((page - 1) * pageSize)
			   .Take(pageSize)
			   .ToList();

			return new PaginatedList<VideoGetViewModel>(items, GetByCondition(l=>l.UserId==useerId)
				.Include(l=>l.Video)
				.Where(l=>l.Video.Visible)
				.Count(), page, pageSize);
		}

	}
}
