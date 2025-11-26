// Dtos/OrderDtos/OrderReadDto.cs
namespace drinking_be.Dtos.OrderDtos
{
    public class OrderReadDto
    {
        public long Id { get; set; }
        public string OrderCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // Chuyển từ TINYINT sang string (New, Confirmed...)

        // Thông tin Khách hàng
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;

        // Tổng tiền
        public decimal TotalAmount { get; set; } // Tổng trước chiết khấu/phí ship
        public decimal DiscountAmount { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal GrandTotal { get; set; } // Tổng cuối cùng

        // Thời gian
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? CreatedAt { get; set; }

        // Chi tiết các món hàng
        public List<OrderItemReadDto> Items { get; set; } = new List<OrderItemReadDto>();
    }
}