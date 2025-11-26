// Services/NewsService.cs
using drinking_be.Dtos.NewsDtos;
using drinking_be.Interfaces;
using drinking_be.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using drinking_be.Interfaces.NewsInterfaces;

namespace drinking_be.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepo;
        private readonly IGenericRepository<User> _userRepo; // Cần kiểm tra User ID
        private readonly IMapper _mapper;

        public NewsService(INewsRepository newsRepo,
                           IGenericRepository<User> userRepo, // Giả định GenericRepository được dùng cho User
                           IMapper mapper)
        {
            _newsRepo = newsRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        // --- PUBLIC API ---

        public async Task<IEnumerable<NewsReadDto>> GetPublishedNewsAsync()
        {
            var news = await _newsRepo.GetPublishedNewsAsync();

            // Ánh xạ và trả về
            return _mapper.Map<IEnumerable<NewsReadDto>>(news);
        }

        public async Task<NewsReadDto?> GetNewsBySlugAsync(string slug)
        {
            var news = await _newsRepo.GetBySlugAsync(slug);
            if (news == null || news.Status != "Published")
            {
                return null;
            }

            return _mapper.Map<NewsReadDto>(news);
        }

        // --- ADMIN API ---

        public async Task<NewsReadDto> CreateNewsAsync(NewsCreateDto newsDto)
        {
            // 1. Xác thực User (Admin)
            var user = await _userRepo.GetByIdAsync(newsDto.UserId);
            if (user == null) // TODO: Thêm check Role (user.RoleId == AdminRole)
            {
                throw new Exception("Người dùng không hợp lệ.");
            }

            // 2. Ánh xạ DTO sang Entity
            var news = _mapper.Map<News>(newsDto);

            // Logic nghiệp vụ: Đặt PublishedDate nếu trạng thái là Published
            if (news.Status == "Published")
            {
                news.PublishedDate = DateTime.UtcNow;
            }

            // 3. Lưu vào DB
            await _newsRepo.AddAsync(news);
            await _newsRepo.SaveChangesAsync();

            // 4. Eager Load lại Category để AutoMapper có thể lấy CategoryName
            // Note: Cần viết thêm GetByIdWithCategory() trong Repository nếu muốn tối ưu
            // Tuy nhiên, ta có thể lấy lại bản ghi đã lưu và trả về (cho ví dụ này, ta bỏ qua logic load lại)

            // Giả định: news entity sau khi SaveChangesAsync() đã có ID

            var readDto = _mapper.Map<NewsReadDto>(news);

            // Cần gán lại Category Name thủ công nếu không viết hàm GetByIdWithCategory
            // Giả sử có Navigation Property Category trong News Entity
            // readDto.CategoryName = news.Category.Name; 

            return readDto;
        }

        // TODO: Cần triển khai thêm UpdateNewsAsync và DeleteNewsAsync
    }
}