// Controllers/CategoriesController.cs
using drinking_be.Dtos.CategoryDtos;
using drinking_be.Interfaces.CategoryInerfaces;
using Microsoft.AspNetCore.Mvc;

namespace drinking_be.Controllers
{
    // Frontend gọi GET /api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // ⭐️ GET: api/categories (Lấy danh sách phẳng)
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryReadDto>))]
        public async Task<IActionResult> GetCategories([FromQuery] string? q)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(q);
            return Ok(categories);
        }

        // ⭐️ BỔ SUNG: GET: api/categories/5 (Lấy theo ID)
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Không tìm thấy danh mục với ID: {id}");
            }
            return Ok(category);
        }

        // GET: api/categories/tree (Lấy cấu trúc cây cho admin/frontend menu)
        [HttpGet("tree")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryReadDto>))]
        public async Task<IActionResult> GetCategoryTree()
        {
            var categories = await _categoryService.GetCategoryTreeAsync();
            return Ok(categories);
        }

        // POST: api/categories (Tạo mới)
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CategoryReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto categoryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var newCategory = await _categoryService.CreateCategoryAsync(categoryDto);
                // Trả về 201 Created
                return CreatedAtAction(nameof(GetCategoryById), new { id = newCategory.Id }, newCategory);
            }
            catch (Exception ex)
            {
                // Bắt lỗi nghiệp vụ (ví dụ: Slug trùng lặp)
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/categories/5 (Cập nhật)
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto categoryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var updatedCategory = await _categoryService.UpdateCategoryAsync(id, categoryDto);
                if (updatedCategory == null) return NotFound($"Không tìm thấy danh mục với ID: {id}");

                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                // Bắt lỗi nghiệp vụ (ví dụ: Slug trùng lặp)
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/categories/5 (Xóa)
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result) return NotFound($"Không tìm thấy danh mục với ID: {id}");

            return NoContent(); // 204 No Content
        }

        // GET: api/Categories/5/usage
        [HttpGet("{id}/usage")]
        public async Task<IActionResult> GetCategoryUsage(int id)
        {
            var count = await _categoryService.CountProductsInCategoryAsync(id);
            return Ok(new { count });
        }
    }
}