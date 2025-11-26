// Controllers/IceLevelsController.cs
using drinking_be.Dtos.OptionDtos;
using drinking_be.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace drinking_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Đường dẫn: api/IceLevels
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

        [HttpPost]
        public async Task<IActionResult> CreateIceLevel([FromBody] IceLevelCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                return Ok(await _iceService.CreateIceLevelAsync(dto));
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIceLevel(short id, [FromBody] IceLevelCreateDto dto)
        {
            var result = await _iceService.UpdateIceLevelAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIceLevel(short id)
        {
            if (!await _iceService.DeleteIceLevelAsync(id)) return NotFound();
            return NoContent();
        }

        // GET: api/Sizes/5/usage
        [HttpGet("{id}/usage")]
        public async Task<IActionResult> GetSizeUsage(short id)
        {
            var count = await _iceService.CountProductsUsingIceLevelAsync(id);
            return Ok(new { count }); // Trả về JSON: { "count": 5 }
        }
    }
}