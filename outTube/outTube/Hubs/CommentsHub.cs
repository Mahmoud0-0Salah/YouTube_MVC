using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using ourTube.Repositories;
using ourTube.Repositories.Interfaces;
using ourTube.Services;
using ourTube.ViewModels.Comment;
using outTube.Models;
using outTube.Models.JunctionTables;

namespace ourTube.Hubs
{
    public class CommentsHub : Hub
    {
        private readonly CommentService commentService;

        public CommentsHub(CommentService commentService)
        {
            this.commentService = commentService;
        }

        public async Task CreateComment(CommentCreateViewModel model)
        {
            CommentGetViewModel comment = await commentService.Create(model);
            await Clients.All.SendAsync("ReceiveComment", comment);
        }
    }
}
