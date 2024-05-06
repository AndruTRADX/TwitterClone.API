using Microsoft.AspNetCore.Identity;
using TwitterClone.Data;

namespace TwitterClone.Repositories
{
    public class TweetRepository(TwitterCloneDbContext context, UserManager<IdentityUser> userManager) : ITweetRepository
    {
        public Task CreateTweet()
        {
            return Task.CompletedTask;
        }
    }
}
