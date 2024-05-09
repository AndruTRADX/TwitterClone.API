using Microsoft.EntityFrameworkCore;
using TwitterClone.Data;
using TwitterClone.Models.Domains;
using TwitterClone.Models.DTOs;

namespace TwitterClone.Repositories
{
    public class CommentRepository(TwitterCloneDbContext context) : ICommentRepository
    {
        private readonly TwitterCloneDbContext context = context;

        public async Task<List<CommentDTOListItem>> GetCommentsForTweetAsync(Guid tweetId, int pageNumber, int pageSize)
        {
            var tweetWithComments = await context.Tweets
                .Include(tweet => tweet.Comments)
                .ThenInclude(comment => comment.Likes)
                .FirstOrDefaultAsync(tweet => tweet.Id == tweetId);

            if (tweetWithComments == null)
            {
                return [];
            }

            var commentsForTweet = tweetWithComments.Comments
                .OrderByDescending(comment => comment.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(comment => new CommentDTOListItem
                {
                    Id = comment.Id,
                    FirstName = comment.FirstName,
                    UserName = comment.UserName,
                    Content = comment.Content,
                    CreatedAt = comment.CreatedAt,
                    LikesCount = comment.Likes.Count
                })
                .ToList();

            return commentsForTweet;
        }

        public async Task<Comment?> PostCommentToTweetAsync(SubmitCommentDTO submitCommentDTO, string userName, string firstName, string userId, Guid tweetId)
        {
            Comment comment = new()
            {
                Content = submitCommentDTO.Content,
                UserName = userName,
                FirstName = firstName,
                UserId = userId,
                TweetId = tweetId,
            };

            await context.Comments.AddAsync(comment);

            var tweet = await context.Tweets.FindAsync(tweetId);

            if (tweet == null) {
                return null;
            }

            var commentToPost = await context.Comments.FindAsync(comment.Id);

            if (commentToPost == null)
            {
                return null;
            }

            tweet.Comments.Add(commentToPost);

            await context.SaveChangesAsync();

            return comment;
        }

        public async Task<Comment?> DeleteCommentFromTweetAsync(Guid commentId, string userId)
        {
            var commentToDelete = await context.Comments.FindAsync(commentId);

            if (commentToDelete == null || commentToDelete.UserId != userId) { return null; }

            var tweet = await context.Tweets.FindAsync(commentToDelete.TweetId);

            if (tweet == null) { return null; }

            tweet.Comments.Remove(commentToDelete);
            context.Comments.Remove(commentToDelete);

            await context.SaveChangesAsync();

            return commentToDelete;
        }
    }
}
