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
		private readonly ApplicationDbContext _context;
		public VideoRepo(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public PaginatedList<VideoGetViewModel> GetVideosInfo(int page = 1, int pageSize = 8)
		{
			List<VideoGetViewModel> items = _context.Videos
			   .Include(v => v.User)
			   .Include(v => v.Views)
			   .Select(v => new VideoGetViewModel
			   {
				   Id = v.VideoId,	
				   Views = v.Views.Count,
				   Title = v.Title,
				   Description = v.Description ?? string.Empty,
				   Channel = v.User.FirstName + " " + v.User.LastName,
				   Duration = v.Duration.ToString(@"hh\:mm\:ss"),
				   Time = v.CreatedAt.ToString("MMM dd, yyyy"),
				   Thumb = v.ThumbnailUrl,
				   Avatar = v.User.ImageUrl,
				   VideoUrl = v.VideoUrl
			   })
			   .Skip((page - 1) * pageSize)
			   .Take(pageSize)
			   .ToList();

			return new PaginatedList<VideoGetViewModel>(items, _context.Videos.Count(), page, pageSize);
		}
	}
}