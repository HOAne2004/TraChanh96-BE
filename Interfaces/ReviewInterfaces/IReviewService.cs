// Interfaces/IReviewService.cs
using drinking_be.Dtos.ReviewDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces
{
    public interface IReviewService
    {
        // Public API
        Task<IEnumerable<ReviewReadDto>> GetApprovedReviewsAsync(int productId);

        // User API (Cần xác thực)
        Task<ReviewReadDto> CreateReviewAsync(ReviewCreateDto reviewDto, int userId);

        // Admin API (TODO)
        // Task<ReviewReadDto> ApproveReviewAsync(int reviewId);
        // Task DeleteReviewAsync(int reviewId);
    }
}