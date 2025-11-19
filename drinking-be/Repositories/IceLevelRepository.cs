using drinking_be.Models;
using Microsoft.EntityFrameworkCore;
using drinking_be.Interfaces.OptionInterfaces;

namespace drinking_be.Repositories
{
    public class IceLevelRepository :GenericRepository<IceLevel>, IIceLevelRepository
    {
        private DBDrinkContext _context;
        public IceLevelRepository(DBDrinkContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<IceLevel>> GetIceLevelsByIdsAsync(List<short> iceLevelIds)
        {
            return await _context.IceLevels
                                      .Where(i => iceLevelIds.Contains(i.Id))
                                      .ToListAsync();
        }
    }
}
