
namespace TwitterClone.Models.DTOs
{
    public class TweetDTO
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public int Likes { get; set; }
    }
}
