using ourTube.ViewModels.Comment;

namespace ourTube.ViewModels.Video
{
    public class VideoDetailsViewModel : VideoGetViewModel
    {
        public int Likes { get; set; } = 0;
        public bool IsLiked { get; set; } = true;
        public int NumOfComments { get; set; } = 0;
        public List<CommentGetViewModel> Comments { get; set; } = new List<CommentGetViewModel>();
    }
}
