using Microsoft.EntityFrameworkCore;
using TwitterClone.Data;
using TwitterClone.Models.Domains;
using TwitterClone.Models.DTOs;

namespace TwitterClone.Repositories
{
    public class TweetRepository(TwitterCloneDbContext context, TwitterCloneAuthDbContext authDbContext) : ITweetRepository
    {
        private readonly TwitterCloneDbContext context = context;
        private readonly TwitterCloneAuthDbContext authDbContext = authDbContext;

        public async Task<List<TweetDTOListItem>> GetAllTweetsAsync(int size, int offset)
        {
            var tweets = await context.Tweets
                .OrderByDescending(t => t.CreatedAt)
                .Skip((offset - 1) * size)
                .Take(size)
                .Include("Likes")
                .Include("Comments")
                .ToListAsync();

            var tweetDTOs = new List<TweetDTOListItem>();

            foreach (var tweet in tweets)
            {
                var user = await authDbContext.Users.FindAsync(tweet.UserId);
                if (user != null)
                {
                    tweetDTOs.Add(new TweetDTOListItem
                    {
                        Id = tweet.Id,
                        UserName = user.UserName!,
                        FirstName = user.FirstName,
                        Content = tweet.Content,
                        Likes = tweet.Likes.Count(),
                        CreatedAt = tweet.CreatedAt,
                        CommentsCount = tweet.Comments.Count()
                    });
                }
            }

            return tweetDTOs;
        }

        public async Task<TweetDTO?> GetTweetAsync(Guid id)
        {
            var tweet = await context.Tweets.FirstOrDefaultAsync(t => t.Id == id);
            if (tweet == null)
            {
                return null;
            }

            var user = await authDbContext.Users.FindAsync(tweet.UserId);
            if (user == null)
            {
                return null;
            }

            return new TweetDTO
            {
                Id = tweet.Id,
                UserName = user.UserName!,
                FirstName = user.FirstName,
                Content = tweet.Content,
                CreatedAt = tweet.CreatedAt
            };
        }

        public async Task<Tweet?> CreateTweetAsync(SubmitTweetDTO content, string userId)
        {
            Tweet tweet = new()
            {
                Content = content.Content,
                UserId = userId
            };

            var user = await authDbContext.Users.FindAsync(userId);

            if (user == null)
            {
                return null;
            }

            await context.Tweets.AddAsync(tweet);
            user.TweetIds.Add(tweet.Id.ToString());

            await context.SaveChangesAsync();
            await authDbContext.SaveChangesAsync();

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
            var user = await authDbContext.Users.FindAsync(userId);

            if (tweet == null || tweet.UserId != userId || user == null)
            {
                return null;
            }

            context.Tweets.Remove(tweet);
            user.TweetIds.Remove(tweet.Id.ToString());

            await context.SaveChangesAsync();
            await authDbContext.SaveChangesAsync();

            return tweet;
        }
    }
}
