namespace drinking_be.Dtos.CartDtos
{
    public class CartToppingReadDto
    {
        public long Id { get; set; } // CartItem ID của dòng topping này
        public int ProductId { get; set; } // ID của sản phẩm topping
        public string ProductName { get; set; } = string.Empty; // Tên topping (VD: Trân châu đen)
        public string? ImageUrl { get; set; } // (Tùy chọn) Ảnh topping nếu muốn hiện
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } // Giá đơn vị của topping
        public decimal FinalPrice { get; set; } // Tổng giá topping (UnitPrice * Quantity)
    }
}