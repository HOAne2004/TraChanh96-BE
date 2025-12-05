using drinking_be.Dtos.OptionDtos;
using drinking_be.Interfaces; // Namespace chứa ISugarLevelService
using Microsoft.AspNetCore.Mvc;

namespace drinking_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SugarLevelsController : ControllerBase
    {
        private readonly ISugarLevelService _sugarService;

        public SugarLevelsController(ISugarLevelService sugarService)
        {
            _sugarService = sugarService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSugarLevels()
        {
            return Ok(await _sugarService.GetAllSugarLevelsAsync());
        }

        // ⭐️ API CHECK USAGE
        [HttpGet("{id}/usage")]
        public async Task<IActionResult> GetUsage(short id)
        {
            // Giả định bạn đã thêm hàm CountProductsUsingSugarAsync vào Service
            // Nếu chưa, hãy thêm vào hoặc tạm return 0
             var count = await _sugarService.CountProductsUsingSugarLevelAsync(id);
            return Ok(new { count }); // Tạm thời trả về 0 để không lỗi 404
        }

        [HttpPost]
        public async Task<IActionResult> CreateSugarLevel([FromBody] SugarLevelCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var newLevel = await _sugarService.CreateSugarLevelAsync(dto);
                return CreatedAtAction(nameof(GetAllSugarLevels), newLevel);
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSugarLevel(short id, [FromBody] SugarLevelCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = await _sugarService.UpdateSugarLevelAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSugarLevel(short id)
        {
            var result = await _sugarService.DeleteSugarLevelAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}