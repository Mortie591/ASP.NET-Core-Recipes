using OurRecipes.Models.Comments;

namespace OurRecipes.Services
{
    public interface ICommentService
    {
        public ICollection<CommentViewModel> GetComments(string recipeId);
        public CommentViewModel GetComment(string id);
        public ICollection<ReplyViewModel> GetReplies(string commentId);
        public ReplyViewModel GetReply(string id);
        public void AddComment(CommentInputModel comment); 
        public void AddReply(ReplyInputModel reply);
        public void RemoveComment(string commentId);
        public void RemoveReply(string replyId);
    }
}
