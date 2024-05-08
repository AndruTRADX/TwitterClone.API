using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TwitterClone.Models.Domains
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("Tweet")]
        public Guid TweetId { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [JsonIgnore]
        public virtual Tweet? Tweet { get; set; }

        [JsonIgnore]
        public virtual ICollection<LikeToComment> Likes { get; set; } = [];
    }
}
