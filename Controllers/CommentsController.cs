// Controllers/CommentController.cs
using drinking_be.Dtos.CommentDtos;
using drinking_be.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace drinking_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
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
        /// Lấy tất cả bình luận cho một bài viết (theo News ID).
        /// </summary>
        [HttpGet("news/{newsId}")]
        [AllowAnonymous] // Cho phép tất cả mọi người xem
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CommentReadDto>))]
        public async Task<IActionResult> GetCommentsForNews(int newsId)
        {
            var comments = await _commentService.GetCommentsByNewsIdAsync(newsId);
            return Ok(comments);
        }

        // --- SECURE ENDPOINT (USER) ---

        /// <summary>
        /// [USER] Gửi bình luận cho một bài viết.
        /// </summary>
        [HttpPost]
        [Authorize] // ⭐️ Yêu cầu người dùng phải đăng nhập
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommentReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // News không tồn tại
        public async Task<IActionResult> CreateComment([FromBody] CommentCreateDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = GetUserIdFromToken();
                var createdComment = await _commentService.CreateCommentAsync(commentDto, userId);

                return StatusCode(StatusCodes.Status201Created, createdComment);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Bài viết không tồn tại
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Lỗi nghiệp vụ (ví dụ: ParentId không hợp lệ)
            }
        }
    }
}