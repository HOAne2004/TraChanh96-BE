using drinking_be.Models;
using Microsoft.EntityFrameworkCore;
using drinking_be.Interfaces.OptionInterfaces;

namespace drinking_be.Repositories
{
    public class SugarLevelRepository : GenericRepository<SugarLevel>, ISugarLevelRepository
    {
        private DBDrinkContext _context;

        public SugarLevelRepository(DBDrinkContext context) :base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SugarLevel>> GetSugarLevelsByIdsAsync(List<short> sugarLevelIds)
        {
            return await _context.SugarLevels
                                      .Where(s => sugarLevelIds.Contains(s.Id))
                                      .ToListAsync();
        }
    }
}
