// Controllers/StoreController.cs
using drinking_be.Dtos.StoreDtos;
using drinking_be.Interfaces.StoreInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace drinking_be.Controllers
{
    [Route("api/stores")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        // --- PUBLIC ENDPOINTS (Khách hàng) ---

        /// <summary>
        /// Lấy danh sách tất cả cửa hàng đang hoạt động.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StoreReadDto>))]
        public async Task<IActionResult> GetActiveStores()
        {
            var stores = await _storeService.GetActiveStoresAsync();
            return Ok(stores);
        }

        /// <summary>
        /// Lấy chi tiết một cửa hàng theo Slug.
        /// </summary>
        [HttpGet("{slug}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StoreReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStoreBySlug(string slug)
        {
            var store = await _storeService.GetStoreBySlugAsync(slug);
            if (store == null)
            {
                return NotFound("Không tìm thấy cửa hàng này.");
            }
            return Ok(store);
        }

        // --- ADMIN ENDPOINT ---

        /// <summary>
        /// [ADMIN] Tạo cửa hàng mới.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StoreReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateStore([FromBody] StoreCreateDto storeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdStore = await _storeService.CreateStoreAsync(storeDto);

                return CreatedAtAction(nameof(GetStoreBySlug),
                                       new { slug = createdStore.Slug },
                                       createdStore);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        // ⭐️ THÊM MỚI: Cập nhật cửa hàng (PUT /api/stores/{id})
        [HttpPut("{id}")]
        // [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StoreReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStore(long id, [FromBody] StoreCreateDto storeDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedStore = await _storeService.UpdateStoreAsync(id, storeDto);

            if (updatedStore == null)
            {
                return NotFound($"Không tìm thấy cửa hàng với ID: {id}");
            }

            return Ok(updatedStore);
        }

        // ⭐️ THÊM MỚI: Xóa cửa hàng (DELETE /api/stores/{id})
        [HttpDelete("{id}")]
        // [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStore(long id)
        {
            var result = await _storeService.DeleteStoreAsync(id);

            if (!result)
            {
                return NotFound($"Không tìm thấy cửa hàng với ID: {id}");
            }

            return NoContent(); // 204 No Content thành công
        }
    }
}