// Dtos/OrderDtos/OrderItemReadDto.cs
namespace drinking_be.Dtos.OrderDtos
{
    public class OrderItemReadDto
    {
        public long Id { get; set; } // Order_item ID
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal BasePrice { get; set; } // Giá gốc của sản phẩm
        public decimal FinalPrice { get; set; } // Giá của món chính (sau khi tính size/options)

        // Options
        public string SizeLabel { get; set; } = string.Empty;
        public string SugarLabel { get; set; } = string.Empty;
        public string IceLabel { get; set; } = string.Empty;

        // Toppings (Order Items con)
        public List<OrderToppingReadDto> Toppings { get; set; } = new List<OrderToppingReadDto>();
    }
}