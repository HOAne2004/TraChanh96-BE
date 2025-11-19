using drinking_be.Models;

namespace drinking_be.Interfaces.OrderInterfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        /// <summary>
        /// Tạo đơn hàng mới, đảm bảo việc lưu Order và tất cả OrderItems (bao gồm cả Topping)
        /// được thực hiện trong một Transaction.
        /// </summary>
        /// <param name="order">Entity Order chính.</param>
        /// <param name="allOrderItems">Danh sách tất cả OrderItem (món chính và topping).</param>
        /// <returns>Order Entity đã được tạo.</returns>
        Task<Order> CreateOrderWithDetails(Order order,
                                          List<OrderItem> allOrderItems);

        Task<IEnumerable<Order>> GetOrdersAsync(int? userId);

    }
}