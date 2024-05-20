using Microsoft.EntityFrameworkCore;
using TwitterClone.Data;
using TwitterClone.Models.Domains;
using TwitterClone.Models.DTOs;

namespace TwitterClone.Repositories
{
    public class CommentRepository(TwitterCloneDbContext context, TwitterCloneAuthDbContext authDbContext) : ICommentRepository
    {
        private readonly TwitterCloneDbContext context = context;
        private readonly TwitterCloneAuthDbContext authDbContext = authDbContext;

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

            var commentDTOs = new List<CommentDTOListItem>();

            foreach (var comment in tweetWithComments.Comments)
            {
                var user = await authDbContext.Users.FindAsync(comment.UserId);
                if (user != null)
                {
                    commentDTOs.Add(new CommentDTOListItem
                    {
                        Id = comment.Id,
                        UserName = user.UserName!,
                        FirstName = user.FirstName,
                        Content = comment.Content,
                        CreatedAt = comment.CreatedAt,
                        LikesCount = comment.Likes.Count
                    });
                }
            }

            return commentDTOs
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }


        public async Task<Comment?> PostCommentToTweetAsync(SubmitCommentDTO submitCommentDTO, string userId, Guid tweetId)
        {
            Comment comment = new()
            {
                Content = submitCommentDTO.Content,
                UserId = userId,
                TweetId = tweetId,
            };

            await context.Comments.AddAsync(comment);

            var tweet = await context.Tweets.FindAsync(tweetId);

            if (tweet == null)
            {
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

            if (commentToDelete == null || commentToDelete.UserId != userId)
            {
                return null;
            }

            var tweet = await context.Tweets.FindAsync(commentToDelete.TweetId);

            if (tweet == null)
            {
                return null;
            }

            tweet.Comments.Remove(commentToDelete);
            context.Comments.Remove(commentToDelete);

            await context.SaveChangesAsync();

            return commentToDelete;
        }

    }
}
