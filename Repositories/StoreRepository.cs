// Repositories/StoreRepository.cs
using drinking_be.Interfaces.StoreInterfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drinking_be.Repositories
{
    public class StoreRepository : GenericRepository<Store>, IStoreRepository
    {
        private readonly DBDrinkContext _context;

        public StoreRepository(DBDrinkContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Store>> GetActiveStoresAsync()
        {
            // Eager Loading Brand để lấy BrandName cho DTO
            return await _context.Stores
                                 .Where(s => s.IsActive == true)
                                 .Include(s => s.Brand)
                                 .OrderBy(s => s.Name)
                                 .ToListAsync();
        }

        public async Task<Store?> GetBySlugAsync(string slug)
        {
            // Eager Loading Brand để lấy BrandName cho DTO
            return await _context.Stores
                                 .Include(s => s.Brand)
                                 .FirstOrDefaultAsync(s => s.Slug == slug);
        }
    }
}