// Controllers/BrandController.cs
using drinking_be.Dtos.BrandDtos;
using drinking_be.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        private BrandReadDto GetDefaultBrandInfo()
        {
            return new BrandReadDto
            {
                Name = "Trà Chanh 96",
                LogoUrl = "/assets/logo-footer.png",
                Address = "Đang cập nhật địa chỉ...",
                Hotline = "1900 xxxx",
                EmailSupport = "support@trachanh96.vn",
                // ... các field khác
            };
        }

        // --- CÁC API MỚI KHỚP VỚI FRONTEND ---

        /// <summary>
        /// API phục vụ Footer (Frontend gọi: GET /api/footerInfo)
        /// </summary>
        [HttpGet("/api/footerInfo")]
        public async Task<IActionResult> GetFooterInfo()
        {
            var brand = await _brandService.GetPrimaryBrandInfoAsync();
            // Nếu null thì dùng hàm helper
            return Ok(brand ?? GetDefaultBrandInfo());
        }

        [HttpGet("/api/appConfig")]
        public async Task<IActionResult> GetAppConfig()
        {
            var brand = await _brandService.GetPrimaryBrandInfoAsync();
            return Ok(brand ?? GetDefaultBrandInfo());
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
        [Authorize(Roles = "Admin")]
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
                var updatedBrand = await _brandService.CreateBrandAsync(brandDto);
                return Ok(updatedBrand);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] BrandUpdateDto brandDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var updatedBrand = await _brandService.UpdateBrandAsync(id, brandDto);
                if (updatedBrand == null) return NotFound();
                return Ok(updatedBrand);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
