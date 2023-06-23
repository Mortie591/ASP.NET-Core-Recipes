using Microsoft.AspNetCore.Identity;
using OurRecipes.Data;
using OurRecipes.Data.Models;

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
        public void AddComment(string recipeId, string userId)
        {
            
        }

        public void AddReply(string commentId, string userId)
        {
            throw new NotImplementedException();
        }

        public void RemoveComment(string id, string recipeId, string userId)
        {
            throw new NotImplementedException();
        }

        public void RemoveReply(string replyId, string commentId, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
