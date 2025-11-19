// Interfaces/INewsRepository.cs
using drinking_be.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces.NewsInterfaces
{
    public interface INewsRepository : IGenericRepository<News>
    {
        // Lấy tin tức đã xuất bản, kèm theo Category (Eager Loading)
        Task<IEnumerable<News>> GetPublishedNewsAsync();

        // Lấy tin tức theo Slug (thường dùng cho chi tiết bài viết)
        Task<News?> GetBySlugAsync(string slug);
    }
}