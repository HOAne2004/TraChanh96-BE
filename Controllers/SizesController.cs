// Controllers/SizesController.cs (TẠO MỚI)
using drinking_be.Dtos.OptionDtos;
using drinking_be.Interfaces.OptionInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace drinking_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Đường dẫn: api/Sizes
    public class SizesController : ControllerBase
    {
        private readonly ISizeService _sizeService;

        public SizesController(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSizes()
        {
            var sizes = await _sizeService.GetAllSizesAsync();
            return Ok(sizes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSize([FromBody] SizeCreateDto sizeDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var newSize = await _sizeService.CreateSizeAsync(sizeDto);
                return CreatedAtAction(nameof(GetAllSizes), newSize);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSize(short id, [FromBody] SizeCreateDto sizeDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updatedSize = await _sizeService.UpdateSizeAsync(id, sizeDto);
            if (updatedSize == null) return NotFound();
            return Ok(updatedSize);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSize(short id)
        {
            var result = await _sizeService.DeleteSizeAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        // GET: api/Sizes/5/usage
        [HttpGet("{id}/usage")]
        public async Task<IActionResult> GetSizeUsage(short id)
        {
            var count = await _sizeService.CountProductsUsingSizeAsync(id);
            return Ok(new { count }); // Trả về JSON: { "count": 5 }
        }
    }
}