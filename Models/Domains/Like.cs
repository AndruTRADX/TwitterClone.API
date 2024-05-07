using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TwitterClone.Models.Domains
{
    public class Like
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("Tweet")]
        public Guid TweetId { get; set; }

        [JsonIgnore]
        public Tweet? Tweet { get; set; }
    }
}
