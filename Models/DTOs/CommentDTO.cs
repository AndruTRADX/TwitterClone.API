namespace TwitterClone.Models.DTOs
{
    public class CommentDTO
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
