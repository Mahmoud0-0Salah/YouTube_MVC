using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ourTube.Repositories;
using ourTube.Repositories.Interfaces;
using ourTube.ViewModels.Comment;
using outTube.Models;
using outTube.Models.JunctionTables;
using System.Security.Claims;

namespace ourTube.Services
{
    public class CommentService
    {
        private readonly ICommentRepo commentRepo;
        private readonly IUserCreateCommentRepo userCreateCommentRepo;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;

        public CommentService(ICommentRepo commentRepo, 
                              IUserCreateCommentRepo userCreateCommentRepo,
                              IHttpContextAccessor httpContextAccessor,
                              UserManager<User> userManager)
        {
            this.commentRepo = commentRepo;
            this.userCreateCommentRepo = userCreateCommentRepo;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task<CommentGetViewModel> Create(CommentCreateViewModel model)
        {
            var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);
            Comment comment = new Comment()
            {
                Content = model.Content,
                UpdatedAt = DateTime.Now,
            };
            commentRepo.Create(comment);
            UserCreateComment userCreateComment = new UserCreateComment()
            {
                CommentId = comment.CommentId,
                VideoId = model.VideoId,
                UserId = user.Id,
                CreatedAt = DateTime.Now
            };
            userCreateCommentRepo.Create(userCreateComment);
            commentRepo.Save();
            CommentGetViewModel commentGetViewModel = new CommentGetViewModel()
            {
                CommentId = comment.CommentId,
                Content = comment.Content,
                UpdatedAt = comment.UpdatedAt ?? DateTime.Now,
                UserName = user.FirstName + " " + user.LastName,
                UserAvatar = user.ImageUrl
            };
            return commentGetViewModel;
        }
    }
}
