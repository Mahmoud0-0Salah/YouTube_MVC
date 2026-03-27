using ourTube.Repositories.Interfaces;
using OurTube.Repositories;
using OurTube.Repositories.Interfaces.Common;
using outTube.Data;
using outTube.Models.JunctionTables;
using Microsoft.EntityFrameworkCore;
using ourTube.Repositories.Interfaces;
using ourTube.ViewModels;
using ourTube.ViewModels.Video;
using OurTube.Repositories;
using OurTube.Repositories.Interfaces.Common;
using outTube.Data;
using outTube.Models;
using System.Linq.Expressions;

namespace ourTube.Repositories
{
    public class UserSubscriberRepo : Repository<UserSubscriber>, IUserSubscriberRepo
    {
		ApplicationDbContext _context;
		public UserSubscriberRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
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
			List<string> usersIds = GetByCondition(s => s.SubscriberId == useerId)
				.Select(s => s.UserId)
				.ToList();

			List<VideoGetViewModel> items = _context.Videos
		   .Where(v => v.Visible && usersIds.Contains(v.UserId))
		   .Include(v => v.User)
		   .Include(v=>v.Views)
		   .Select(v => GetVideoGetViewModel(v))
		   .Skip((page - 1) * pageSize)
		   .Take(pageSize)
		   .ToList();

			return new PaginatedList<VideoGetViewModel>(items, _context.Videos
		   .Where(v => v.Visible && usersIds.Contains(v.UserId))
		   .Count(), page, pageSize);
		}
	}
}
