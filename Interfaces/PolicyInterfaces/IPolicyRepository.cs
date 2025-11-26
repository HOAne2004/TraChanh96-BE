// Interfaces/IPolicyRepository.cs
using drinking_be.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces.PolicyInterfaces
{
    public interface IPolicyRepository : IGenericRepository<Policy>
    {
        Task<IEnumerable<Policy>> GetActivePoliciesAsync();
        Task<Policy?> GetBySlugAsync(string slug);
    }
}