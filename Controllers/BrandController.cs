// Controllers/BrandController.cs
using drinking_be.Dtos.BrandDtos;
using drinking_be.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace drinking_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        // --- CÁC API MỚI KHỚP VỚI FRONTEND ---

        /// <summary>
        /// API phục vụ Footer (Frontend gọi: GET /api/footerInfo)
        /// </summary>
        [HttpGet("/api/footerInfo")] // ⭐️ Route tuyệt đối để ghi đè route controller
        public async Task<IActionResult> GetFooterInfo()
        {
            var brand = await _brandService.GetPrimaryBrandInfoAsync();
            if (brand == null) return NotFound();

            // Trả về toàn bộ BrandDto, Frontend sẽ tự lấy address, hotline, social...
            return Ok(brand);
        }

        /// <summary>
        /// API phục vụ cấu hình App (Logo, Tên...) (Frontend gọi: GET /api/appConfig)
        /// </summary>
        [HttpGet("/api/appConfig")] // ⭐️ Route tuyệt đối
        public async Task<IActionResult> GetAppConfig()
        {
            var brand = await _brandService.GetPrimaryBrandInfoAsync();
            if (brand == null) return NotFound();
            return Ok(brand);
        }

        // --- PUBLIC ENDPOINT (Thông tin chung) ---

        /// <summary>
        /// Lấy thông tin chi tiết về Brand/Thương hiệu chính.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPrimaryBrand()
        {
            var brand = await _brandService.GetPrimaryBrandInfoAsync();
            if (brand == null)
            {
                return NotFound("Thông tin thương hiệu chưa được thiết lập.");
            }
            return Ok(brand);
        }

        // --- ADMIN ENDPOINT ---

        /// <summary>
        /// [ADMIN] Tạo mới hoặc cập nhật thông tin Brand chính.
        /// </summary>
        [HttpPost]
        // Thường cần [Authorize(Roles = "Admin")] ở đây
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrUpdateBrand([FromBody] BrandCreateDto brandDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedBrand = await _brandService.CreateOrUpdateBrandAsync(brandDto);
                return Ok(updatedBrand);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
    }
}