using OurRecipes.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace OurRecipes.Models.Comments
{
    public class CommentViewModel
    {
        public CommentViewModel()
        {
            this.Replies = new List<ReplyViewModel>();
        }
        public string Id { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string RecipeId { get; set; }
        public ICollection<ReplyViewModel> Replies { get; set; }
    }
}
