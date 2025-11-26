// Services/PaymentMethodService.cs
using drinking_be.Dtos.PaymentMethodDtos;
using drinking_be.Interfaces;
using drinking_be.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drinking_be.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IPaymentMethodRepository _methodRepo;
        private readonly IMapper _mapper;

        public PaymentMethodService(IPaymentMethodRepository methodRepo, IMapper mapper)
        {
            _methodRepo = methodRepo;
            _mapper = mapper;
        }

        // --- PUBLIC API ---

        public async Task<IEnumerable<PaymentMethodReadDto>> GetActiveMethodsAsync()
        {
            var methods = await _methodRepo.GetActiveMethodsAsync();

            // Ánh xạ Entity sang DTO
            return _mapper.Map<IEnumerable<PaymentMethodReadDto>>(methods);
        }

        // --- ADMIN API ---

        public async Task<PaymentMethodReadDto> CreatePaymentMethodAsync(PaymentMethodCreateDto methodDto)
        {
            // 1. Ánh xạ DTO sang Entity
            var method = _mapper.Map<PaymentMethod>(methodDto);

            // 2. Lưu vào DB
            await _methodRepo.AddAsync(method);
            await _methodRepo.SaveChangesAsync();

            return _mapper.Map<PaymentMethodReadDto>(method);
        }
    }
}