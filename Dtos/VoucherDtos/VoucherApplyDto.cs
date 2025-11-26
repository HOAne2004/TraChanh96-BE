// Dtos/VoucherDtos/VoucherApplyDto.cs
using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.VoucherDtos
{
    public class VoucherApplyDto
    {
        [Required]
        public string VoucherCode { get; set; } = string.Empty;

        [Required]
        public decimal OrderTotalAmount { get; set; } // Tổng tiền tạm tính của đơn hàng
    }
}