// Interfaces/IReviewRepository.cs
using drinking_be.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        // Lấy tất cả đánh giá đã được duyệt của một sản phẩm
        Task<IEnumerable<Review>> GetApprovedReviewsByProductIdAsync(int productId);

        // Kiểm tra xem User đã từng review sản phẩm này chưa
        Task<bool> HasUserReviewedProductAsync(int userId, int productId);
    }
}