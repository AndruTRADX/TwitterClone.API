using Microsoft.EntityFrameworkCore;
using TwitterClone.Models.Domains;

namespace TwitterClone.Data
{
    public class TwitterCloneDbContext(DbContextOptions<TwitterCloneDbContext> dbContextOptions): DbContext(dbContextOptions)
    {
        public DbSet<Tweet> Tweets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureTweetEntity(modelBuilder);
        }

        private static void ConfigureTweetEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tweet>(entity =>
            {
                entity.ToTable("Tweets");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Content).IsRequired().HasMaxLength(255);
                entity.Property(t => t.Likes).HasDefaultValue(0);
                entity.HasOne(t => t.User).WithMany().HasForeignKey(t => t.UserId).IsRequired();
            });
        }

    }
}
