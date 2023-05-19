namespace OurRecipes.Data.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public string RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }
        public ICollection<Reply> Replies { get; set; }
    }
}
