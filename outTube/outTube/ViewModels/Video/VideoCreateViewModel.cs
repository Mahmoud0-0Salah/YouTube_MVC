namespace ourTube.ViewModels.Video
{
    public class VideoCreateViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public IFormFile Source { get; set; }
        public IFormFile Thumbnail { get; set; }
        public bool Visible { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
