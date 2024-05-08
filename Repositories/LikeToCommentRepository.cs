using TwitterClone.Data;
using TwitterClone.Models.Domains;

namespace TwitterClone.Repositories
{
    public class LikeToCommentRepository(TwitterCloneDbContext context) : ILikeToCommentRepository
    {
        private readonly TwitterCloneDbContext _context = context;

        public async Task<LikeToComment?> LikeToCommentToggle(Guid commentId, string userId)
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment == null || userId == null)
            {
                return null;
            }

            var existingLike = _context.LikesToComments
                .FirstOrDefault(l => l.CommentId == commentId && l.UserId == userId);

            if (existingLike == null)
            {
                var newLikeToComment = new LikeToComment
                {
                    CommentId = commentId,
                    UserId = userId
                };

                await _context.LikesToComments.AddAsync(newLikeToComment);

                comment.Likes.Add(newLikeToComment);
                await _context.SaveChangesAsync();

                return newLikeToComment;
            }
            else
            {
                _context.LikesToComments.Remove(existingLike);

                comment.Likes.Remove(existingLike);
                await _context.SaveChangesAsync();

                return null;
            }
        }
    }
}
