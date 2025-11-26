using drinking_be.Dtos.ShopTableDtos;

namespace drinking_be.Interfaces.ReviewInterfaces
{
    public interface IShopTableService
    {
        Task<IEnumerable<ShopTableReadDto>> GetAllTablesAsync();
        Task<ShopTableReadDto?> GetTableByIdAsync(int id);
        Task<IEnumerable<ShopTableReadDto>> GetTablesByStoreIdAsync(int storeId);
        Task<ShopTableReadDto> CreateTableAsync(ShopTableCreateDto createDto);
        Task<bool> UpdateTableAsync(int id, ShopTableUpdateDto updateDto);
        Task<bool> DeleteTableAsync(int id);
    }
}
