// Repositories/NewsRepository.cs
using drinking_be.Interfaces.NewsInterfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drinking_be.Repositories
{
    public class NewsRepository : GenericRepository<News>, INewsRepository
    {
        private readonly new DBDrinkContext _context;

        public NewsRepository(DBDrinkContext context) : base(context)
        {
            _context = context;
        }

        // Lấy tin tức đã xuất bản, kèm theo Category
        public async Task<IEnumerable<News>> GetPublishedNewsAsync()
        {
            return await _context.News // Giả định DbSet là News
                                 .Where(n => n.Status == "Published")
                                 .OrderByDescending(n => n.PublishedDate)
                                 .ToListAsync();
        }

        // Lấy tin tức theo Slug
        public async Task<News?> GetBySlugAsync(string slug)
        {
            return await _context.News
                                 .FirstOrDefaultAsync(n => n.Slug == slug);
        }
    }
}