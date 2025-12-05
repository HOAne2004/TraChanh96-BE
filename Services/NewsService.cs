using AutoMapper;
using drinking_be.Dtos.NewsDtos;
using drinking_be.Interfaces;
using drinking_be.Interfaces.NewsInterfaces;
using drinking_be.Models;
using drinking_be.Utils; // Để dùng SlugGenerator
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IMapper _mapper;

        public NewsService(INewsRepository newsRepo,
                           IGenericRepository<User> userRepo,
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
            return _mapper.Map<IEnumerable<NewsReadDto>>(news);
        }

        public async Task<NewsReadDto?> GetNewsBySlugAsync(string slug)
        {
            var news = await _newsRepo.GetBySlugAsync(slug);
            if (news == null || news.Status != "Published") return null;
            return _mapper.Map<NewsReadDto>(news);
        }

        public async Task<NewsReadDto?> GetNewsByIdAsync(int id)
        {
            var news = await _newsRepo.GetByIdAsync(id);
            if (news == null) return null;
            return _mapper.Map<NewsReadDto>(news);
        }

        // --- ADMIN API ---

        public async Task<NewsReadDto> CreateNewsAsync(NewsCreateDto newsDto)
        {
            // 1. Check User
            var user = await _userRepo.GetByIdAsync(newsDto.UserId);
            if (user == null) throw new Exception("Người dùng không tồn tại.");

            // 2. Map & Logic
            var news = _mapper.Map<News>(newsDto);

            // Tự động tạo Slug từ Title (dùng Utils nếu có, hoặc hàm private)
            news.Slug = SlugGenerator.GenerateSlug(news.Title);
            news.PublicId = Guid.NewGuid(); // Tạo UUID mới
            news.CreatedAt = DateTime.UtcNow;
            news.UpdatedAt = DateTime.UtcNow;

            if (news.Status == "Published")
            {
                news.PublishedDate = DateTime.UtcNow;
            }

            // 3. Save
            await _newsRepo.AddAsync(news);
            await _newsRepo.SaveChangesAsync();

            return _mapper.Map<NewsReadDto>(news);
        }

        public async Task<NewsReadDto?> UpdateNewsAsync(int id, NewsUpdateDto newsDto)
        {
            var existingNews = await _newsRepo.GetByIdAsync(id);
            if (existingNews == null) return null;

            // Map dữ liệu mới vào entity cũ
            _mapper.Map(newsDto, existingNews);

            // Cập nhật lại Slug nếu Title thay đổi (và Slug không được gửi lên)
            // if (!string.IsNullOrEmpty(newsDto.Title)) {
            //     existingNews.Slug = SlugGenerator.GenerateSlug(newsDto.Title);
            // }

            existingNews.UpdatedAt = DateTime.UtcNow;

            _newsRepo.Update(existingNews);
            await _newsRepo.SaveChangesAsync();

            return _mapper.Map<NewsReadDto>(existingNews);
        }

        public async Task<bool> DeleteNewsAsync(int id)
        {
            var existingNews = await _newsRepo.GetByIdAsync(id);
            if (existingNews == null) return false;

            _newsRepo.Delete(existingNews);
            await _newsRepo.SaveChangesAsync();
            return true;
        }
    }
}