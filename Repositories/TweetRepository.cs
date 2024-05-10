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
            var tweetsWithCommentsCount = await context.Tweets
                .OrderByDescending(t => t.CreatedAt)
                .Skip((offset - 1) * size)
                .Take(size)
                .Select(t => new TweetDTOListItem
                {
                    Id = t.Id,
                    UserName = t.UserName,
                    FirstName = t.FirstName,
                    Content = t.Content,
                    Likes = t.Likes.Count(),
                    CreatedAt = t.CreatedAt,
                    CommentsCount = t.Comments.Count()
                })
                .ToListAsync();

            return tweetsWithCommentsCount;
        }

        public async Task<Tweet?> GetTweetAsync(Guid id)
        {
            var tweet = await context.Tweets.FirstOrDefaultAsync(t => t.Id == id);

            return tweet;
        }

        public async Task<Tweet?> CreateTweetAsync(SubmitTweetDTO content, string userName, string firstName, string userId)
        {
            Tweet tweet = new()
            {
                Content = content.Content,
                UserName = userName,
                FirstName = firstName,
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
