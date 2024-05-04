using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TwitterClone.Data
{
    public class TwitterCloneAuthDbContext(DbContextOptions<TwitterCloneAuthDbContext> options) : IdentityDbContext(options)
    {

    }
}
