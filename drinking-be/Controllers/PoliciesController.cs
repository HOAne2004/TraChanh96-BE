// Controllers/PolicyController.cs
using drinking_be.Dtos.PolicyDtos;
using drinking_be.Interfaces.PolicyInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace drinking_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _policyService;

        public PolicyController(IPolicyService policyService)
        {
            _policyService = policyService;
        }

        // --- PUBLIC ENDPOINTS (Khách hàng) ---

        /// <summary>
        /// Lấy danh sách tất cả các chính sách đang hoạt động.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PolicyReadDto>))]
        public async Task<IActionResult> GetActivePolicies()
        {
            var policies = await _policyService.GetActivePoliciesAsync();
            return Ok(policies);
        }

        /// <summary>
        /// Lấy chi tiết một chính sách theo Slug.
        /// </summary>
        [HttpGet("{slug}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PolicyReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPolicyBySlug(string slug)
        {
            var policy = await _policyService.GetPolicyBySlugAsync(slug);
            if (policy == null)
            {
                return NotFound("Không tìm thấy chính sách này.");
            }
            return Ok(policy);
        }

        // --- ADMIN ENDPOINT ---

        /// <summary>
        /// [ADMIN] Tạo chính sách mới.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PolicyReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePolicy([FromBody] PolicyCreateDto policyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdPolicy = await _policyService.CreatePolicyAsync(policyDto);

                return CreatedAtAction(nameof(GetPolicyBySlug),
                                       new { slug = createdPolicy.Slug },
                                       createdPolicy);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
    }
}