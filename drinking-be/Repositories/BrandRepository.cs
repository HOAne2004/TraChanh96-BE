// Repositories/BrandRepository.cs
using drinking_be.Interfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace drinking_be.Repositories
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        private readonly DBDrinkContext _context;

        public BrandRepository(DBDrinkContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Brand?> GetPrimaryBrandAsync()
        {
            // Lấy Brand có ID thấp nhất, giả định là Brand chính
            return await _context.Brands
                .Include(b => b.SocialMedia)
                                 .OrderBy(b => b.Id)
                                 .FirstOrDefaultAsync();
        }
    }
}