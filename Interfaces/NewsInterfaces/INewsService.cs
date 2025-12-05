using drinking_be.Dtos.NewsDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces.NewsInterfaces
{
    public interface INewsService
    {
        // --- Public API ---
        Task<IEnumerable<NewsReadDto>> GetPublishedNewsAsync();
        Task<NewsReadDto?> GetNewsBySlugAsync(string slug);
        Task<NewsReadDto?> GetNewsByIdAsync(int id); // Thêm hàm lấy theo ID cho Admin

        // --- Admin API ---
        // Sửa kiểu trả về thành NewsReadDto để đồng bộ
        Task<NewsReadDto> CreateNewsAsync(NewsCreateDto newsDto);

        // Sửa tham số thành NewsUpdateDto
        Task<NewsReadDto?> UpdateNewsAsync(int id, NewsUpdateDto newsDto);

        // Thống nhất ID là int
        Task<bool> DeleteNewsAsync(int id);
    }
}