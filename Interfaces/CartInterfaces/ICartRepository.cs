// Interfaces/ICartRepository.cs
using drinking_be.Models;
using System.Threading.Tasks;

namespace drinking_be.Interfaces
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        // Lấy giỏ hàng (kèm items và sub-items) bằng User ID
        Task<Cart?> GetCartByUserIdAsync(int userId);

        // Lấy 1 item cụ thể (để xóa/cập nhật)
        Task<CartItem?> GetCartItemByIdAsync(long cartItemId);

        // Xóa CartItem (và các topping con của nó)
        void DeleteCartItem(CartItem cartItem);
    }
}