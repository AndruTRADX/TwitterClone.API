﻿using System.ComponentModel.DataAnnotations;

namespace TwitterClone.Models.DTOs
{
    public class SubmitCommentDTO
    {
        [Required]
        [MaxLength(255, ErrorMessage = "Tweet cannot be longer than 255")]
        [MinLength(1, ErrorMessage = "Please write content before publishing")]
        public string Content { get; set; } = string.Empty;
    }
}
