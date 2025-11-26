// Repositories/UserVoucherRepository.cs
using drinking_be.Interfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drinking_be.Repositories
{
    public class UserVoucherRepository : GenericRepository<UserVoucher>, IUserVoucherRepository
    {
        private readonly DBDrinkContext _context;

        public UserVoucherRepository(DBDrinkContext context) : base(context)
        {
            _context = context;
        }

        public async Task<UserVoucher?> GetValidVoucherByCodeAsync(string code)
        {
            var now = DateTime.UtcNow;

            return await _context.UserVouchers
                                 .Include(uv => uv.VoucherTemplate) // Eager loading Template
                                 .FirstOrDefaultAsync(uv => uv.VoucherCode == code &&
                                                           uv.Status == 1 && // 1: Unused
                                                           uv.ExpiryDate > now);
        }

        public async Task<IEnumerable<UserVoucher>> GetValidVouchersByUserIdAsync(int userId)
        {
            var now = DateTime.UtcNow;

            return await _context.UserVouchers
                                 .Include(uv => uv.VoucherTemplate) // Eager loading Template
                                 .Where(uv => uv.UserId == userId &&
                                               uv.Status == 1 && // 1: Unused
                                               uv.ExpiryDate > now)
                                 .OrderBy(uv => uv.ExpiryDate) // Ưu tiên voucher sắp hết hạn
                                 .ToListAsync();
        }
    }
}