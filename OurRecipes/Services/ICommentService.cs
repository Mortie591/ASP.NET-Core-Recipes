namespace OurRecipes.Services
{
    public interface ICommentService
    {
        
        public void AddComment(); 
        public void AddReply(string commentId);
        public void RemoveComment(string id);
        public void RemoveReply(string id);
    }
}
