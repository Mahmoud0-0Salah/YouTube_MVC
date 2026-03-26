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
                UserId = user.Id,
                VideoId = model.VideoId,
                Content = comment.Content,
                UpdatedAt = comment.UpdatedAt ?? DateTime.Now,
                UserName = user.FirstName + " " + user.LastName,
                UserAvatar = user.ImageUrl
            };
            return commentGetViewModel;
        }

        public async Task Delete(string commentId)
        {
            Comment comment = commentRepo.GetByCondition(c => c.CommentId == commentId).FirstOrDefault();
            UserCreateComment userCreateComment = userCreateCommentRepo.GetByCondition(uc => uc.CommentId == commentId).FirstOrDefault();
            commentRepo.Delete(comment);
            userCreateCommentRepo.Delete(userCreateComment);
            commentRepo.Save();
        }

        public async Task<CommentGetViewModel> Update(string commentId, string content)
        {
            var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);
            Comment comment = commentRepo.GetByCondition(c => c.CommentId == commentId).FirstOrDefault();
            if (comment != null)
            {
                comment.Content = content;
                comment.UpdatedAt = DateTime.Now;
                commentRepo.Update(comment);
                commentRepo.Save();
                return new CommentGetViewModel
                {
                    CommentId = comment.CommentId,
                    UserId = user.Id,
                    Content = comment.Content,
                    UpdatedAt = comment.UpdatedAt ?? DateTime.Now,
                    UserName = user.FirstName + " " + user.LastName,
                    UserAvatar = user.ImageUrl
                };
            }
            return null;
        }
    }
}
