// Interfaces/IIceLevelService.cs
using drinking_be.Dtos.OptionDtos;

namespace drinking_be.Interfaces
{
    public interface IIceLevelService
    {
        Task<IEnumerable<IceLevelDto>> GetAllIceLevelsAsync();
    }
}