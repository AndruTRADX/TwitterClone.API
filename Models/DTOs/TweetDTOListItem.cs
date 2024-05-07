namespace TwitterClone.Models.DTOs
{
    public class TweetDTOListItem
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public int CommentsCount { get; set; }

        public int Likes { get; set; }
    }
}
