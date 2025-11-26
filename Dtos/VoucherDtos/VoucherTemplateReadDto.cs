// Dtos/VoucherDtos/VoucherTemplateReadDto.cs
using System;

namespace drinking_be.Dtos.VoucherDtos
{
    public class VoucherTemplateReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Thông tin giảm giá
        public decimal DiscountValue { get; set; }
        public string DiscountType { get; set; } = string.Empty; // Percent or Fixed
        public decimal MinOrderValue { get; set; }
        public decimal? MaxDiscountAmount { get; set; }

        // Thông tin phát hành
        public int? UsageLimit { get; set; } // Tổng số lượt sử dụng
        public int UsedCount { get; set; } // Số lượt đã sử dụng
        public bool IsActive { get; set; }

        // Thời gian
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Đối tượng áp dụng (Cần Join)
        public byte? LevelId { get; set; }
        public string LevelName { get; set; } = string.Empty; // Tên hạng: Vàng, Bạc...
    }
}