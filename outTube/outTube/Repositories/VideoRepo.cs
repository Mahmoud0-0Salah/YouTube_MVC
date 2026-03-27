using Microsoft.EntityFrameworkCore;
using ourTube.Repositories.Interfaces;
using ourTube.ViewModels;
using ourTube.ViewModels.Comment;
using ourTube.ViewModels.Video;
using OurTube.Repositories;
using outTube.Data;
using outTube.Models;
using outTube.Models.JunctionTables;

namespace ourTube.Repositories
{
	public class VideoRepo : Repository<Video>, IVideoRepo
	{
        private readonly IWatchVideoRepo watchVideoRepo;

        //private readonly ApplicationDbContext _context;
        public VideoRepo(ApplicationDbContext context, IWatchVideoRepo watchVideoRepo) : base(context)
		{
            this.watchVideoRepo = watchVideoRepo;
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

        public VideoDetailsViewModel GetVideoDetails(string videoId, string userId)
        {
            Video video = GetByCondition(v => v.VideoId == videoId)
				.Include(v => v.User)
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
				Description = video.Description ?? string.Empty,
				Duration = video.Duration.ToString(@"hh\:mm\:ss"),
				Time = video.CreatedAt.ToString("MMM dd, yyyy"),
				Thumb = video.ThumbnailUrl,
				Avatar = video.User.ImageUrl,
				VideoUrl = video.VideoUrl,
				Views = video.Views.Count,
				Likes = video.Likes.Count,
				IsLiked = video.Likes.Where(l=>l.UserId==userId).Count() > 0? true : false, 
				NumOfComments = video.Comments.Count,
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
    }
}