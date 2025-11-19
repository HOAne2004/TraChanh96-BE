// Controllers/AdminController.cs
using drinking_be.Dtos.UserDtos;
using drinking_be.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace drinking_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // ⭐️ CHỈ ADMIN MỚI ĐƯỢC GỌI
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// [ADMIN] Lấy danh sách tất cả người dùng.
        /// </summary>
        [HttpGet("users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserReadDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            return Ok(users);
        }

        // TODO: Thêm [HttpPut("users/{id}")] (Cập nhật role)
        // TODO: Thêm [HttpDelete("users/{id}")] (Xóa user)
    }
}