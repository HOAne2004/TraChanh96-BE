// Interfaces/ICartService.cs
using drinking_be.Dtos.CartDtos;
using System.Threading.Tasks;

namespace drinking_be.Interfaces
{
    public interface ICartService
    {
        // Lấy giỏ hàng của tôi
        Task<CartReadDto> GetMyCartAsync(int userId);

        // Thêm sản phẩm vào giỏ
        Task<CartReadDto> AddItemToCartAsync(int userId, CartItemCreateDto itemDto);

        // Xóa sản phẩm khỏi giỏ
        Task<CartReadDto> RemoveItemFromCartAsync(int userId, long cartItemId);

        // Cập nhật số lượng (TODO)
        // Task<CartReadDto> UpdateItemQuantityAsync(int userId, long cartItemId, int quantity);

        // Xóa sạch giỏ hàng (sau khi đặt hàng)
        Task ClearCartAsync(int userId);

        //Cập nhật số lượng sản phẩm trong giỏ hàng
        Task<CartReadDto> UpdateItemQuantityAsync(int userId, CartItemUpdateDto updateDto);
    }
}