// Repositories/OrderRepository.cs
using drinking_be.Interfaces.OrderInterfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Repositories
{
    // Giả định GenericRepository đã được định nghĩa
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DBDrinkContext _context;

        public OrderRepository(DBDrinkContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderWithDetails(
            Order order,
            List<OrderItem> allOrderItems)
        {
            // Bắt đầu một Transaction để đảm bảo tính nguyên tử (atomic)
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Lưu Order chính
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                // --- Liên kết OrderId cho tất cả OrderItems ---
                // Do chúng ta đang sử dụng các đối tượng đã được theo dõi (tracking) bởi EF
                // Nếu OrderItem không được theo dõi, ta cần gán thủ công:
                // allOrderItems.ForEach(item => item.OrderId = order.Id);

                // 2. Lưu TẤT CẢ Order Items (Món chính & Topping)
                // EF Core sẽ tự động xử lý ID của món chính và gán ParentItemId cho các topping
                await _context.OrderItems.AddRangeAsync(allOrderItems);
                await _context.SaveChangesAsync();

                // 3. Cam kết Transaction
                await transaction.CommitAsync();

                return order;
            }
            catch (Exception ex)
            {
                // 4. Hoàn tác Transaction nếu có lỗi
                await transaction.RollbackAsync();
                Console.WriteLine($"Lỗi khi tạo đơn hàng: {ex.Message}");
                throw; // Ném lỗi để Service biết và xử lý
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(int? userId)
        {
            var query = _context.Orders
                // Nạp thông tin Items
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product) // Nạp tên SP
                                                 // Nạp các Option của Item (Size, Sugar, Ice)
                .Include(o => o.OrderItems).ThenInclude(i => i.Size)
                .Include(o => o.OrderItems).ThenInclude(i => i.SugarLevel)
                .Include(o => o.OrderItems).ThenInclude(i => i.IceLevel)
                .AsQueryable();

            // Lọc theo User nếu có
            if (userId.HasValue)
            {
                query = query.Where(o => o.UserId == userId.Value);
            }

            // Mặc định sắp xếp giảm dần theo ngày tạo (Mới nhất lên đầu)
            // Việc sắp xếp chi tiết hơn (sort by field) có thể xử lý thêm ở Service hoặc Controller
            return await query.OrderByDescending(o => o.OrderDate).ToListAsync();
        }
    }
}