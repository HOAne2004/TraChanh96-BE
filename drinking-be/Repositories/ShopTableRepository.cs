using drinking_be.Interfaces.ShopTableInterfaces;
using drinking_be.Models;
using drinking_be.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DBDrink.Repositories
{
    public class ShopTableRepository : GenericRepository<ShopTable>, IShopTableRepository
    {
        public ShopTableRepository(DBDrinkContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ShopTable>> GetTablesByStoreIdAsync(int storeId)
        {
            // Include Store để lấy tên quán map vào DTO nếu cần
            return await _context.ShopTables
                .Include(t => t.Store)
                .Where(t => t.StoreId == storeId && t.IsActive == true) // Chỉ lấy bàn đang hoạt động
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<bool> IsTableNameExistsAsync(int storeId, string name)
        {
            return await _context.ShopTables
                .AnyAsync(t => t.StoreId == storeId && t.Name == name);
        }
    }
}