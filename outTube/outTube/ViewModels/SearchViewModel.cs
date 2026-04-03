using ourTube.ViewModels.Video;

namespace ourTube.ViewModels
{
    public class SearchViewModel
    {
        public string Query { get; set; } = string.Empty;
        public PaginatedList<VideoGetViewModel> Videos { get; set; }
        public List<ChannelSearchViewModel> Channels { get; set; }
    }
}
