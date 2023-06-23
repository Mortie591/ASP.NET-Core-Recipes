namespace OurRecipes.Services
{
    public interface ICommentService
    {
        
        public void AddComment(string recipeId,string userId); 
        public void AddReply(string commentId, string userId);
        public void RemoveComment(string id,string recipeId, string userId);
        public void RemoveReply(string replyId,string commentId, string userId);
    }
}
