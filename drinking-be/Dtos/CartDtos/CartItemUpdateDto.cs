using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.CartDtos
{
    public class CartItemUpdateDto
    {
        [Required]
        public long CartItemId { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Số lượng phải từ 1 đến 100.")]
        public int Quantity { get; set; }
    }
}   