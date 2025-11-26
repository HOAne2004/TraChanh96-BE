// Dtos/ReviewDtos/ReviewCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.ReviewDtos
{
    public class ReviewCreateDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, 5)]
        public byte Rating { get; set; } // TINYINT (1-5)

        [MaxLength(1000)]
        public string? Content { get; set; }

        // UserId sẽ được lấy từ Token JWT, không cần trong DTO
    }
}