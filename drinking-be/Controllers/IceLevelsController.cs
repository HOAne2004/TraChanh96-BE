// Controllers/IceLevelsController.cs
using drinking_be.Interfaces;
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
            var levels = await _iceService.GetAllIceLevelsAsync();
            return Ok(levels);
        }
    }
}