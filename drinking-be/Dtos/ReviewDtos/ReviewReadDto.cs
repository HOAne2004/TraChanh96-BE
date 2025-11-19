// Dtos/ReviewDtos/ReviewReadDto.cs
using System;

namespace drinking_be.Dtos.ReviewDtos
{
    public class ReviewReadDto
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public byte Rating { get; set; } // TINYINT (1-5)
        public DateTime CreatedAt { get; set; }

        // Thông tin người dùng (Lấy từ Join)
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? UserThumbnailUrl { get; set; }
    }
}