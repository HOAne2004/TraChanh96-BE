// Dtos/CommentDtos/CommentReadDto.cs
using System;

namespace drinking_be.Dtos.CommentDtos
{
    public class CommentReadDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; } // Hỗ trợ bình luận lồng nhau
        public int NewsId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Thông tin người dùng (Lấy từ Join)
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? UserThumbnailUrl { get; set; }
    }
}