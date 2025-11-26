using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.OrderDtos
{
    public class OrderToppingCreateDto
    {
        [Required]
        public int ToppingId { get; set; } // ProductId của Topping

        [Required]
        [Range(1, 50)]
        public int Quantity { get; set; }
    }
}