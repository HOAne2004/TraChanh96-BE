// Interfaces/ISugarLevelService.cs
using drinking_be.Dtos.OptionDtos;

namespace drinking_be.Interfaces
{
    public interface ISugarLevelService
    {
        Task<IEnumerable<SugarLevelDto>> GetAllSugarLevelsAsync();
    }
}