// Controllers/MembershipController.cs
using drinking_be.Interfaces;
using drinking_be.Dtos.MembershipDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // Cần để lấy User ID từ Token

namespace drinking_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // ⭐️ Yêu cầu người dùng phải đăng nhập
    public class MembershipsController : ControllerBase
    {
        private readonly IMembershipService _membershipService;

        public MembershipsController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
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
        /// [USER] Lấy thông tin thành viên (cấp độ, điểm, v.v.) của người dùng hiện tại.
        /// </summary>
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MembershipReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyMembership()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var membershipInfo = await _membershipService.GetMyMembershipAsync(userId);

                if (membershipInfo == null)
                {
                    return NotFound("Không tìm thấy thông tin thành viên cho người dùng này.");
                }

                return Ok(membershipInfo);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}