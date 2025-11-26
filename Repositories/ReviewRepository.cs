// Repositories/ReviewRepository.cs
using drinking_be.Interfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drinking_be.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly DBDrinkContext _context;

        public ReviewRepository(DBDrinkContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetApprovedReviewsByProductIdAsync(int productId)
        {
            // Lấy các review đã được duyệt ("Approved")
            return await _context.Reviews
                                 .Include(r => r.User) // Eager Loading User để lấy UserName
                                 .Where(r => r.ProductId == productId && r.Status == "Approved")
                                 .OrderByDescending(r => r.CreatedAt)
                                 .ToListAsync();
        }

        public async Task<bool> HasUserReviewedProductAsync(int userId, int productId)
        {
            // Kiểm tra xem đã tồn tại review của user này cho product này chưa
            return await _context.Reviews
                                 .AnyAsync(r => r.UserId == userId && r.ProductId == productId);
        }
    }
}