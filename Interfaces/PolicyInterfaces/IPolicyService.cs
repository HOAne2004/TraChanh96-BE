// Interfaces/IPolicyService.cs
using drinking_be.Dtos.PolicyDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces.PolicyInterfaces
{
    public interface IPolicyService
    {
        Task<IEnumerable<PolicyReadDto>> GetActivePoliciesAsync();
        Task<PolicyReadDto?> GetPolicyBySlugAsync(string slug);
        Task<PolicyReadDto> CreatePolicyAsync(PolicyCreateDto policyDto);
    }
}