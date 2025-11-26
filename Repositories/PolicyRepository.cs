// Repositories/PolicyRepository.cs
using drinking_be.Interfaces.PolicyInterfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drinking_be.Repositories
{
    public class PolicyRepository : GenericRepository<Policy>, IPolicyRepository
    {
        private readonly DBDrinkContext _context;

        public PolicyRepository(DBDrinkContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Policy>> GetActivePoliciesAsync()
        {
            // Lấy tất cả các chính sách đang hoạt động
            return await _context.Policies
                                 .Where(p => p.IsActive == true)
                                 .OrderBy(p => p.CreatedAt)
                                 .ToListAsync();
        }

        public async Task<Policy?> GetBySlugAsync(string slug)
        {
            // Lấy chi tiết chính sách theo Slug
            return await _context.Policies
                                 .FirstOrDefaultAsync(p => p.Slug == slug);
        }
    }
}