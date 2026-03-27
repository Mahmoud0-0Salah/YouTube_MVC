using ourTube.ViewModels.Comment;

namespace ourTube.ViewModels.Video
{
    public class VideoDetailsViewModel : VideoGetViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public int Likes { get; set; } = 0;
        public bool IsLiked { get; set; } = true;
        public int NumOfComments { get; set; } = 0;
        public int NumOfSubscribers { get; set; }
        public bool IsSubscribed { get; set; }
        public List<CommentGetViewModel> Comments { get; set; } = new List<CommentGetViewModel>();
    }
}
