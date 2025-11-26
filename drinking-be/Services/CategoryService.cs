// Services/CategoryService.cs
using AutoMapper;
using drinking_be.Dtos.CategoryDtos;
using drinking_be.Interfaces.CategoryInerfaces;
using drinking_be.Interfaces.ProductInterfaces;
using drinking_be.Models;
using drinking_be.Utils; // Giả định có SlugGenerator (hoặc dùng hàm tiện ích)
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;
        private readonly DBDrinkContext _context;

        public CategoryService(ICategoryRepository categoryRepo, IMapper mapper, DBDrinkContext context)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _context = context;
        }

        // --- LOGIC XỬ LÝ CÂY (RECURSIVE MAPPING) ---

        // Helper nội bộ để xây dựng cấu trúc cây từ danh sách phẳng
        private List<CategoryReadDto> BuildCategoryTree(List<CategoryReadDto> allCategories, int? parentId = null)
        {
            return allCategories
                .Where(c => c.ParentId == parentId)
                .OrderBy(c => c.Name) // Sắp xếp theo tên
                .Select(c =>
                {
                    c.Children = BuildCategoryTree(allCategories, c.Id);
                    return c;
                })
                .ToList();
        }

        // ⭐️ Lấy Categories dưới dạng cấu trúc cây
        public async Task<IEnumerable<CategoryReadDto>> GetCategoryTreeAsync()
        {
            var categories = await _categoryRepo.GetAllCategoriesAsync(null);
            var categoryDtos = _mapper.Map<List<CategoryReadDto>>(categories);

            // Bắt đầu xây dựng cây từ các nút gốc (ParentId == null)
            return BuildCategoryTree(categoryDtos);
        }

        // ⭐️ Lấy tất cả (phẳng)
        public async Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync(string? searchQuery)
        {
            var categories = await _categoryRepo.GetAllCategoriesAsync(searchQuery);
            return _mapper.Map<IEnumerable<CategoryReadDto>>(categories);
        }

        public async Task<CategoryReadDto?> GetCategoryByIdAsync(int id)
        {
            // 1. Lấy Entity từ Repository
            var category = await _categoryRepo.GetByIdAsync(id);

            if (category == null)
            {
                return null; // Trả về null nếu không tìm thấy
            }

            // 2. Ánh xạ (Map) Entity sang DTO và trả về
            return _mapper.Map<CategoryReadDto>(category);
        }

        // --- LOGIC CRUD ---

        public async Task<CategoryReadDto> CreateCategoryAsync(CategoryCreateDto categoryDto)
        {
            // 1. Tạo Slug
            string slug = string.IsNullOrEmpty(categoryDto.Slug)
                ? SlugGenerator.GenerateSlug(categoryDto.Name) // Giả định có SlugGenerator
                : categoryDto.Slug;

            // 2. Kiểm tra Slug trùng lặp
            if (await _categoryRepo.IsSlugExistsAsync(slug))
            {
                throw new Exception("Slug đã tồn tại. Vui lòng chọn tên/slug khác.");
            }

            // 3. Map và Lưu
            var category = _mapper.Map<Category>(categoryDto);
            category.Slug = slug;
            category.CreatedAt = DateTime.UtcNow;
            category.UpdatedAt = DateTime.UtcNow;

            await _categoryRepo.AddAsync(category);
            await _categoryRepo.SaveChangesAsync();

            // NOTE: Để trả về cây chính xác, tốt nhất là chạy lại GetCategoryTreeAsync()
            // Nhưng ta sẽ trả về DTO cơ bản
            return _mapper.Map<CategoryReadDto>(category);
        }

        public async Task<CategoryReadDto?> UpdateCategoryAsync(int id, CategoryCreateDto categoryDto)
        {
            var existingCategory = await _categoryRepo.GetByIdAsync(id);
            if (existingCategory == null) return null;

            // 1. Tạo Slug mới và kiểm tra trùng lặp (trừ chính nó)
            string newSlug = string.IsNullOrEmpty(categoryDto.Slug)
                ? SlugGenerator.GenerateSlug(categoryDto.Name)
                : categoryDto.Slug;

            if (newSlug != existingCategory.Slug && await _categoryRepo.IsSlugExistsAsync(newSlug, id))
            {
                throw new Exception("Slug đã tồn tại. Vui lòng chọn tên/slug khác.");
            }

            // 2. Map dữ liệu
            _mapper.Map(categoryDto, existingCategory);
            existingCategory.Slug = newSlug;
            existingCategory.UpdatedAt = DateTime.UtcNow;

            // 3. Lưu
            _categoryRepo.Update(existingCategory);
            await _categoryRepo.SaveChangesAsync();

            return _mapper.Map<CategoryReadDto>(existingCategory);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null) return false;

            // ⭐️ 1. KIỂM TRA SẢN PHẨM LIÊN QUAN ⭐️
            var productCount = await _productRepo.CountProductsInCategoryAsync(id);

            if (productCount > 0)
            {
                // ⭐️ Ném Exception với thông báo cụ thể cho Frontend
                throw new Exception($"Danh mục '{category.Name}' đang có {productCount} sản phẩm. Vui lòng xóa hết sản phẩm hoặc chuyển chúng sang danh mục khác trước khi xóa.");
            }

            // 2. Nếu không có sản phẩm, thực hiện xóa
            _categoryRepo.Delete(category);
            await _categoryRepo.SaveChangesAsync();
            return true;
        }
        public async Task<int> CountProductsInCategoryAsync(int id)
        {
            return await _context.Products.CountAsync(p => p.CategoryId == id);
        }
    }
}
