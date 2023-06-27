using System.ComponentModel.DataAnnotations;

namespace OurRecipes.Data.Models
{
    public class Reply
    {
        public Reply()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        [Required, MaxLength(250)]
        public string Content { get; set; }
        public string? UserId { get; set; }
        public virtual AppIdentityUser User { get; set; }
        public string CommentId { get; set; }
        public virtual Comment Comment { get; set; }
    }
}