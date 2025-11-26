using drinking_be.Dtos.OrderDtos;
using drinking_be.Models;

namespace drinking_be.Interfaces.OrderInterfaces
{
    public interface IOrderService
    {
        // Phương thức chính: Nhận DTO và xử lý toàn bộ logic tạo đơn hàng
        Task<OrderReadDto> CreateOrderAsync(OrderCreateDto orderDto);

        Task<IEnumerable<OrderReadDto>> GetOrdersAsync(int? userId);

        // Các phương thức khác: Get, Update Status, v.v.
    }
}