// Interfaces/IUserRepository.cs
using drinking_be.Models;
using System.Threading.Tasks;

namespace drinking_be.Interfaces.UserInterfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email); // Tìm kiếm bằng Username hoặc Email
        Task<bool> IsEmailTakenAsync(string email);
        Task UpdateLastLoginAsync(User user);
    }
}