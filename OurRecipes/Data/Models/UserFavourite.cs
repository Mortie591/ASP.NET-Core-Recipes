using System.ComponentModel.DataAnnotations.Schema;

namespace OurRecipes.Data.Models
{
    public class UserFavourite
    {
        public int Id { get; set; }

        [ForeignKey(nameof(AppIdentityUser))]
        public string? UserId { get; set; }
        public virtual AppIdentityUser User { get; set; }

        [ForeignKey(nameof(Recipe))]
        public string? RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
