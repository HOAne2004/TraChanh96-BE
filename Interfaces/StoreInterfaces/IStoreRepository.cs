// Interfaces/IStoreRepository.cs
using drinking_be.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces.StoreInterfaces
{
    public interface IStoreRepository : IGenericRepository<Store>
    {
        // Lấy tất cả cửa hàng đang hoạt động
        Task<IEnumerable<Store>> GetActiveStoresAsync();

        // Lấy chi tiết cửa hàng theo Slug
        Task<Store?> GetBySlugAsync(string slug);
    }
}