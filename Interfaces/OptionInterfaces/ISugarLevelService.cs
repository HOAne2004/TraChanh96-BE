// Interfaces/ISugarLevelService.cs
using drinking_be.Dtos.OptionDtos;

namespace drinking_be.Interfaces
{
    public interface ISugarLevelService
    {
        Task<IEnumerable<SugarLevelDto>> GetAllSugarLevelsAsync();
        Task<SugarLevelDto> CreateSugarLevelAsync(SugarLevelCreateDto sugarLevelDto);
        Task<SugarLevelDto?> UpdateSugarLevelAsync(short id, SugarLevelCreateDto sugarLevelDto);
        Task<bool> DeleteSugarLevelAsync(short id);
        Task<int> CountProductsUsingSugarLevelAsync(short id);
    }
}