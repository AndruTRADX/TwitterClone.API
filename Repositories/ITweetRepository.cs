using TwitterClone.Models.Domains;
using TwitterClone.Models.DTOs;

namespace TwitterClone.Repositories
{
    public interface ITweetRepository
    {
        public Task<Tweet> CreateTweetAsync(SubmitTweetDTO content, string userName, string userId);
        public Task<Tweet> UpdateTweetAsync(SubmitTweetDTO tweetDTO, string userId);
    }
}
