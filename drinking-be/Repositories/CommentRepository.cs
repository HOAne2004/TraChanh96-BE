// Repositories/CommentRepository.cs
using drinking_be.Interfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drinking_be.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly DBDrinkContext _context;

        public CommentRepository(DBDrinkContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByNewsIdAsync(int newsId)
        {
            // Lấy tất cả bình luận của bài viết, kèm thông tin User
            return await _context.Comments
                                 .Include(c => c.User) // Eager Loading User
                                 .Where(c => c.NewsId == newsId)
                                 .OrderBy(c => c.CreatedAt) // Sắp xếp cũ nhất trước
                                 .ToListAsync();
        }
    }
}