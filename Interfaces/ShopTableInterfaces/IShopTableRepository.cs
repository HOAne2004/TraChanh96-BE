using drinking_be.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace drinking_be.Interfaces.ShopTableInterfaces
{
    public interface IShopTableRepository : IGenericRepository<ShopTable>
    {
        // Lấy danh sách bàn theo StoreId
        Task<IEnumerable<ShopTable>> GetTablesByStoreIdAsync(int storeId);

        // Kiểm tra xem tên bàn đã tồn tại trong quán chưa (tránh trùng tên bàn trong cùng 1 quán)
        Task<bool> IsTableNameExistsAsync(int storeId, string name);
    }
}
