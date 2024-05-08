namespace TwitterClone.Models.DTOs
{
    public class CommentDTOListItem
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int LikesCount { get; set; }
    }
}
