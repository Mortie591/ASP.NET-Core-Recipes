using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OurRecipes.Data;
using OurRecipes.Data.Models;
using OurRecipes.Models.Comments;
using System.ComponentModel.Design;

namespace OurRecipes.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<AppIdentityUser> userManager;

        public CommentService(ApplicationDbContext context, UserManager<AppIdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public void AddComment(CommentInputModel comment)
        {
            var newComment = new Comment
            {
                UserId = comment.UserId,
                RecipeId = comment.RecipeId,
                Content = comment.Content
            };
            this.context.Comments.Add(newComment);
            this.context.SaveChanges();
        }

        public void AddReply(ReplyInputModel reply)
        {
            var newReply = new Reply
            {
                UserId = reply.UserId,
                CommentId = reply.CommentId,
                Content = reply.Content
            };
            this.context.Replies.Add(newReply);
            this.context.SaveChanges();
        }

        public CommentViewModel GetComment(string id)
        {
            var comment = this.context.Comments.FirstOrDefault(x => x.Id == id);
            if(comment != null)
            {
                return new CommentViewModel
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    UserId = comment.UserId,
                    RecipeId= comment.RecipeId
                };
            }
            else
            {
                throw new NullReferenceException(nameof(id));
            }
        }

        public ICollection<CommentViewModel> GetComments(string recipeId)
        {
            var comments = this.context.Comments
                .Include(x=>x.User)
                .Where(x => x.RecipeId == recipeId).ToList();
            var viewComments = new List<CommentViewModel>();
            foreach (var comment in comments)
            {
                viewComments.Add(new CommentViewModel
                {
                    UserName = comment.User.UserName,
                    Content = comment.Content
                });
            }
            return viewComments;
        }

        public ICollection<ReplyViewModel> GetReplies(string commentId)
        {
            var replies = this.context.Replies
                .Include(x => x.User)
                .Where(x => x.CommentId == commentId).ToList();
            var viewReplies = new List<ReplyViewModel>();
            foreach (var reply in replies)
            {
                viewReplies.Add(new ReplyViewModel
                {
                    UserName = reply.User.UserName,
                    Content = reply.Content
                });
            }
            return viewReplies;
        }

        public ReplyViewModel GetReply(string id)
        {
            var reply = this.context.Replies.FirstOrDefault(x => x.Id == id);
            if (reply != null)
            {
                return new ReplyViewModel
                {
                    Id = reply.Id,
                    Content = reply.Content,
                    UserId = reply.UserId
                };
            }
            else
            {
                throw new NullReferenceException(nameof(id));
            }
        }

        public void RemoveComment(string commentId)
        {
            var commentToDelete = this.context.Comments.FirstOrDefault(x => x.Id == commentId);
            if (commentToDelete != null)
            {
                this.context.Comments.Remove(commentToDelete);
                this.context.SaveChanges();
            }
            else
            {
                throw new NullReferenceException(nameof(commentId));
            }
        }

        public void RemoveReply(string replyId)
        {
            var replyToDelete = this.context.Replies.FirstOrDefault(x => x.Id == replyId);
            if (replyToDelete != null)
            {
                this.context.Replies.Remove(replyToDelete);
                this.context.SaveChanges();
            }
            else
            {
                throw new NullReferenceException(nameof(replyId));
            }
        }
    }
}
