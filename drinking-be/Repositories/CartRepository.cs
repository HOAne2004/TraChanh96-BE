// Repositories/CartRepository.cs
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
            // Truy vấn này Eager Load tất cả thông tin cần thiết để hiển thị giỏ hàng
            return await _context.Carts
                // 1. Lấy CartItems
                .Include(c => c.CartItems)
                    // 2. Lấy thông tin Product (Tên, Ảnh)
                    .ThenInclude(ci => ci.Product)
                .Include(c => c.CartItems)
                    // 3. Lấy thông tin Size (Label)
                    .ThenInclude(ci => ci.Size)
                .Include(c => c.CartItems)
                    // 4. Lấy thông tin Sugar (Label)
                    .ThenInclude(ci => ci.SugarLevel)
                .Include(c => c.CartItems)
                    // 5. Lấy thông tin Ice (Label)
                    .ThenInclude(ci => ci.IceLevel)
                .Include(c => c.CartItems)
                    // 6. Lấy Topping (Items con)
                    .ThenInclude(ci => ci.InverseParentItem)
                        // 7. Lấy thông tin Product của Topping
                        .ThenInclude(topping => topping.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<CartItem?> GetCartItemByIdAsync(long cartItemId)
        {
            // Lấy 1 item (cần kèm topping nếu muốn xóa)
            return await _context.CartItems
                .Include(ci => ci.InverseParentItem) // Lấy các topping con
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
        }

        public void DeleteCartItem(CartItem cartItem)
        {
            // Xóa các topping con trước (nếu có)
            if (cartItem.InverseParentItem.Any())
            {
                _context.CartItems.RemoveRange(cartItem.InverseParentItem);
            }

            // Xóa món chính
            _context.CartItems.Remove(cartItem);
        }
    }
}