using drinking_be.Dtos.OptionDtos;
using drinking_be.Interfaces.OptionInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace drinking_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            return Ok(await _sizeService.GetAllSizesAsync());
        }

        // ⭐️ API CHECK USAGE (Để sửa lỗi 404)
        [HttpGet("{id}/usage")]
        public async Task<IActionResult> GetSizeUsage(short id)
        {
            var count = await _sizeService.CountProductsUsingSizeAsync(id);
            return Ok(new { count });
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
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
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
            try
            {
                var result = await _sizeService.DeleteSizeAsync(id);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (Exception ex) // Bắt lỗi FK constraint
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}