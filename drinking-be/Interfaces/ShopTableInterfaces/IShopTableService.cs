using drinking_be.Dtos.ShopTableDtos;

namespace drinking_be.Interfaces.ShopTableInterfaces
{
    public interface IShopTableService
    {
        Task<IEnumerable<ShopTableReadDto>> GetTablesByStoreAsync(int storeId);
        Task<ShopTableReadDto?> GetTableByIdAsync(int id);
        Task<ShopTableReadDto> CreateTableAsync(ShopTableCreateDto createDto);
        Task<bool> UpdateTableAsync(int id, ShopTableUpdateDto updateDto);
        Task<bool> DeleteTableAsync(int id); // Xóa mềm (IsActive = false) hoặc xóa cứng
    }
}
