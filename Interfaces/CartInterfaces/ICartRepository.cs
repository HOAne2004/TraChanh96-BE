// Interfaces/ICartRepository.cs
using drinking_be.Interfaces;
using drinking_be.Models;

public interface ICartRepository : IGenericRepository<Cart>
{
    Task<Cart?> GetCartByUserIdAsync(int userId);
    Task<CartItem?> GetCartItemByIdAsync(long cartItemId);
    void DeleteCartItem(CartItem cartItem);

    // ⭐️ Thêm dòng này
    Task ClearCartItemsAsync(long cartId);
}