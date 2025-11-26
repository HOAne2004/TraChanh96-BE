using drinking_be.Dtos.ShopTableDtos;
using drinking_be.Interfaces.ShopTableInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace drinking_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopTableController : ControllerBase
    {
        private readonly IShopTableService _shopTableService;

        public ShopTableController(IShopTableService shopTableService)
        {
            _shopTableService = shopTableService;
        }

        // GET: api/ShopTable/store/{storeId}
        [HttpGet("store/{storeId}")]
        public async Task<IActionResult> GetTablesByStore(int storeId)
        {
            var tables = await _shopTableService.GetTablesByStoreAsync(storeId);
            return Ok(tables);
        }

        // GET: api/ShopTable/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTableById(int id)
        {
            var table = await _shopTableService.GetTableByIdAsync(id);
            if (table == null)
            {
                return NotFound(new { message = "Không tìm thấy bàn." });
            }
            return Ok(table);
        }

        // POST: api/ShopTable
        [HttpPost]
        public async Task<IActionResult> CreateTable([FromBody] ShopTableCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdTable = await _shopTableService.CreateTableAsync(createDto);
                return CreatedAtAction(nameof(GetTableById), new { id = createdTable.Id }, createdTable);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/ShopTable/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTable(int id, [FromBody] ShopTableUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _shopTableService.UpdateTableAsync(id, updateDto);
            if (!result)
            {
                return NotFound(new { message = "Không tìm thấy bàn để cập nhật." });
            }

            return Ok(new { message = "Cập nhật bàn thành công." });
        }

        // DELETE: api/ShopTable/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTable(int id)
        {
            var result = await _shopTableService.DeleteTableAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Không tìm thấy bàn để xóa." });
            }

            return Ok(new { message = "Xóa bàn thành công." });
        }
    }
}