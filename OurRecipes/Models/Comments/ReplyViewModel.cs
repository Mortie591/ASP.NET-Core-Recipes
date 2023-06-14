using OurRecipes.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace OurRecipes.Models.Comments
{
    public class ReplyViewModel
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public string CommentId { get; set; }
    }
}
