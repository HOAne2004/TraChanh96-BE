// Dtos/VoucherDtos/VoucherApplyResultDto.cs

namespace drinking_be.Dtos.VoucherDtos
{
    public class VoucherApplyResultDto
    {
        public decimal DiscountAmount { get; set; } // Số tiền được giảm
        public decimal FinalAmount { get; set; } // Số tiền cuối cùng
        public string VoucherCode { get; set; } = string.Empty;
    }
}