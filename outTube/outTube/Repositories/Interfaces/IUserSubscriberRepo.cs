using OurTube.Repositories.Interfaces.Common;
using outTube.Models.JunctionTables;

namespace ourTube.Repositories.Interfaces
{
    public interface IUserSubscriberRepo:IRepository<UserSubscriber>
    {
		public PaginatedList<VideoGetViewModel> GetLikedVideosInfo(string useerId, int page = 1, int pageSize = 8);


	}
}
