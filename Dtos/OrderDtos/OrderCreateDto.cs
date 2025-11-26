// Dtos/OrderDtos/OrderCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.OrderDtos
{
    public class OrderCreateDto
    {
        // Thông tin Khách hàng
        [Required]
        [MaxLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [Phone] // Validation phone number
        public string CustomerPhone { get; set; } = string.Empty;

        // Thông tin Đặt hàng
        public int? UserId { get; set; } // Có thể NULL (Khách vãng lai)

        [Required]
        public int StoreId { get; set; }

        [Required]
        public int PaymentMethodId { get; set; }

        [Required]
        [MaxLength(500)]
        public string DeliveryAddress { get; set; } = string.Empty;

        // Mã giảm giá
        public string? VoucherCodeUsed { get; set; }

        // Danh sách tất cả các món hàng trong đơn hàng
        [Required]
        [MinLength(1, ErrorMessage = "Đơn hàng phải có ít nhất một món.")]
        public List<OrderItemCreateDto> Items { get; set; } = new List<OrderItemCreateDto>();
    }
}