using drinking_be.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces
{
    public interface IUserAddressRepository : IGenericRepository<UserAddress>
    {
        // Lấy tất cả địa chỉ của một người dùng cụ thể
        Task<IEnumerable<UserAddress>> GetAddressesByUserIdAsync(int userId);

        // Đếm số lượng địa chỉ hiện tại (để thực thi giới hạn)
        Task<int> CountAddressesByUserIdAsync(int userId);

        // Tìm địa chỉ mặc định hiện tại
        Task<UserAddress?> FindDefaultAddressAsync(int userId);
    }
}