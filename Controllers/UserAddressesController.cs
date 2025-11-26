// Controllers/UserAddressesController.cs
using drinking_be.Dtos.UserAddressDtos;
using drinking_be.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace drinking_be.Controllers
{
    [Route("api/users/addresses")] // Đặt route rõ ràng dưới namespace user
    [ApiController]
    [Authorize] // ⭐️ Yêu cầu đăng nhập cho toàn bộ Controller
    public class UserAddressesController : ControllerBase
    {
        private readonly IUserAddressService _addressService;

        public UserAddressesController(IUserAddressService addressService)
        {
            _addressService = addressService;
        }

        // --- Helper Function ---
        private int GetUserIdFromToken()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }
            // Token không hợp lệ (mặc dù Authorize attribute đã bắt)
            throw new UnauthorizedAccessException("User ID không hợp lệ trong token.");
        }

        // GET: /api/users/addresses
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserAddressReadDto>))]
        public async Task<IActionResult> GetMyAddresses()
        {
            var userId = GetUserIdFromToken();
            var addresses = await _addressService.GetAllMyAddressesAsync(userId);
            return Ok(addresses);
        }

        // POST: /api/users/addresses
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserAddressReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAddress([FromBody] UserAddressCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userId = GetUserIdFromToken();
                var newAddress = await _addressService.CreateAddressAsync(userId, dto);

                return Created(string.Empty, newAddress);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: /api/users/addresses/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserAddressReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] UserAddressCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetUserIdFromToken();
            var updatedAddress = await _addressService.UpdateAddressAsync(id, userId, dto);

            if (updatedAddress == null) return NotFound();

            return Ok(updatedAddress);
        }

        // DELETE: /api/users/addresses/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var userId = GetUserIdFromToken();
            var result = await _addressService.DeleteAddressAsync(id, userId);

            if (!result) return NotFound();

            return NoContent();
        }
    }
}