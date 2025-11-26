// Repositories/MembershipRepository.cs
using drinking_be.Interfaces;
using drinking_be.Interfaces.MembershipInterfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace drinking_be.Repositories
{
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {
        private readonly DBDrinkContext _context;

        public MembershipRepository(DBDrinkContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Membership?> GetByUserIdWithLevelAsync(int userId)
        {
            // Eager Loading Level và User để AutoMapper có thể lấy LevelName và UserName
            return await _context.Memberships
                                 .Include(m => m.Level)
                                 .Include(m => m.User)
                                 .FirstOrDefaultAsync(m => m.UserId == userId);
        }
    }
}