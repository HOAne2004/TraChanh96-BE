// Interfaces/ISugarLevelService.cs
using drinking_be.Dtos.OptionDtos;

namespace drinking_be.Interfaces
{
    public interface ISugarLevelService
    {
        Task<IEnumerable<SugarLevelReadDto>> GetAllSugarLevelsAsync();
        Task<SugarLevelReadDto> CreateSugarLevelAsync(SugarLevelCreateDto sugarLevelDto);
        Task<SugarLevelReadDto?> UpdateSugarLevelAsync(short id, SugarLevelCreateDto sugarLevelDto);
        Task<bool> DeleteSugarLevelAsync(short id);
        Task<int> CountProductsUsingSugarLevelAsync(short id);
    }
}