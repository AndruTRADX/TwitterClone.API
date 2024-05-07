using Microsoft.EntityFrameworkCore;
using TwitterClone.Models.Domains;

namespace TwitterClone.Data
{
    public class TwitterCloneDbContext(DbContextOptions<TwitterCloneDbContext> dbContextOptions): DbContext(dbContextOptions)
    {
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
