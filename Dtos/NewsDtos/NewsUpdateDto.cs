using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.NewsDtos
{
    public class NewsUpdateDto
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;

        [Required]
        public int CategoryId { get; set; }

        // Admin ID typically comes from claims; kept for testing
        [Required]
        public int UserId { get; set; }

        public string? ThumbnailUrl { get; set; }

        // e.g. "Published", "Draft"
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = null!;
    }
}