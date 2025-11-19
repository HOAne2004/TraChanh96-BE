// Controllers/SizesController.cs (TẠO MỚI)
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
            var sizes = await _sizeService.GetAllSizesAsync();
            return Ok(sizes);
        }
    }
}