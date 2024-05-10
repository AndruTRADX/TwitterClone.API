using TwitterClone.Models.Domains;
using TwitterClone.Models.DTOs;

namespace TwitterClone.Repositories
{
    public interface IProfileRepository
    {
        Task<ApplicationUser?> GetUserProfileAsync(string userName);
        Task<List<TweetDTOListItem>?> GetAllUserTweetsAsync(string userName, int pageNumber, int pageSize);
        Task<List<ApplicationUser>> SearchUsersAsync(string searchTerm);
        Task<ApplicationUser?> UpdateUserProfileAsync(SubmitProfileDTO submitProfileDTO, string userId);
    }
}
