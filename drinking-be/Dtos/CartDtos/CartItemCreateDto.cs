// Dtos/CartDtos/CartItemCreateDto.cs
using System.ComponentModel.DataAnnotations;
using drinking_be.Dtos.OrderDtos; // Tận dụng DTO của Order

namespace drinking_be.Dtos.CartDtos
{
    // Chúng ta có thể TÁI SỬ DỤNG cấu trúc của OrderItemCreateDto
    // Nhưng để rõ ràng, ta sẽ tạo DTO riêng (hoặc kế thừa)

    public class CartToppingCreateDto
    {
        [Required]
        public int ProductId { get; set; } // ToppingId

        [Required]
        [Range(1, 50)]
        public int Quantity { get; set; }
    }

    public class CartItemCreateDto
    {
        [Required]
        public int ProductId { get; set; } // Trà sữa

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }

        // Tùy chọn bắt buộc
        public short SizeId { get; set; }
        public short SugarLevelId { get; set; }
        public short IceLevelId { get; set; }

        // Danh sách các topping đính kèm
        public List<CartToppingCreateDto> Toppings { get; set; } = new List<CartToppingCreateDto>();
    }
}