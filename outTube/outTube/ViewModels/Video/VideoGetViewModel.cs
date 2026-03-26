namespace ourTube.ViewModels.Video
{
	public class VideoGetViewModel
	{
        public string Id { get; set; }
        public string Title { get; set; } = string.Empty;
		public string Channel { get; set; } = string.Empty;
		public int Views { get; set; } = 0;
		public string Time { get; set; } = string.Empty;
		public string Thumb { get; set; } = string.Empty;
		public string Avatar { get; set; } = string.Empty;
		public string Duration { get; set; } = string.Empty;
		public string VideoUrl { get; set; } = string.Empty;
	}
}
