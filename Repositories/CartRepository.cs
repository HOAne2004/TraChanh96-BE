using drinking_be.Interfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drinking_be.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        private readonly new DBDrinkContext _context;

        public CartRepository(DBDrinkContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Size)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.SugarLevel)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.IceLevel)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.InverseParentItem) // Lấy topping
                        .ThenInclude(topping => topping.Product) // Lấy tên topping
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<CartItem?> GetCartItemByIdAsync(long cartItemId)
        {
            return await _context.CartItems
                // ⭐️ QUAN TRỌNG: Phải Include Size để tính lại giá khi update số lượng
                .Include(ci => ci.Size)
                // Phải Include Topping để cập nhật số lượng topping theo món chính
                .Include(ci => ci.InverseParentItem)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
        }

        public void DeleteCartItem(CartItem cartItem)
        {
            // Xóa các topping con trước (nếu có)
            if (cartItem.InverseParentItem != null && cartItem.InverseParentItem.Any())
            {
                _context.CartItems.RemoveRange(cartItem.InverseParentItem);
            }

            // Xóa món chính
            _context.CartItems.Remove(cartItem);
        }

        // ⭐️ BỔ SUNG HÀM NÀY (Service đang gọi mà chưa có)
        public async Task ClearCartItemsAsync(long cartId)
        {
            // Tìm tất cả item thuộc cart này
            var items = _context.CartItems.Where(ci => ci.CartId == cartId);

            // Xóa hết (EF Core đủ thông minh để xóa topping con nếu cấu hình Cascade, 
            // nhưng để an toàn ta cứ RemoveRange hết danh sách lấy được)
            _context.CartItems.RemoveRange(items);

            await _context.SaveChangesAsync();
        }
    }
}