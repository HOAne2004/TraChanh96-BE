// Interfaces/IAdminService.cs
using drinking_be.Dtos.UserDtos;

namespace drinking_be.Interfaces
{
    public interface IAdminService
    {
        Task<bool> DeleteUserByPublicIdAsync(Guid publicId);
        Task<IEnumerable<UserReadDto>> GetAllUsersAsync();
        Task <UserReadDto> UpdateUserByPublicIdAsync(Guid publicId, UserUpdateDto updateDto);
    }
}