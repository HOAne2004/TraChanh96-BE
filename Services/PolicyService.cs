// Services/PolicyService.cs
using drinking_be.Dtos.PolicyDtos;
using drinking_be.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using drinking_be.Interfaces.PolicyInterfaces;

namespace drinking_be.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _policyRepo;
        private readonly IMapper _mapper;

        public PolicyService(IPolicyRepository policyRepo, IMapper mapper)
        {
            _policyRepo = policyRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PolicyReadDto>> GetActivePoliciesAsync()
        {
            var policies = await _policyRepo.GetActivePoliciesAsync();

            // Ánh xạ danh sách Policy Entities sang DTO
            return _mapper.Map<IEnumerable<PolicyReadDto>>(policies);
        }

        public async Task<PolicyReadDto?> GetPolicyBySlugAsync(string slug)
        {
            var policy = await _policyRepo.GetBySlugAsync(slug);
            if (policy == null || !policy.IsActive == true)
            {
                return null;
            }

            // Ánh xạ Policy Entity sang DTO
            return _mapper.Map<PolicyReadDto>(policy);
        }

        public async Task<PolicyReadDto> CreatePolicyAsync(PolicyCreateDto policyDto)
        {
            // 1. Ánh xạ DTO sang Entity
            var policy = _mapper.Map<Policy>(policyDto);

            // Logic tạo Slug (Cần phải là duy nhất)
            // LƯU Ý: AutoMapper đã được cấu hình để tạo Slug, nhưng ta có thể ghi đè/kiểm tra tại đây.
            // Nếu bạn muốn tạo Slug an toàn, cần kiểm tra tính duy nhất.

            // 2. Lưu vào DB
            await _policyRepo.AddAsync(policy);
            await _policyRepo.SaveChangesAsync();

            return _mapper.Map<PolicyReadDto>(policy);
        }
    }
}