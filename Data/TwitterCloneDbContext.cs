using Microsoft.EntityFrameworkCore;

namespace TwitterClone.Data
{
    public class TwitterCloneDbContext(DbContextOptions<TwitterCloneDbContext> dbContextOptions): DbContext(dbContextOptions)
    {
    }
}
