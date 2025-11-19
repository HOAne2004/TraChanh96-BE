// Dtos/CommentDtos/CommentCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.CommentDtos
{
    public class CommentCreateDto
    {
        [Required]
        public int NewsId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Content { get; set; } = string.Empty;

        // Dùng để trả lời bình luận khác (nếu là bình luận gốc thì để NULL)
        public int? ParentId { get; set; }

        // UserId sẽ được lấy từ Token JWT
    }
}