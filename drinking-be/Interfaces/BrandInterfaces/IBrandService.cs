// Interfaces/IBrandService.cs
using drinking_be.Dtos.BrandDtos;
using System.Threading.Tasks;

namespace drinking_be.Interfaces
{
    public interface IBrandService
    {
        Task<BrandReadDto?> GetPrimaryBrandInfoAsync();
        // Phương thức cho Admin tạo/cập nhật
        Task<BrandReadDto> CreateOrUpdateBrandAsync(BrandCreateDto brandDto);
    }
}