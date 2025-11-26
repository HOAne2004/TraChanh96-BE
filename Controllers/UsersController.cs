// Controllers/AuthController.cs
using drinking_be.Dtos.UserDtos;
using drinking_be.Interfaces.UserInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // Cần thiết để lấy thông tin từ JWT
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {

            _userService = userService;
        }

        // --- PUBLIC ENDPOINTS (Không cần xác thực) ---

        /// <summary>
        /// Đăng ký người dùng mới.
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous] // Cho phép truy cập mà không cần Token
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newUser = await _userService.RegisterAsync(registerDto);
                // Trả về 201 Created
                return CreatedAtAction(nameof(GetCurrentUser), new { id = newUser.PublicId }, newUser);
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException dbEx)
                {
                    // Lấy lỗi gốc từ SQL Server (nếu có)
                    var innerEx = dbEx.InnerException;
                    string errorMessage = "Lỗi CSDL: ";

                    if (innerEx != null)
                    {
                        errorMessage += innerEx.Message; // Lỗi thật nằm ở đây!
                    }
                    else
                    {
                        errorMessage += dbEx.Message; // Lỗi chung
                    }

                    // Trả về lỗi chi tiết
                    return BadRequest(errorMessage);
                }
                // Xử lý lỗi nghiệp vụ (ví dụ: "Email đã được sử dụng.")
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Đăng nhập và nhận Token JWT.
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))] // Trả về Token (chuỗi)
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var token = await _userService.LoginAsync(loginDto);
                // Trả về Token JWT thành công
                return Ok(token);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi xác thực mật khẩu/tên đăng nhập
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        // --- SECURE ENDPOINT (Cần xác thực) ---

        /// <summary>
        /// Lấy thông tin người dùng hiện tại từ Token JWT.
        /// </summary>
        [Authorize] // Bắt buộc phải có Token hợp lệ
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserReadDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCurrentUser()
        {
            // Tìm ID trong claim "sub" (nếu đã Clear map) hoặc "nameidentifier" (mặc định của .NET)
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)
                           ?? User.FindFirst(ClaimTypes.NameIdentifier);

            // Kiểm tra nếu không tìm thấy hoặc không phải Guid hợp lệ
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid publicId))
            {
                return Unauthorized("Token không hợp lệ hoặc lỗi phiên bản Token.");
            }

            var user = await _userService.GetUserByPublicIdAsync(publicId);

            if (user == null)
            {
                return NotFound("Không tìm thấy thông tin người dùng.");
            }

            return Ok(user);
        }
    }
}
