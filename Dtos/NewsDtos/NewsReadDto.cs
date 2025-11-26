using System;

namespace drinking_be.Dtos.NewsDtos
{
    public class NewsReadDto
    {
        public long Id { get; set; }
        public Guid PublicId { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public string Status { get; set; } = string.Empty; // Published, Draft

        // Thông tin liên kết
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public DateTime? PublishedDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}