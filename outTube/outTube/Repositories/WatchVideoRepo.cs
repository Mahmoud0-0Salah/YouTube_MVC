using Microsoft.EntityFrameworkCore;
using ourTube.Repositories.Interfaces;
using ourTube.ViewModels;
using ourTube.ViewModels.Video;
using OurTube.Repositories;
using OurTube.Repositories.Interfaces.Common;
using outTube.Data;
using outTube.Models;
using outTube.Models.JunctionTables;

namespace ourTube.Repositories
{
    public class WatchVideoRepo : Repository<WatchVideo>, IWatchVideoRepo
    {
        public WatchVideoRepo(ApplicationDbContext context) : base(context)
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
		public PaginatedList<VideoGetViewModel> GetWatchedVideosInfo(string userId, int page = 1, int pageSize = 8)
		{
			var pagedVideoIds = GetByCondition(w => w.UserId == userId)
				.GroupBy(w => w.VideoId)
				.Select(g => new { VideoId = g.Key, LastWatchedAt = g.Max(w => w.WatchedAt) })
				.OrderByDescending(x => x.LastWatchedAt)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(x => x.VideoId)
				.ToList();

			List<VideoGetViewModel> items = GetByCondition(w => w.UserId == userId && pagedVideoIds.Contains(w.VideoId))
				.Include(w => w.Video)
				.ThenInclude(v => v.User)
				.Include(w => w.Video)
				.ThenInclude(v => v.Views)
				.AsEnumerable()
				.OrderByDescending(w => w.WatchedAt)
				.DistinctBy(w => w.VideoId)
				.OrderBy(w => pagedVideoIds.IndexOf(w.VideoId))
				.Select(w => w.Video)
				.Select(GetVideoGetViewModel)
				.ToList();

			int totalCount = GetByCondition(w => w.UserId == userId)
				.Select(w => w.VideoId)
				.Distinct()
				.Count();

			return new PaginatedList<VideoGetViewModel>(items, totalCount, page, pageSize);
		}
	}
}
