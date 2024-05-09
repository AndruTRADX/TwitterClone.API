using TwitterClone.Models.Domains;

namespace TwitterClone.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(ApplicationUser user);
    }
}
