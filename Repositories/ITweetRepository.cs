using TwitterClone.Models.Domains;
using TwitterClone.Models.DTOs;

namespace TwitterClone.Repositories
{
    public interface ITweetRepository
    {
        public Task<List<Tweet>> GetAllTweetsAsync(int size, int offset);
        public Task<Tweet?> GetTweetAsync(Guid id);
        public Task<Tweet> CreateTweetAsync(SubmitTweetDTO content, string userName, string userId);
        public Task<Tweet?> UpdateTweetAsync(SubmitTweetDTO tweetDTO, Guid tweetId, string userId);
        public Task<Tweet?> DeleteTweetAsync(Guid id, string userId);
    }
}
