using System.ComponentModel.DataAnnotations;

namespace OurRecipes.Data.Models
{
    public class Comment
    {
        public Comment()
        {
            this.Replies = new HashSet<Reply>();
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        [Required, MaxLength(250)]
        public string Content { get; set; }
        public string? UserId { get; set; }
        public virtual AppIdentityUser User { get; set; }
        public string RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }
        public virtual ICollection<Reply> Replies { get; set; }
    }
}
