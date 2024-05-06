using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TwitterClone.Models.Domains
{
    public class Tweet
    {
        [Key]
        public Guid Id { get; set; }

        public string Content {  get; set; } = string.Empty;

        public int Likes { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        
        public IdentityUser User { get; set; }
    }
}
