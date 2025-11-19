// Repositories/MembershipLevelRepository.cs
using drinking_be.Interfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drinking_be.Repositories
{
    public class MembershipLevelRepository : GenericRepository<MembershipLevel>, IMembershipLevelRepository
    {
        private readonly DBDrinkContext _context;

        public MembershipLevelRepository(DBDrinkContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MembershipLevel>> GetAllSortedAsync()
        {
            // Lấy tất cả cấp độ, sắp xếp theo mức chi tiêu tối thiểu (thấp đến cao)
            return await _context.MembershipLevels
                                 .OrderBy(ml => ml.MinSpendRequired)
                                 .ToListAsync();
        }
    }
}