// Repositories/SizeRepository.cs
using drinking_be.Interfaces.OptionInterfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drinking_be.Repositories
{
    public class SizeRepository : GenericRepository<Size>, ISizeRepository
    {
        private readonly DBDrinkContext _context;

        public SizeRepository(DBDrinkContext context) : base(context)
        {
            _context = context;
        }

        // Triển khai phương thức cần thiết cho OrderService
        public async Task<IEnumerable<Size>> GetSizesByIdsAsync(List<short> sizeIds)
        {
            return await _context.Sizes
                                 .Where(s => sizeIds.Contains(s.Id))
                                 .ToListAsync();
        }
    }
}