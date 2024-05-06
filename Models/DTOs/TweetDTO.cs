﻿namespace TwitterClone.Models.DTOs
{
    public class TweetDTO
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public int Likes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<CommentDTO> Comments { get; set; } = [];
    }
}
