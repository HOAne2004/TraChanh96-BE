// Controllers/VoucherController.cs
using drinking_be.Dtos.VoucherDtos;
using drinking_be.Interfaces;
using Microsoft.AspNetCore.Authorization; // Cần thiết
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // Cần thiết để lấy User ID từ Token

namespace drinking_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // ⭐️ Yêu cầu người dùng phải đăng nhập cho tất cả API trong Controller này
    public class VouchersController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VouchersController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        // --- Helper Function ---
        /// <summary>
        /// Lấy User ID (int) từ Token JWT
        /// </summary>
        private int GetUserIdFromToken()
        {
            // Chúng ta đã lưu ID (PK) trong ClaimTypes.NameIdentifier khi tạo token
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("User ID không hợp lệ trong token.");
        }

        /// <summary>
        /// [USER] Lấy danh sách voucher hợp lệ (chưa dùng, chưa hết hạn) của người dùng hiện tại.
        /// </summary>
        [HttpGet("my-vouchers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserVoucherReadDto>))]
        public async Task<IActionResult> GetMyVouchers()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var vouchers = await _voucherService.GetUserVouchersAsync(userId);
                return Ok(vouchers);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        /// <summary>
        /// [USER] Kiểm tra và tính toán giá trị áp dụng voucher.
        /// </summary>
        [HttpPost("apply")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VoucherApplyResultDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Lỗi nghiệp vụ (hết hạn, không đủ điều kiện)
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ApplyVoucher([FromBody] VoucherApplyDto applyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = GetUserIdFromToken();
                var result = await _voucherService.ApplyVoucherAsync(userId, applyDto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                // Lỗi nghiệp vụ (VD: "Mã không hợp lệ", "Không đủ điều kiện")
                return BadRequest(ex.Message);
            }
        }
    }
}