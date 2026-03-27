using Microsoft.EntityFrameworkCore;
using System.Linq;
using outTube.Data;
using outTube.Models;
using outTube.Models.JunctionTables;
using System.Security.Claims;
using ourTube.Repositories.Interfaces;
using ourTube.ViewModels;
using ourTube.ViewModels.Video;
using System.Linq.Expressions;
using OurTube.Repositories;
using OurTube.Repositories.Interfaces.Common;

namespace ourTube.Repositories
{
    public class UserSubscriberRepo : Repository<UserSubscriber>, IUserSubscriberRepo
    {
        private readonly ApplicationDbContext _context;

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

        public PaginatedList<VideoGetViewModel> GetLikedVideosInfo(string userId, int page = 1, int pageSize = 8)
        {
            List<string> usersIds = GetByCondition(s => s.SubscriberId == userId)
                .Select(s => s.UserId)
                .ToList();

            List<VideoGetViewModel> items = _context.Videos
                .Where(v => v.Visible && usersIds.Contains(v.UserId))
                .Include(v => v.User)
                .Include(v => v.Views)
                .Select(v => GetVideoGetViewModel(v))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedList<VideoGetViewModel>(
                items, 
                _context.Videos.Where(v => v.Visible && usersIds.Contains(v.UserId)).Count(), 
                page, 
                pageSize);
        }
    }
}
