// Interfaces/IStoreService.cs
using drinking_be.Dtos.StoreDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces.StoreInterfaces
{
    public interface IStoreService
    {
        // Public API
        Task<IEnumerable<StoreReadDto>> GetActiveStoresAsync();
        Task<StoreReadDto?> GetStoreBySlugAsync(string slug);

        Task<StoreReadDto> CreateStoreAsync(StoreCreateDto storeDto);

        Task<StoreReadDto?> UpdateStoreAsync(long id, StoreCreateDto storeDto);
        Task<bool> DeleteStoreAsync(long id);
    }
}