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

        public async Task Create(CommentCreateViewModel model)
        {
            CommentGetViewModel comment = await commentService.Create(model);
            await Clients.All.SendAsync("ReceiveComment", comment);
        }
        public async Task Delete(string commentId)
        {
            await commentService.Delete(commentId);
            await Clients.All.SendAsync("ReceiveDeletedComment", commentId);
        }

        public async Task Update(string commentId, string content)
        {
            CommentGetViewModel comment = await commentService.Update(commentId, content);
            if (comment != null)
            {
                await Clients.All.SendAsync("ReceiveUpdatedComment", comment);
            }
        }
    }
}
