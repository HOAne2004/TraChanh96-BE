// Services/ReviewService.cs
using AutoMapper;
using drinking_be.Dtos.ReviewDtos;
using drinking_be.Interfaces;
using drinking_be.Interfaces.ProductInterfaces;
using drinking_be.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drinking_be.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepo;
        // Giả định cần kiểm tra Product có tồn tại không
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepo, IProductRepository productRepo, IMapper mapper)
        {
            _reviewRepo = reviewRepo;
            _productRepo = productRepo;
            _mapper = mapper;
        }

        // --- PUBLIC API ---

        public async Task<IEnumerable<ReviewReadDto>> GetApprovedReviewsAsync(int productId)
        {
            var reviews = await _reviewRepo.GetApprovedReviewsByProductIdAsync(productId);
            return _mapper.Map<IEnumerable<ReviewReadDto>>(reviews);
        }

        // --- USER API ---

        public async Task<ReviewReadDto> CreateReviewAsync(ReviewCreateDto reviewDto, int userId)
        {
            // 1. Kiểm tra Product có tồn tại không
            var product = await _productRepo.GetByIdAsync(reviewDto.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException("Sản phẩm không tồn tại.");
            }

            // 2. Kiểm tra User đã review sản phẩm này chưa
            if (await _reviewRepo.HasUserReviewedProductAsync(userId, reviewDto.ProductId))
            {
                throw new Exception("Bạn đã đánh giá sản phẩm này rồi.");
            }

            // 3. TODO: Kiểm tra logic nghiệp vụ (ví dụ: User phải mua hàng mới được review)

            // 4. Ánh xạ DTO sang Entity
            var review = _mapper.Map<Review>(reviewDto);

            // 5. Gán các giá trị hệ thống
            review.UserId = userId;
            review.Status = "Pending"; // Mặc định chờ duyệt
            review.CreatedAt = DateTime.UtcNow;

            // 6. Lưu vào DB
            await _reviewRepo.AddAsync(review);
            await _reviewRepo.SaveChangesAsync();

            // 7. Trả về DTO (Lưu ý: Cần Eager Load User nếu muốn hiển thị UserName ngay)
            // Vì repository AddAsync không trả về User, ta gán thủ công nếu cần
            // Hoặc đơn giản là trả về DTO đã ánh xạ (không có UserName)

            return _mapper.Map<ReviewReadDto>(review);
        }
    }
}