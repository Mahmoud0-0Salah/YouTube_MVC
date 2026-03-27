using ourTube.ViewModels;
using ourTube.ViewModels.Video;
using OurTube.Repositories.Interfaces.Common;
using outTube.Models.JunctionTables;

namespace ourTube.Repositories.Interfaces
{
	public interface ILikeRepo:IRepository<LikeVideo>
	{
		PaginatedList<VideoGetViewModel> GetLikedVideosInfo(string useerId, int page = 1, int pageSize = 8);
	}
}
