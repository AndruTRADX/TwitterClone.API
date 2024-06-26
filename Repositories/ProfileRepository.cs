﻿using Microsoft.EntityFrameworkCore;
using TwitterClone.Data;
using TwitterClone.Models.Domains;
using TwitterClone.Models.DTOs;

namespace TwitterClone.Repositories
{
    public class ProfileRepository(TwitterCloneAuthDbContext authDbContext, TwitterCloneDbContext dbContext) : IProfileRepository
    {
        private readonly TwitterCloneAuthDbContext authDbContext = authDbContext;
        private readonly TwitterCloneDbContext dbContext = dbContext;

        public async Task<ApplicationUser?> GetUserProfileAsync(string userName)
        {
            return await authDbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<List<TweetDTOListItem>?> GetAllUserTweetsAsync(string userName, int pageNumber, int pageSize)
        {
            var user = await authDbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);

            if (user == null)
            {
                return new List<TweetDTOListItem>();
            }

            var tweetIds = user.TweetIds.Select(id => Guid.Parse(id)).ToList();

            var userTweets = await dbContext.Tweets
                .Where(tweet => tweetIds.Contains(tweet.Id))
                .OrderByDescending(tweet => tweet.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userTweetsDTO = new List<TweetDTOListItem>();

            foreach (var tweet in userTweets)
            {
                var tweetUser = await authDbContext.Users.FindAsync(tweet.UserId);
                if (tweetUser != null)
                {
                    userTweetsDTO.Add(new TweetDTOListItem
                    {
                        Id = tweet.Id,
                        UserName = tweetUser.UserName!,
                        FirstName = tweetUser.FirstName,
                        Content = tweet.Content,
                        Likes = tweet.Likes.Count(),
                        CreatedAt = tweet.CreatedAt,
                        CommentsCount = tweet.Comments.Count
                    });
                }
            }

            return userTweetsDTO;
        }

        public async Task<List<ApplicationUser>> SearchUsersAsync(string searchTerm)
        {
            var matchedUsers = await authDbContext.Users
                .Where(user =>
                    EF.Functions.Like(user.UserName, $"%{searchTerm}%") ||
                    EF.Functions.Like(user.FirstName, $"%{searchTerm}%") ||
                    EF.Functions.Like(user.LastName, $"%{searchTerm}%"))
                .Take(15)
                .ToListAsync();

            var orderedUsers = matchedUsers
                .OrderByDescending(user =>
                    (user.UserName.Contains(searchTerm) ? 3 : 0) + // Ponderación alta si coincide en UserName
                    (user.FirstName.Contains(searchTerm) ? 2 : 0) + // Ponderación media si coincide en FirstName
                    (user.LastName.Contains(searchTerm) ? 1 : 0)) // Ponderación baja si coincide en LastName
                .ToList();

            return orderedUsers;
        }

        public async Task<ApplicationUser?> UpdateUserProfileAsync(SubmitProfileDTO submitProfileDTO, string userId)
        {
            var user = await authDbContext.Users.FindAsync(userId);

            if (user == null)
            {
                return null;
            }

            user.UserName = submitProfileDTO.UserName;
            user.FirstName = submitProfileDTO.FirstName;
            user.LastName = submitProfileDTO.LastName;
            user.Email = submitProfileDTO.Email;
            user.Biography = submitProfileDTO.Biography;

            await authDbContext.SaveChangesAsync();

            return user;
        }
    }
}
