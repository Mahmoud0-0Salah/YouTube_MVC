using ourTube.ViewModels;
using ourTube.ViewModels.Video;
using OurTube.Repositories.Interfaces.Common;
using outTube.Models.JunctionTables;

namespace ourTube.Repositories.Interfaces
{
    public interface IWatchVideoRepo : IRepository<WatchVideo>
    {
		public PaginatedList<VideoGetViewModel> GetWatchedVideosInfo(string userId, int page = 1, int pageSize = 8);

	}
}
