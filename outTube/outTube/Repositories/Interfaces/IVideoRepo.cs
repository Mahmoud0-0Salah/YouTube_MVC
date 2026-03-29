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
		PaginatedList<VideoGetViewModel> GetYourVideosInfo(string userId, int page = 1, int pageSize = 8);
		VideoDetailsViewModel GetVideoDetails(string videoId, string userId);

		bool CreateReport(ReportCreateViewModel model, string userId);
		PaginatedList<VideoGetViewModel> SearchVideos(string query, int page = 1, int pageSize = 8);
		List<ChannelSearchViewModel> SearchChannels(string query, int take = 5);
    }
}
