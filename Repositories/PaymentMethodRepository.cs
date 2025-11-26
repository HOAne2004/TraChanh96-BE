// Repositories/PaymentMethodRepository.cs
using drinking_be.Interfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drinking_be.Repositories
{
    public class PaymentMethodRepository : GenericRepository<PaymentMethod>, IPaymentMethodRepository
    {
        private readonly DBDrinkContext _context;

        public PaymentMethodRepository(DBDrinkContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaymentMethod>> GetActiveMethodsAsync()
        {
            // Lấy tất cả các phương thức thanh toán có IsActive = true
            return await _context.PaymentMethods
                                 .Where(p => p.IsActive == true)
                                 .OrderBy(p => p.Id)
                                 .ToListAsync();
        }
    }
}