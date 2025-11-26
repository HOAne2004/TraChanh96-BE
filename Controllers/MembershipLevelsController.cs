// Controllers/MembershipLevelController.cs
using drinking_be.Dtos.MembershipLevelDtos;
using drinking_be.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace drinking_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipLevelsController : ControllerBase
    {
        private readonly IMembershipLevelService _levelService;

        public MembershipLevelsController(IMembershipLevelService levelService)
        {
            _levelService = levelService;
        }

        // --- PUBLIC ENDPOINT (Khách hàng) ---

        /// <summary>
        /// Lấy danh sách tất cả các cấp độ thành viên.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MembershipLevelReadDto>))]
        public async Task<IActionResult> GetAllLevels()
        {
            var levels = await _levelService.GetAllLevelsAsync();
            return Ok(levels);
        }

        // --- ADMIN ENDPOINTS ---
        // (Giả định cần [Authorize(Roles = "Admin")])

        /// <summary>
        /// [ADMIN] Lấy chi tiết cấp độ theo ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MembershipLevelReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLevelById(byte id)
        {
            try
            {
                var level = await _levelService.GetLevelByIdAsync(id);
                return Ok(level);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// [ADMIN] Tạo cấp độ thành viên mới.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MembershipLevelReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLevel([FromBody] MembershipLevelCreateDto levelDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdLevel = await _levelService.CreateLevelAsync(levelDto);
                return CreatedAtAction(nameof(GetLevelById), new { id = createdLevel.Id }, createdLevel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Ví dụ: Tên trùng lặp
            }
        }

        /// <summary>
        /// [ADMIN] Cập nhật cấp độ thành viên.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MembershipLevelReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateLevel(byte id, [FromBody] MembershipLevelCreateDto levelDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedLevel = await _levelService.UpdateLevelAsync(id, levelDto);
                return Ok(updatedLevel);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}