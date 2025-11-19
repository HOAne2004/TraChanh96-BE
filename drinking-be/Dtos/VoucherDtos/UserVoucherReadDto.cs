// Dtos/VoucherDtos/UserVoucherReadDto.cs
using System;

namespace drinking_be.Dtos.VoucherDtos
{
    public class UserVoucherReadDto
    {
        public long Id { get; set; }
        public string VoucherCode { get; set; } = string.Empty; // Mã code duy nhất
        public short Status { get; set; } // 1:Unused, 2:Used, 3:Expired
        public DateTime ExpiryDate { get; set; }

        // Thông tin chi tiết từ Template
        public string TemplateName { get; set; } = string.Empty;
        public decimal DiscountValue { get; set; }
        public string DiscountType { get; set; } = string.Empty;
        public decimal MinOrderValue { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
    }
}