// Dtos/OptionDtos/SizeCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.OptionDtos
{
    public class SizeCreateDto
    {
        [Required]
        [MaxLength(20)]
        public string Label { get; set; } = string.Empty;

        // Giá phụ thu (Có thể là 0)
        [Range(0, 1000000, ErrorMessage = "Giá tiền không hợp lệ")]
        public decimal PriceModifier { get; set; } = 0m;
    }
}