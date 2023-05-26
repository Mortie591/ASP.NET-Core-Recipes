using Microsoft.AspNetCore.Identity;

namespace OurRecipes.Data.Models
{
    public class AppIdentityUser : IdentityUser
    {
        public AppIdentityUser()
        {
            this.MyRecipes = new HashSet<Recipe>();
            this.UserFavourites = new HashSet<UserFavourite>();
            this.UserComments = new HashSet<Comment>();
            this.UserReplies = new HashSet<Reply>();
        }
        //recipes created by the user
        public virtual ICollection<Recipe> MyRecipes { get; set; }
        //recipes liked by the user
        public virtual ICollection<UserFavourite> UserFavourites { get; set; } 
        public virtual ICollection<Comment> UserComments { get; set; }
        public virtual ICollection<Reply> UserReplies { get; set; }

    }
}
