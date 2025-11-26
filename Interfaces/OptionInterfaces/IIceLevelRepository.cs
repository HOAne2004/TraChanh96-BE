using drinking_be.Models;

namespace drinking_be.Interfaces.OptionInterfaces
{
    public interface IIceLevelRepository : IGenericRepository<IceLevel>
    {
        // Phương thức để lấy nhiều ice level theo ID
        Task<IEnumerable<IceLevel>> GetIceLevelsByIdsAsync(List<short> iceLevelIds);    
    }
}
