// Interfaces/IPaymentMethodService.cs
using drinking_be.Dtos.PaymentMethodDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces
{
    public interface IPaymentMethodService
    {
        // Public API
        Task<IEnumerable<PaymentMethodReadDto>> GetActiveMethodsAsync();

        // Admin API
        Task<PaymentMethodReadDto> CreatePaymentMethodAsync(PaymentMethodCreateDto methodDto);
        // ... (Thêm Update/Delete nếu cần)
    }
}