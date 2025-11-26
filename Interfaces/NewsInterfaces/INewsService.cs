using drinking_be.Dtos.NewsDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces.NewsInterfaces
{
    public interface INewsService
    {
        // Public API
        Task<IEnumerable<NewsReadDto>> GetPublishedNewsAsync();
        Task<NewsReadDto?> GetNewsBySlugAsync(string slug);

        // Admin API
        Task<NewsReadDto> CreateNewsAsync(NewsCreateDto newsDto);
        // ... (Update, Delete)
    }
}