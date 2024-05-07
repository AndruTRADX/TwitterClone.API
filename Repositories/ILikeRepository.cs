using TwitterClone.Models.Domains;

namespace TwitterClone.Repositories
{
    public interface ILikeRepository
    {
        Task<Like?> LikeToggle(Guid tweetId, string userId);
    }
}
