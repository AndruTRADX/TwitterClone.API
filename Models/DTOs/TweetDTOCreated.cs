﻿namespace TwitterClone.Models.DTOs
{
    public class TweetDTOCreated
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
