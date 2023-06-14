using System.ComponentModel.DataAnnotations;

namespace OurRecipes.Models.Comments
{
    public class CommentInputModel
    {
        public CommentInputModel()
        {
        }
        [Required, MinLength(10), MaxLength(250)]
        public string Content { get; set; }
        public string UserId { get; set; }
        public string RecipeId { get; set; }
    }
}
