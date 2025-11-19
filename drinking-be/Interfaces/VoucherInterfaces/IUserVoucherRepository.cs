// Interfaces/IUserVoucherRepository.cs
using drinking_be.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces
{
    public interface IUserVoucherRepository : IGenericRepository<UserVoucher>
    {
        // Lấy voucher (chưa sử dụng, chưa hết hạn) bằng Code
        Task<UserVoucher?> GetValidVoucherByCodeAsync(string code);

        // Lấy tất cả voucher hợp lệ của User
        Task<IEnumerable<UserVoucher>> GetValidVouchersByUserIdAsync(int userId);
    }
}