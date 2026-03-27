using OurTube.Repositories.Interfaces.Common;
using outTube.Models.JunctionTables;
using ourTube.ViewModels;
using ourTube.ViewModels.Video;

namespace ourTube.Repositories.Interfaces
{
    public interface IUserSubscriberRepo:IRepository<UserSubscriber>
    {
		public PaginatedList<VideoGetViewModel> GetLikedVideosInfo(string userId, int page = 1, int pageSize = 8);
	}
}
