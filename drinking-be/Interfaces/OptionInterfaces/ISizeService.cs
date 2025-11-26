// Interfaces/ISizeService.cs (TẠO MỚI)
using drinking_be.Dtos.ProductDtos; // Tái sử dụng DTO nếu có, hoặc tạo DTO riêng
using drinking_be.Models;
using drinking_be.Dtos.OptionDtos;

namespace drinking_be.Interfaces.OptionInterfaces
{

    public interface ISizeService
    {
        Task<IEnumerable<SizeDto>> GetAllSizesAsync();
        Task<SizeDto> CreateSizeAsync(SizeCreateDto sizeDto);
        Task<SizeDto?> UpdateSizeAsync(short id, SizeCreateDto sizeDto);
        Task<bool> DeleteSizeAsync(short id);
        Task<int> CountProductsUsingSizeAsync(short id);
    }
}