// Interfaces/IAuthService.cs
using drinking_be.Dtos.UserDtos;
using System.Threading.Tasks;

namespace drinking_be.Interfaces.UserInterfaces
{
    public interface IUserService
    {
        Task<UserReadDto> RegisterAsync(UserRegisterDto registerDto);
        Task<string> LoginAsync(UserLoginDto loginDto); // Trả về JWT Token (Giả định)
        Task<UserReadDto?> GetUserByPublicIdAsync(Guid publicId);

        Task<UserReadDto?> UpdateUserByPublicIdAsync(Guid publicId, UserUpdateDto updateDto);
        Task<bool> DeleteUserByPublicIdAsync(Guid publicId);
    }
}