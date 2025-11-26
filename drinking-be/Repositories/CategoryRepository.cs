// Repositories/CategoryRepository.cs
using drinking_be.Interfaces.CategoryInerfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly DBDrinkContext _context;

        public CategoryRepository(DBDrinkContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(string? searchQuery)
        {
            var query = _context.Categories.AsQueryable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                var lowerQuery = searchQuery.ToLower();
                query = query.Where(c => c.Name.ToLower().Contains(lowerQuery) || c.Slug.ToLower().Contains(lowerQuery));
            }
            // Eager Load children (cho việc build cây)
            // Lấy tất cả và sắp xếp theo ID (hoặc theo thứ tự tùy chỉnh)
            return await query.OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<bool> IsSlugExistsAsync(string slug, int? excludeId = null)
        {
            if (excludeId.HasValue)
            {
                return await _context.Categories
                                     .AnyAsync(c => c.Slug == slug && c.Id != excludeId.Value);
            }
            return await _context.Categories.AnyAsync(c => c.Slug == slug);
        }
    }
}