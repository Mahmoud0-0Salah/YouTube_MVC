namespace ourTube.ViewModels
{
    public class ChannelSearchViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string ChannelName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int SubscriberCount { get; set; } = 0;
        public int VideoCount { get; set; } = 0;
    }
}
