namespace OurRecipes.Data.Models
{
    public class Reply
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public AppIdentityUser User { get; set; }
        public string CommentId { get; set; }
        public virtual Comment Comment { get; set; }
    }
}