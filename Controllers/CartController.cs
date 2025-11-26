// Controllers/CartController.cs
using drinking_be.Dtos.CartDtos;
using drinking_be.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace drinking_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // ⭐️ Yêu cầu đăng nhập cho toàn bộ giỏ hàng
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // --- Helper Function ---
        private int GetUserIdFromToken()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("User ID không hợp lệ trong token.");
        }

        /// <summary>
        /// Lấy giỏ hàng hiện tại của người dùng.
        /// </summary>
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartReadDto))]
        public async Task<IActionResult> GetMyCart()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var cart = await _cartService.GetMyCartAsync(userId);
                return Ok(cart);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        /// <summary>
        /// Thêm một món hàng (và topping) vào giỏ.
        /// </summary>
        [HttpPost("add-item")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddItemToCart([FromBody] CartItemCreateDto itemDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userId = GetUserIdFromToken();
                var cart = await _cartService.AddItemToCartAsync(userId, itemDto);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Ví dụ: Product/Size không hợp lệ
            }
        }

        /// <summary>
        /// Xóa một món hàng (và các topping kèm theo) khỏi giỏ.
        /// </summary>
        /// <param name="cartItemId">ID của CartItem (món chính)</param>
        [HttpDelete("remove-item/{cartItemId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveItemFromCart(long cartItemId)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var cart = await _cartService.RemoveItemFromCartAsync(userId, cartItemId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message); // Ví dụ: Item không tìm thấy
            }
        }

        /// <summary>
        /// Xóa sạch tất cả các mục khỏi giỏ hàng của người dùng hiện tại.
        /// </summary>
        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartReadDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = GetUserIdFromToken(); // Lấy User ID từ Token

                // Gọi Service (Service này đã có logic ClearCartAsync)
                await _cartService.ClearCartAsync(userId);

                // Trả về giỏ hàng mới (đã rỗng)
                var newCart = await _cartService.GetMyCartAsync(userId);
                return Ok(newCart);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật số lượng món hàng.
        /// </summary>
        [HttpPut("update-item")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateItemQuantity([FromBody] CartItemUpdateDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userId = GetUserIdFromToken();
                var cart = await _cartService.UpdateItemQuantityAsync(userId, updateDto);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}