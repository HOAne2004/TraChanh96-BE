// Services/CategoryService.cs
using AutoMapper;
using drinking_be.Dtos.CategoryDtos;
using drinking_be.Interfaces.CategoryInerfaces;
using drinking_be.Models;
using drinking_be.Repositories;
using drinking_be.Utils; // Giả định có SlugGenerator (hoặc dùng hàm tiện ích)
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drinking_be.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
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
            var categories = await _categoryRepo.GetAllCategoriesAsync();
            var categoryDtos = _mapper.Map<List<CategoryReadDto>>(categories);

            // Bắt đầu xây dựng cây từ các nút gốc (ParentId == null)
            return BuildCategoryTree(categoryDtos);
        }

        // ⭐️ Lấy tất cả (phẳng)
        public async Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepo.GetAllCategoriesAsync();
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

            // TODO: Cần thêm logic kiểm tra xem Category này có children hoặc products không
            // Nếu có, cần chặn xóa hoặc chuyển Product/Children sang ParentId khác.

            // Thực hiện xóa
            _categoryRepo.Delete(category);
            await _categoryRepo.SaveChangesAsync();
            return true;
        }
    }
}
