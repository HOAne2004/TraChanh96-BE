// Interfaces/IMembershipLevelService.cs
using drinking_be.Dtos.MembershipLevelDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces
{
    public interface IMembershipLevelService
    {
        // Public API
        Task<IEnumerable<MembershipLevelReadDto>> GetAllLevelsAsync();

        // Admin API
        Task<MembershipLevelReadDto> GetLevelByIdAsync(byte id);
        Task<MembershipLevelReadDto> CreateLevelAsync(MembershipLevelCreateDto levelDto);
        Task<MembershipLevelReadDto> UpdateLevelAsync(byte id, MembershipLevelCreateDto levelDto);
    }
}