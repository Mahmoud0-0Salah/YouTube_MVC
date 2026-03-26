using System.ComponentModel.DataAnnotations;

namespace ourTube.ViewModels.Comment
{
    public class CommentGetViewModel
    {
        public string CommentId { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public string Content { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
