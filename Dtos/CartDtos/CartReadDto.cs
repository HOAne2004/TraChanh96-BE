// Dtos/CartDtos/CartReadDto.cs

namespace drinking_be.Dtos.CartDtos
{
    public class CartReadDto // Toàn bộ giỏ hàng
    {
        public long Id { get; set; } // Cart ID
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; } // Tổng tiền
        public List<CartItemReadDto> Items { get; set; } = new List<CartItemReadDto>();
    }
}