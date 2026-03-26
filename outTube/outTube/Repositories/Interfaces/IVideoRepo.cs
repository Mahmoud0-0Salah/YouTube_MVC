using ourTube.ViewModels;
using ourTube.ViewModels.Video;
using OurTube.Repositories.Interfaces.Common;
using outTube.Models;

namespace ourTube.Repositories.Interfaces
{
	public interface IVideoRepo: IRepository<Video>
	{
		PaginatedList<VideoGetViewModel> GetVideosInfo(int page = 1, int pageSize = 8);
		PaginatedList<VideoGetViewModel> GetTrendingVideosInfo(int page = 1, int pageSize = 8);
		PaginatedList<VideoGetViewModel> GetLastestVideosInfo(int page = 1, int pageSize = 8);
	}
}
