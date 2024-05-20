using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TwitterClone.Models.Domains;

namespace TwitterClone.Models.DTOs
{
    public class CommentDTOCreated
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
