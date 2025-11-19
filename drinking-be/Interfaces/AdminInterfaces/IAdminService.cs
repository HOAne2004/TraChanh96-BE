// Interfaces/IAdminService.cs
using drinking_be.Dtos.UserDtos;

namespace drinking_be.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<UserReadDto>> GetAllUsersAsync();
        // (Chúng ta có thể thêm các hàm Update/Delete sau)
    }
}