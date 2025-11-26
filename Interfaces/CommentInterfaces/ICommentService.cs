// Interfaces/ICommentService.cs
using drinking_be.Dtos.CommentDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces
{
    public interface ICommentService
    {
        // Public API
        Task<IEnumerable<CommentReadDto>> GetCommentsByNewsIdAsync(int newsId);

        // User API (Cần xác thực)
        Task<CommentReadDto> CreateCommentAsync(CommentCreateDto commentDto, int userId);
    }
}