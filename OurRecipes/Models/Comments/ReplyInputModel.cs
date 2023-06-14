using System.ComponentModel.DataAnnotations;

namespace OurRecipes.Models.Comments
{
    public class ReplyInputModel
    {
        [Required, MinLength(4),MaxLength(250)]
        public string Content { get; set; }
        public string UserId { get; set; }
        public string CommentId { get; set; }
    }
}
