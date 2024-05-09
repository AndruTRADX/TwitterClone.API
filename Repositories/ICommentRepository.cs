using TwitterClone.Models.Domains;
using TwitterClone.Models.DTOs;

namespace TwitterClone.Repositories
{
    public interface ICommentRepository
    {
        Task<List<CommentDTOListItem>> GetCommentsForTweetAsync(Guid tweetId, int pageNumber, int pageSize);
        Task<Comment?> PostCommentToTweetAsync(SubmitCommentDTO submitCommentDTO, string userName, string firstName, string userId, Guid tweetId);
        Task<Comment?> DeleteCommentFromTweetAsync(Guid commentId, string userId);
    }
}
