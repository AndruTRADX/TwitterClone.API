using Microsoft.AspNetCore.Identity;
using TwitterClone.Data;
using TwitterClone.Models.Domains;
using TwitterClone.Models.DTOs;

namespace TwitterClone.Repositories
{
    public class TweetRepository(TwitterCloneDbContext context) : ITweetRepository
    {
        private readonly TwitterCloneDbContext context = context;

        public async Task<Tweet> CreateTweetAsync(SubmitTweetDTO content, string userName, string userId)
        {
            Tweet tweet = new()
            {
                Content = content.Content,
                UserName = userName,
                UserId = userId
            };

            await context.Tweets.AddAsync(tweet);
            await context.SaveChangesAsync();

            return tweet;
        }
    }
}
