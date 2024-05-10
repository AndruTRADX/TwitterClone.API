using Microsoft.AspNetCore.Identity;

namespace TwitterClone.Models.Domains
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; } = string.Empty;
        public required string LastName { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public virtual List<string> TweetIds { get; set; } = [];
    }
}
