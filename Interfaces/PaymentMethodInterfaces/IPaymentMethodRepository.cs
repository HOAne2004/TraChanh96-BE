// Interfaces/IPaymentMethodRepository.cs
using drinking_be.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces
{
    public interface IPaymentMethodRepository : IGenericRepository<PaymentMethod>
    {
        // Lấy danh sách các phương thức đang hoạt động
        Task<IEnumerable<PaymentMethod>> GetActiveMethodsAsync();
    }
}