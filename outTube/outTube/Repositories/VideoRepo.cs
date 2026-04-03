using Microsoft.EntityFrameworkCore;
using ourTube.Repositories.Interfaces;
using ourTube.ViewModels;
using ourTube.ViewModels.Comment;
using ourTube.ViewModels.Video;
using OurTube.Repositories;
using outTube.Data;
using outTube.Models;
using outTube.Models.Enums;
using outTube.Models.JunctionTables;

namespace ourTube.Repositories
{
	public class VideoRepo : Repository<Video>, IVideoRepo
	{
		private readonly IWatchVideoRepo watchVideoRepo;

		private readonly ApplicationDbContext _context;
		public VideoRepo(ApplicationDbContext context, IWatchVideoRepo watchVideoRepo) : base(context)
		{
			this.watchVideoRepo = watchVideoRepo;
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

		public PaginatedList<VideoGetViewModel> GetYourVideosInfo(string userId, int page = 1, int pageSize = 8)
		{
			List<VideoGetViewModel> items = GetByCondition(v => v.UserId == userId)
			   .Include(v => v.User)
			   .Include(v => v.Views)
			   .OrderByDescending(v => v.CreatedAt)
			   .Select(v => GetVideoGetViewModel(v))
			   .Skip((page - 1) * pageSize)
			   .Take(pageSize)
			   .ToList();

			return new PaginatedList<VideoGetViewModel>(items, GetByCondition(v => v.UserId == userId).Count(), page, pageSize);
		}

		public VideoDetailsViewModel GetVideoDetails(string videoId, string userId)
        {
            Video video = GetByCondition(v => v.VideoId == videoId)
				.Include(v => v.User)
					.ThenInclude(u => u.Subscribers)
				.Include(v => v.Views)
				.Include(v => v.Likes)
				.Include(v => v.Comments)
					.ThenInclude(c => c.Comment)
                .Include(v => v.Comments)
					.ThenInclude(c => c.User)
                .FirstOrDefault();

            WatchVideo watchVideo = new WatchVideo()
            {
                VideoId = video.VideoId,
                UserId = userId,
                WatchedAt = DateTime.Now
            };
            watchVideoRepo.Create(watchVideo);
            watchVideoRepo.Save();

            return new VideoDetailsViewModel
			{
				Id = video.VideoId,
				Title = video.Title,
				Channel = video.User.FirstName + " " + video.User.LastName,
				UserId = video.UserId,
                Description = video.Description ?? string.Empty,
				Duration = video.Duration.ToString(@"hh\:mm\:ss"),
				Time = video.CreatedAt.ToString("MMM dd, yyyy"),
				Thumb = video.ThumbnailUrl,
				Avatar = video.User.ImageUrl,
				VideoUrl = video.VideoUrl,
				Views = video.Views.Count,
				Likes = video.Likes.Count,
				IsLiked = video.Likes.Where(l=>l.UserId==userId && l.VideoId == videoId).Count() > 0? true : false, 
				NumOfComments = video.Comments.Count,
				NumOfSubscribers = video.User.Subscribers != null ? video.User.Subscribers.Count : 0,
				IsSubscribed = video.User.Subscribers != null && userId != null && video.User.Subscribers.Any(s => s.SubscriberId == userId),
				Comments = video.Comments.Select(c => new CommentGetViewModel
				{
					CommentId = c.CommentId,
					VideoId = c.VideoId,
                    UserName = c.User.FirstName + " " + c.User.LastName,
					Content = c.Comment.Content,
					UserAvatar = c.User.ImageUrl,
					UpdatedAt = c.Comment.UpdatedAt ?? DateTime.Now
                }).ToList()
			 };
        }

		public bool CreateReport(ReportCreateViewModel model, string userId)
		{
			if (_context.UserCreateReports.Any(ur => ur.VideoId == model.VideoId && ur.UserId == userId))
			{
				return false;
			}

			Report report = new Report()
			{
				Reason = model.Reason,
				Description = model.Description,
				Status = ReportStatus.Pending,
			};
			_context.Reports.Add(report);

			UserCreateReport userCreateReport = new UserCreateReport()
			{
				VideoId = model.VideoId,
				ReportId = report.ReportId,
				UserId = userId,
				CreatedAt = DateTime.Now
			};
			_context.UserCreateReports.Add(userCreateReport);

			return true;
		}

		public PaginatedList<VideoGetViewModel> SearchVideos(string query, int page = 1, int pageSize = 8)
		{
			if (string.IsNullOrWhiteSpace(query))
				return new PaginatedList<VideoGetViewModel>(new List<VideoGetViewModel>(), 0, page, pageSize);

			query = query.ToLower();

			var queryable = GetByCondition(v => v.Visible && (v.Title.ToLower().Contains(query) || (v.Description != null && v.Description.ToLower().Contains(query))));

			List<VideoGetViewModel> items = queryable
			   .Include(v => v.User)
			   .Include(v => v.Views)
			   .Select(v => new VideoGetViewModel
			   {
				   Id = v.VideoId,
				   Title = v.Title,
				   Channel = v.User.FirstName + " " + v.User.LastName,
				   Description = v.Description ?? string.Empty,
				   Duration = v.Duration.ToString(@"hh\:mm\:ss"),
				   Time = v.CreatedAt.ToString("MMM dd, yyyy"),
				   Thumb = v.ThumbnailUrl,
				   Avatar = v.User.ImageUrl,
				   VideoUrl = v.VideoUrl,
				   Views = v.Views.Count
			   })
			   .Skip((page - 1) * pageSize)
			   .Take(pageSize)
			   .ToList();

			return new PaginatedList<VideoGetViewModel>(items, queryable.Count(), page, pageSize);
		}

		public List<ChannelSearchViewModel> SearchChannels(string query, int take = 5)
		{
			if (string.IsNullOrWhiteSpace(query))
				return new List<ChannelSearchViewModel>();

			query = query.ToLower();

			return _context.Users
				.Where(u => u.FirstName.ToLower().Contains(query) || u.LastName.ToLower().Contains(query))
				.Include(u => u.Subscribers)
				.Include(u => u.Videos)
				.Select(u => new ChannelSearchViewModel
				{
					UserId = u.Id,
					ChannelName = u.FirstName + " " + u.LastName,
					ImageUrl = u.ImageUrl ?? string.Empty,
					SubscriberCount = u.Subscribers != null ? u.Subscribers.Count : 0,
					VideoCount = u.Videos != null ? u.Videos.Count : 0
				})
				.Take(take)
				.ToList();
		}
	}
}