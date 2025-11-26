// Interfaces/CategoryInterfaces/ICategoryRepository.cs
using drinking_be.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces.CategoryInerfaces
{
    // Kế thừa từ IGenericRepository<Category>
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        // ⭐️ Phương thức chuyên biệt: Lấy tất cả danh mục (có thể không cần eager loading)
        Task<IEnumerable<Category>> GetAllCategoriesAsync(string? searchQuery);

        // Kiểm tra Slug đã tồn tại chưa
        Task<bool> IsSlugExistsAsync(string slug, int? excludeId = null);
    }
}