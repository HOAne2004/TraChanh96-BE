// Repositories/UserAddressRepository.cs
using drinking_be.Interfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drinking_be.Repositories
{
    public class UserAddressRepository : GenericRepository<UserAddress>, IUserAddressRepository
    {
        private readonly new DBDrinkContext _context;

        public UserAddressRepository(DBDrinkContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserAddress>> GetAddressesByUserIdAsync(int userId)
        {
            return await _context.UserAddresses
                                 .Where(ua => ua.UserId == userId)
                                 .OrderByDescending(ua => ua.IsDefault) // Địa chỉ mặc định lên đầu
                                 .ThenByDescending(ua => ua.CreatedAt)
                                 .ToListAsync();
        }

        public async Task<int> CountAddressesByUserIdAsync(int userId)
        {
            return await _context.UserAddresses.CountAsync(ua => ua.UserId == userId);
        }

        public async Task<UserAddress?> FindDefaultAddressAsync(int userId)
        {
            return await _context.UserAddresses.FirstOrDefaultAsync(ua => ua.UserId == userId && ua.IsDefault == true);
        }
    }
}