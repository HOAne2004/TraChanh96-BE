namespace drinking_be.Dtos.CartDtos
{
    public class CartItemReadDto
    {
        public long Id { get; set; } // CartItem ID
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }

        // Giá
        public decimal BasePrice { get; set; } // Giá gốc sản phẩm
        public decimal FinalPrice { get; set; } // Tổng giá item này (Đã bao gồm Size + Topping * Quantity)

        // Options (Hiển thị tên để FE đỡ phải tra cứu lại)
        public string SizeLabel { get; set; } = string.Empty;
        public string SugarLabel { get; set; } = string.Empty;
        public string IceLabel { get; set; } = string.Empty;

        // Danh sách Topping đi kèm món này
        public List<CartToppingReadDto> Toppings { get; set; } = new List<CartToppingReadDto>();
    }
}