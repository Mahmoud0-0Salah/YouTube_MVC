using Microsoft.EntityFrameworkCore;
using ourTube.Repositories.Interfaces;
using ourTube.ViewModels;
using ourTube.ViewModels.Video;
using OurTube.Repositories;
using outTube.Models;
using outTube.Data;

namespace ourTube.Repositories
{
	public class VideoRepo : Repository<Video>, IVideoRepo
	{
		//private readonly ApplicationDbContext _context;
		public VideoRepo(ApplicationDbContext context) : base(context)
		{
			//_context = context;
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
		public PaginatedList<VideoGetViewModel> GetVideosInfo(int page = 1, int pageSize = 8)
		{
			List<VideoGetViewModel> items = GetByCondition(v=>v.Visible)
			   .Include(v => v.User)
			   .Include(v => v.Views)
			   .Select(v => GetVideoGetViewModel(v))
			   .Skip((page - 1) * pageSize)
			   .Take(pageSize)
			   .ToList();

			return new PaginatedList<VideoGetViewModel>(items, GetByCondition(v => v.Visible).Count(), page, pageSize);
		}

		public PaginatedList<VideoGetViewModel> GetTrendingVideosInfo(int page = 1, int pageSize = 8)
		{
			List<VideoGetViewModel> items = GetByCondition(v => v.Visible)
			   .Include(v => v.User)
			   .Include(v => v.Views)
			   .OrderByDescending(v => v.Views.Count)
			   .Select(v => GetVideoGetViewModel(v))
			   .Skip((page - 1) * pageSize)
			   .Take(pageSize)
			   .ToList();

			return new PaginatedList<VideoGetViewModel>(items, GetByCondition(v => v.Visible).Count(), page, pageSize);
		}

		public PaginatedList<VideoGetViewModel> GetLastestVideosInfo(int page = 1, int pageSize = 8)
		{
			List<VideoGetViewModel> items = GetByCondition(v => v.Visible)
			   .Include(v => v.User)
			   .Include(v => v.Views)
			   .OrderByDescending(v => v.CreatedAt)
			   .Select(v => GetVideoGetViewModel(v))
			   .Skip((page - 1) * pageSize)
			   .Take(pageSize)
			   .ToList();

			return new PaginatedList<VideoGetViewModel>(items, GetByCondition(v => v.Visible).Count(), page, pageSize);
		}
	}
}