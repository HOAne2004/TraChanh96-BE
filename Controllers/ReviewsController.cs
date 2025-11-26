// Controllers/ReviewController.cs
using drinking_be.Dtos.ReviewDtos;
using drinking_be.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // Cần để lấy User ID

namespace drinking_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
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

        // --- PUBLIC ENDPOINT ---

        /// <summary>
        /// Lấy tất cả các đánh giá đã được duyệt cho một sản phẩm.
        /// </summary>
        /// <param name="productId">ID của sản phẩm.</param>
        [HttpGet("product/{productId}")]
        [AllowAnonymous] // Cho phép tất cả mọi người xem
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReviewReadDto>))]
        public async Task<IActionResult> GetReviewsForProduct(int productId)
        {
            var reviews = await _reviewService.GetApprovedReviewsAsync(productId);
            return Ok(reviews);
        }

        // --- SECURE ENDPOINT (USER) ---

        /// <summary>
        /// [USER] Gửi đánh giá cho một sản phẩm.
        /// </summary>
        [HttpPost]
        [Authorize] // ⭐️ Yêu cầu người dùng phải đăng nhập
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReviewReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Lỗi nghiệp vụ (đã review)
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Product không tồn tại
        public async Task<IActionResult> CreateReview([FromBody] ReviewCreateDto reviewDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = GetUserIdFromToken();
                var createdReview = await _reviewService.CreateReviewAsync(reviewDto, userId);

                // Trả về 201 Created (có thể dùng GetById nếu có)
                return StatusCode(StatusCodes.Status201Created, createdReview);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Sản phẩm không tồn tại
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Lỗi nghiệp vụ (ví dụ: đã review)
            }
        }
    }
}