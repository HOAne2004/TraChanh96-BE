// Interfaces/ICommentRepository.cs
using drinking_be.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        // Lấy tất cả bình luận (kèm User) của một bài viết News
        Task<IEnumerable<Comment>> GetCommentsByNewsIdAsync(int newsId);
    }
}