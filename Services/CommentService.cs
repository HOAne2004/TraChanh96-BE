// Services/CommentService.cs
using AutoMapper;
using drinking_be.Dtos.CommentDtos;
using drinking_be.Interfaces;
using drinking_be.Interfaces.NewsInterfaces;
using drinking_be.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drinking_be.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepo;
        private readonly INewsRepository _newsRepo; // Cần kiểm tra News tồn tại
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepo, INewsRepository newsRepo, IMapper mapper)
        {
            _commentRepo = commentRepo;
            _newsRepo = newsRepo;
            _mapper = mapper;
        }

        // --- PUBLIC API ---

        public async Task<IEnumerable<CommentReadDto>> GetCommentsByNewsIdAsync(int newsId)
        {
            var comments = await _commentRepo.GetCommentsByNewsIdAsync(newsId);
            return _mapper.Map<IEnumerable<CommentReadDto>>(comments);
        }

        // --- USER API ---

        public async Task<CommentReadDto> CreateCommentAsync(CommentCreateDto commentDto, int userId)
        {
            // 1. Kiểm tra News có tồn tại không
            var news = await _newsRepo.GetByIdAsync(commentDto.NewsId);
            if (news == null)
            {
                throw new KeyNotFoundException("Bài viết không tồn tại.");
            }

            // 2. (Tùy chọn) Kiểm tra ParentId (bình luận cha) có tồn tại không
            if (commentDto.ParentId.HasValue)
            {
                var parentComment = await _commentRepo.GetByIdAsync(commentDto.ParentId.Value);
                if (parentComment == null || parentComment.NewsId != commentDto.NewsId)
                {
                    throw new Exception("Bình luận cha không hợp lệ.");
                }
            }

            // 3. Ánh xạ DTO sang Entity
            var comment = _mapper.Map<Comment>(commentDto);

            // 4. Gán các giá trị hệ thống
            comment.UserId = userId;
            comment.CreatedAt = DateTime.UtcNow;

            // 5. Lưu vào DB
            await _commentRepo.AddAsync(comment);
            await _commentRepo.SaveChangesAsync();

            // 6. Trả về DTO
            // Cần Eager Load User để hiển thị UserName ngay lập tức
            var createdComment = await _commentRepo.GetByIdAsync(comment.Id); // Tải lại để lấy User (nếu cần)
                                                                              // Hoặc gán thủ công nếu đã có thông tin User

            return _mapper.Map<CommentReadDto>(comment);
        }
    }
}