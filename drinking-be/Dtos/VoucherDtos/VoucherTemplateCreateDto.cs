// Dtos/VoucherDtos/VoucherTemplateCreateDto.cs
using System.ComponentModel.DataAnnotations;
using System;

namespace drinking_be.Dtos.VoucherDtos
{
    public class VoucherTemplateCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, (double)decimal.MaxValue)]
        public decimal DiscountValue { get; set; }

        [Required]
        public string DiscountType { get; set; } = "Fixed"; // "Percent" or "Fixed"

        public decimal MinOrderValue { get; set; } = 0;
        public decimal? MaxDiscountAmount { get; set; }

        public int? UsageLimit { get; set; }
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        // Áp dụng cho hạng nào? (NULL = tất cả)
        public byte? LevelId { get; set; }
    }
}