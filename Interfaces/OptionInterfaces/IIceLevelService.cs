// Interfaces/IIceLevelService.cs
using drinking_be.Dtos.OptionDtos;

namespace drinking_be.Interfaces
{
    public interface IIceLevelService
    {
        Task<IEnumerable<IceLevelDto>> GetAllIceLevelsAsync();
        Task<IceLevelDto> CreateIceLevelAsync(IceLevelCreateDto iceLevelDto);
        Task<IceLevelDto?> UpdateIceLevelAsync(short id, IceLevelCreateDto iceLevelDto);
        Task<bool> DeleteIceLevelAsync(short id);
        Task<int> CountProductsUsingIceLevelAsync(short id);
    }
}