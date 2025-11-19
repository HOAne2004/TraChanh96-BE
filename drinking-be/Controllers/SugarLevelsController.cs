// Controllers/SugarLevelsController.cs
using drinking_be.Interfaces;
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
            var levels = await _sugarService.GetAllSugarLevelsAsync();
            return Ok(levels);
        }
    }
}