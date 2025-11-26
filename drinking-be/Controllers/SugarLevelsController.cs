// Controllers/SugarLevelsController.cs
using drinking_be.Dtos.OptionDtos;
using drinking_be.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace drinking_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Đường dẫn: api/SugarLevels
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

        [HttpPost]
        public async Task<IActionResult> CreateSugarLevel([FromBody] SugarLevelCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                return Ok(await _sugarService.CreateSugarLevelAsync(dto));
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSugarLevel(short id, [FromBody] SugarLevelCreateDto dto)
        {
            var result = await _sugarService.UpdateSugarLevelAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSugarLevel(short id)
        {
            if (!await _sugarService.DeleteSugarLevelAsync(id)) return NotFound();
            return NoContent();
        }


        // GET: api/Sizes/5/usage
        [HttpGet("{id}/usage")]
        public async Task<IActionResult> GetSizeUsage(short id)
        {
            var count = await _sugarService.CountProductsUsingSugarLevelAsync(id);
            return Ok(new { count }); // Trả về JSON: { "count": 5 }
        }
    }
}