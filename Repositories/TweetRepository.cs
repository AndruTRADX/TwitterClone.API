using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TwitterClone.Data;
using TwitterClone.Models.Domains;
using TwitterClone.Models.DTOs;

namespace TwitterClone.Repositories
{
    public class TweetRepository(TwitterCloneDbContext context) : ITweetRepository
    {
        private readonly TwitterCloneDbContext context = context;

        public async Task<List<Tweet>> GetAllTweetsAsync(int size, int offset)
        {
            var tweets = context.Tweets.AsQueryable();

            var skipResults = (offset - 1) * size;

            return await tweets.Skip(skipResults).Take(size).ToListAsync();
        }

        public async Task<Tweet?> GetTweetAsync(Guid id)
        {
            var tweet = await context.Tweets.FindAsync(id);

            return tweet;
        }

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

        public async Task<Tweet?> UpdateTweetAsync(SubmitTweetDTO tweetDTO, Guid tweetId, string userId)
        {
            var tweet = await context.Tweets.FindAsync(tweetId);

            if (tweet == null || tweet.UserId != userId)
            {
                return null;
            }

            tweet.Content = tweetDTO.Content;
            await context.SaveChangesAsync();

            return tweet;
        }

        public async Task<Tweet?> DeleteTweetAsync(Guid id, string userId)
        {
            var tweet = await context.Tweets.FindAsync(id);

            if (tweet == null || tweet.UserId != userId)
            {
                return null;
            }

            context.Tweets.Remove(tweet);
            await context.SaveChangesAsync();

            return tweet;

        }
    }
}
