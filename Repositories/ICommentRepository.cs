using TwitterClone.Models.Domains;
using TwitterClone.Models.DTOs;

namespace TwitterClone.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment?> PostCommentToTweetAsync(SubmitCommentDTO submitCommentDTO, string userName, string userId, Guid tweetId);
        Task<Comment?> DeleteCommentFromTweetAsync(Guid commentId, string userId);
    }
}
