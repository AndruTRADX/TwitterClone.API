using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TwitterClone.Models.Domains;

namespace TwitterClone.Data
{
    public class TwitterCloneAuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public TwitterCloneAuthDbContext(DbContextOptions<TwitterCloneAuthDbContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}