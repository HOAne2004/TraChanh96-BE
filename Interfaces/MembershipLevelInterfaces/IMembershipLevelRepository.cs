// Interfaces/IMembershipLevelRepository.cs
using drinking_be.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces
{
    public interface IMembershipLevelRepository : IGenericRepository<MembershipLevel>
    {
        // Lấy danh sách cấp độ (sắp xếp theo mức chi tiêu)
        Task<IEnumerable<MembershipLevel>> GetAllSortedAsync();
    }
}