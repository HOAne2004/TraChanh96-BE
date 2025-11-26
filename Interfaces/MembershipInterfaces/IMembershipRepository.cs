// Interfaces/IMembershipRepository.cs
using drinking_be.Models;
using System.Threading.Tasks;

namespace drinking_be.Interfaces.MembershipInterfaces
{
    public interface IMembershipRepository : IGenericRepository<Membership>
    {
        // Lấy thông tin thành viên (kèm Cấp độ) bằng User ID
        Task<Membership?> GetByUserIdWithLevelAsync(int userId);
    }
}