// Dtos/OrderDtos/OrderItemCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.OrderDtos
{
    public class OrderItemCreateDto
    {
        [Required]
        public int ProductId { get; set; } // ProductId của món chính

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }

        // Tùy chọn Options
        [Required]
        public short SizeId { get; set; }
        [Required]
        public short SugarLevelId { get; set; }
        [Required]
        public short IceLevelId { get; set; }

        // Danh sách các topping đính kèm (OrderToppingCreateDto)
        public List<OrderToppingCreateDto> Toppings { get; set; } = new List<OrderToppingCreateDto>();
    }
}