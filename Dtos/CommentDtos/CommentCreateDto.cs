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

        public int? ParentId { get; set; }

    }
}