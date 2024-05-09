using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TwitterClone.Models.Domains;

namespace TwitterClone.Data
{
    public class TwitterCloneAuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public TwitterCloneAuthDbContext(DbContextOptions<TwitterCloneAuthDbContext> options)
            : base(options)
        {
        }
    }
}