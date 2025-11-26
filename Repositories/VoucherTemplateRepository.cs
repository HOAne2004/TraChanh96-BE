// Repositories/VoucherTemplateRepository.cs
using drinking_be.Interfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drinking_be.Repositories
{
    public class VoucherTemplateRepository : GenericRepository<VoucherTemplate>, IVoucherTemplateRepository
    {
        private readonly DBDrinkContext _context;

        public VoucherTemplateRepository(DBDrinkContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VoucherTemplate>> GetAllWithLevelAsync()
        {
            // Eager Loading MembershipLevel (Level) để lấy LevelName
            return await _context.VoucherTemplates
                                 .Include(vt => vt.Level) // Level là Navigation Property tới MembershipLevel
                                 .OrderByDescending(vt => vt.CreatedAt)
                                 .ToListAsync();
        }

        public async Task<VoucherTemplate?> GetByIdWithLevelAsync(int id)
        {
            return await _context.VoucherTemplates
                                 .Include(vt => vt.Level)
                                 .FirstOrDefaultAsync(vt => vt.Id == id);
        }
    }
}