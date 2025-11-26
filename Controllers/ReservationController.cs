using drinking_be.Dtos.ReservationDtos;
using drinking_be.Interfaces.ReservationInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace drinking_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        // POST: api/Reservation
        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationCreateDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Nếu user đã đăng nhập, tự động gán UserId nếu DTO chưa có
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && createDto.UserId == null)
            {
                createDto.UserId = int.Parse(userIdClaim.Value);
            }

            try
            {
                var result = await _reservationService.CreateReservationAsync(createDto);
                return CreatedAtAction(nameof(GetReservationById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Reservation/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationById(long id)
        {
            var result = await _reservationService.GetReservationByIdAsync(id);
            if (result == null) return NotFound(new { message = "Không tìm thấy thông tin đặt bàn." });
            return Ok(result);
        }

        // GET: api/Reservation/my-history
        // API này yêu cầu User phải đăng nhập (Authorize)
        [HttpGet("my-history")]
        [Authorize]
        public async Task<IActionResult> GetMyHistory()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _reservationService.GetHistoryByUserIdAsync(userId);
            return Ok(result);
        }

        // GET: api/Reservation/store/{storeId}
        // Dành cho Admin/Staff quán xem danh sách
        [HttpGet("store/{storeId}")]
        public async Task<IActionResult> GetByStore(int storeId, [FromQuery] DateTime? date)
        {
            var result = await _reservationService.GetReservationsByStoreAsync(storeId, date);
            return Ok(result);
        }

        // PUT: api/Reservation/{id}
        // Admin cập nhật trạng thái hoặc gán bàn
        [HttpPut("{id}")]
        // [Authorize(Roles = "Admin,Staff")] // Uncomment nếu có phân quyền
        public async Task<IActionResult> UpdateReservation(long id, [FromBody] ReservationUpdateDto updateDto)
        {
            try
            {
                var result = await _reservationService.UpdateReservationAsync(id, updateDto);
                if (!result) return NotFound();
                return Ok(new { message = "Cập nhật thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Reservation/{id}/cancel
        // User tự hủy đơn
        [HttpPut("{id}/cancel")]
        [Authorize]
        public async Task<IActionResult> CancelReservation(long id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            try
            {
                var result = await _reservationService.CancelReservationAsync(id, userId);
                if (!result) return BadRequest(new { message = "Không thể hủy đơn này (sai thông tin hoặc sai trạng thái)." });
                return Ok(new { message = "Đã hủy đặt bàn thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}