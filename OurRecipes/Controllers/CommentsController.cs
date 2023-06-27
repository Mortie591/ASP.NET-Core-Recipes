using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OurRecipes.Data.Models;
using OurRecipes.Models.Comments;
using OurRecipes.Services;

namespace OurRecipes.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly IRecipeService recipeService;
        private readonly UserManager<AppIdentityUser> userManager;
        private readonly ICommentService commentService;

        public CommentsController(IRecipeService recipeService, UserManager<AppIdentityUser> userManager, ICommentService commentService) 
        {
            this.recipeService = recipeService;
            this.userManager = userManager;
            this.commentService = commentService;
        }

        public IActionResult _ViewComments(string recipeId)
        {
            var viewComments = this.commentService.GetComments(recipeId);
            return View(viewComments);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult _AddComment(CommentInputModel input)
        {
            string authorId = userManager.GetUserId(User);
            
            if (!ModelState.IsValid)
            {
                return this.View(input);
            }
            this.commentService.AddComment(input);
            return RedirectToAction("Details", "Recipes", new {id=input.RecipeId},"#recipe-comments");
        }

        [Authorize]
        public IActionResult RemoveComment(string id)
        {
            var userId = userManager.GetUserId(User);
            var comment = this.commentService.GetComment(id);
            var recipeId = comment.RecipeId;
            if(comment.UserId != userId)
            {
                return Forbid();
            }else
            {
                this.commentService.RemoveComment(id);
                return RedirectToAction("Details", "Recipes", new { id = recipeId }, "#recipe-comments");
            }
        }

        public IActionResult ViewReplies(string commentId)
        {
            var viewReplies = this.commentService.GetReplies(commentId);
            return View(viewReplies);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult AddReply(ReplyInputModel input)
        {
            string authorId = userManager.GetUserId(User);
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ViewReplies", "Comments");
            }
            this.commentService.AddReply(input);
            return RedirectToAction("ViewReplies", "Comments");
        }
        [Authorize]
        public IActionResult RemoveReply(string id)
        {
            var userId = userManager.GetUserId(User);
            var reply = this.commentService.GetReply(id);
            if (reply.UserId != userId)
            {
                return Forbid();
            }
            else
            {
                this.commentService.RemoveReply(id);
                return RedirectToAction("ViewReplies", "Comments");
            }
        }
    }
}
