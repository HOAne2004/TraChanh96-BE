using drinking_be.Dtos.OptionDtos;
using drinking_be.Interfaces; 
using drinking_be.Services;
using Microsoft.AspNetCore.Mvc;

namespace drinking_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IceLevelsController : ControllerBase
    {
        private readonly IIceLevelService _iceService;

        public IceLevelsController(IIceLevelService iceService)
        {
            _iceService = iceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIceLevels()
        {
            return Ok(await _iceService.GetAllIceLevelsAsync());
        }

        // ⭐️ API CHECK USAGE
        [HttpGet("{id}/usage")]
        public async Task<IActionResult> GetUsage(short id)
        {
            // Giả định bạn đã thêm hàm CountProductsUsingIceAsync vào Service
            // Nếu chưa, hãy thêm vào hoặc tạm return 0
            var count = await _iceService.CountProductsUsingIceLevelAsync(id);
            return Ok(new { count  });
        }

        [HttpPost]
        public async Task<IActionResult> CreateIceLevel([FromBody] IceLevelCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var newLevel = await _iceService.CreateIceLevelAsync(dto);
                return CreatedAtAction(nameof(GetAllIceLevels), newLevel);
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIceLevel(short id, [FromBody] IceLevelCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = await _iceService.UpdateIceLevelAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIceLevel(short id)
        {
            var result = await _iceService.DeleteIceLevelAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}