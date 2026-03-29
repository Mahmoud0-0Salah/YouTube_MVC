using ourTube.ViewModels.Video;

namespace ourTube.ViewModels
{
    public class ChannelPageViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string ChannelName { get; set; } = string.Empty;
        public string Handle { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int SubscriberCount { get; set; } = 0;
        public int VideoCount { get; set; } = 0;
        public DateTime JoinedDate { get; set; }
        public bool IsSubscribed { get; set; } = false;
        public bool IsOwnChannel { get; set; } = false;
        public PaginatedList<VideoGetViewModel> Videos { get; set; }
    }
}
