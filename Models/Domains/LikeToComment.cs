using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TwitterClone.Models.Domains
{
    public class LikeToComment
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("Comment")]
        public Guid CommentId { get; set; }

        [JsonIgnore]
        public Comment? Comment { get; set; }
    }
}
