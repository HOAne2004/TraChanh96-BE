// Interfaces/IBrandRepository.cs
using drinking_be.Models;
using System.Threading.Tasks;

namespace drinking_be.Interfaces
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        // Lấy thông tin Brand chính (thường chỉ có 1 bản ghi hoặc bản ghi active)
        Task<Brand?> GetPrimaryBrandAsync();
    }
}