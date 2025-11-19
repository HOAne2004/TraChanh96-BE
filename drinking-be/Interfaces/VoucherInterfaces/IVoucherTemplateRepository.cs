// Interfaces/IVoucherTemplateRepository.cs
using drinking_be.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces
{
    public interface IVoucherTemplateRepository : IGenericRepository<VoucherTemplate>
    {
        // Lấy danh sách voucher template (kèm theo Level Name)
        Task<IEnumerable<VoucherTemplate>> GetAllWithLevelAsync();
        Task<VoucherTemplate?> GetByIdWithLevelAsync(int id);
    }
}