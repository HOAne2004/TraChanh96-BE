// Controllers/VoucherTemplateController.cs (CẬP NHẬT)
using drinking_be.Dtos.VoucherDtos;
using drinking_be.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace drinking_be.Controllers
{
    [Route("api/admin/[controller]")] // Đặt trong /admin/
    [ApiController]
    // [Authorize(Roles = "Admin")] // Cần có xác thực Admin
    public class VoucherTemplateController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VoucherTemplateController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        /// <summary>
        /// [ADMIN] Lấy tất cả các mẫu voucher đã tạo.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VoucherTemplateReadDto>))]
        public async Task<IActionResult> GetAllTemplates()
        {
            var templates = await _voucherService.GetAllTemplatesAsync();
            return Ok(templates);
        }

        /// <summary>
        /// [ADMIN] Lấy chi tiết mẫu voucher theo ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VoucherTemplateReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTemplateById(int id)
        {
            try
            {
                var template = await _voucherService.GetTemplateByIdAsync(id);
                return Ok(template);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// [ADMIN] Tạo một mẫu voucher mới.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VoucherTemplateReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTemplate([FromBody] VoucherTemplateCreateDto templateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdTemplate = await _voucherService.CreateTemplateAsync(templateDto);
                return CreatedAtAction(nameof(GetTemplateById), new { id = createdTemplate.Id }, createdTemplate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Ví dụ: Ngày không hợp lệ
            }
        }

        // ⭐️ ENDPOINT MỚI ĐƯỢC BỔ SUNG ⭐️

        /// <summary>
        /// [ADMIN] Phát hành (Issue) một voucher từ template cho User.
        /// </summary>
        [HttpPost("issue")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserVoucherReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> IssueVoucherToUser([FromBody] VoucherIssueDto issueDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var issuedVoucher = await _voucherService.IssueVoucherAsync(issueDto);

                // Trả về 201 Created (không có route cụ thể để lấy UserVoucher)
                return StatusCode(StatusCodes.Status201Created, issuedVoucher);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // User hoặc Template không tồn tại
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Lỗi khác
            }
        }
    }
}