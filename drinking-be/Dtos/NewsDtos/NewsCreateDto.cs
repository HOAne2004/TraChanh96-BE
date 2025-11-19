using System.ComponentModel.DataAnnotations;
using System;

namespace drinking_be.Dtos.NewsDtos
{
    public class NewsCreateDto
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        // Giả định Admin ID sẽ được lấy từ JWT/Claim, nhưng ta vẫn giữ để test
        [Required]
        public int UserId { get; set; }

        public string? ThumbnailUrl { get; set; }

        public string Status { get; set; } = "Draft"; // Mặc định là Draft
    }
}