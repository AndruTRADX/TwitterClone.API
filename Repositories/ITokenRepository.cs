using Microsoft.AspNetCore.Identity;

namespace TwitterClone.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user);
    }
}
