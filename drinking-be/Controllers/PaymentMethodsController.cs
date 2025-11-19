// Controllers/PaymentMethodController.cs
using drinking_be.Dtos.PaymentMethodDtos;
using drinking_be.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace drinking_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly IPaymentMethodService _methodService;

        public PaymentMethodsController(IPaymentMethodService methodService)
        {
            _methodService = methodService;
        }

        // --- PUBLIC ENDPOINT (Khách hàng) ---

        /// <summary>
        /// Lấy danh sách các phương thức thanh toán đang hoạt động.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PaymentMethodReadDto>))]
        public async Task<IActionResult> GetActiveMethods()
        {
            var methods = await _methodService.GetActiveMethodsAsync();
            return Ok(methods);
        }

        // --- ADMIN ENDPOINT ---

        /// <summary>
        /// [ADMIN] Tạo phương thức thanh toán mới.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PaymentMethodReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePaymentMethod([FromBody] PaymentMethodCreateDto methodDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdMethod = await _methodService.CreatePaymentMethodAsync(methodDto);

                // Trả về 201 Created (có thể dùng GetById nếu có)
                return StatusCode(StatusCodes.Status201Created, createdMethod);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
    }
}