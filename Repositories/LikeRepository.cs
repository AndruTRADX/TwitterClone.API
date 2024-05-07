using TwitterClone.Data;
using TwitterClone.Models.Domains;

namespace TwitterClone.Repositories
{
    public class LikeRepository(TwitterCloneDbContext context) : ILikeRepository
    {
        private readonly TwitterCloneDbContext _context = context;

        public async Task<Like?> LikeToggle(Guid tweetId, string userId)
        {
            var tweet = await _context.Tweets.FindAsync(tweetId);

            if (tweet == null || userId == null)
            {
                return null;
            }

            var existingLike = _context.Likes
                .FirstOrDefault(l => l.TweetId == tweetId && l.UserId == userId);

            if (existingLike == null)
            {
                var newLike = new Like
                {
                    TweetId = tweetId,
                    UserId = userId
                };

                await _context.Likes.AddAsync(newLike);

                tweet.Likes.Add(newLike);
                await _context.SaveChangesAsync();

                return newLike;
            }
            else
            {
                _context.Likes.Remove(existingLike);

                tweet.Likes.Remove(existingLike);
                await _context.SaveChangesAsync();

                return null;
            }
        }
    }
}
