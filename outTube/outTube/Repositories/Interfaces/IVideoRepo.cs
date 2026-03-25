using ourTube.ViewModels;
using ourTube.ViewModels.Video;
using OurTube.Repositories.Interfaces.Common;

namespace ourTube.Repositories.Interfaces
{
	public interface IVideoRepo: IRepository<Video>
	{
		PaginatedList<VideoGetViewModel> GetVideosInfo(int page = 1, int pageSize = 8);
	}
}
