using TwitterClone.Models.Domains;

namespace TwitterClone.Repositories
{
    public interface ILikeToCommentRepository
    {
        Task<LikeToComment?> LikeToCommentToggle(Guid commentId, string userId);
    }
}
