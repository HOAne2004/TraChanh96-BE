// Dtos/MembershipLevelDtos/MembershipLevelCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.MembershipLevelDtos
{
    public class MembershipLevelCreateDto
    {
        [Required]
        [MaxLength(35)]
        public string Name { get; set; } = string.Empty; // Đồng, Bạc, Vàng...

        [Required]
        [Range(0, (double)decimal.MaxValue)]
        public decimal MinSpendRequired { get; set; } // Mức chi tiêu tối thiểu

        [Required]
        [Range(1, 3650)] // 1 ngày đến 10 năm
        public short DurationDays { get; set; } // Thời hạn duy trì cấp độ

        public string? Benefits { get; set; } // Mô tả quyền lợi (có thể là JSON)
    }
}