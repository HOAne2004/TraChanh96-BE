// Interfaces/ISizeService.cs (TẠO MỚI)
using drinking_be.Dtos.OptionDtos;

namespace drinking_be.Interfaces.OptionInterfaces
{

    public interface ISizeService
    {
        Task<IEnumerable<SizeReadDto>> GetAllSizesAsync();
        Task<SizeReadDto> CreateSizeAsync(SizeCreateDto sizeDto);
        Task<SizeReadDto?> UpdateSizeAsync(short id, SizeCreateDto sizeDto);
        Task<bool> DeleteSizeAsync(short id);
        Task<int> CountProductsUsingSizeAsync(short id);
    }
}