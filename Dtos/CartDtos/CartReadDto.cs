// Dtos/CartDtos/CartReadDto.cs

namespace drinking_be.Dtos.CartDtos
{
    public class CartToppingReadDto // Chi tiết 1 topping
    {
        public long Id { get; set; } // CartItem ID của topping
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal FinalPrice { get; set; } // Giá của topping
    }

    public class CartItemReadDto // Chi tiết 1 món trong giỏ
    {
        public long Id { get; set; } // CartItem ID
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }

        public decimal BasePrice { get; set; } // Giá gốc
        public decimal FinalPrice { get; set; } // Giá (đã tính size)

        // Options
        public string SizeLabel { get; set; } = string.Empty;
        public string SugarLabel { get; set; } = string.Empty;
        public string IceLabel { get; set; } = string.Empty;

        // Toppings
        public List<CartToppingReadDto> Toppings { get; set; } = new List<CartToppingReadDto>();
    }

    public class CartReadDto // Toàn bộ giỏ hàng
    {
        public long Id { get; set; } // Cart ID
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; } // Tổng tiền
        public List<CartItemReadDto> Items { get; set; } = new List<CartItemReadDto>();
    }
}