using drinking_be.Models;

namespace drinking_be.Interfaces.OptionInterfaces
{
    public interface ISugarLevelRepository : IGenericRepository<SugarLevel>
    {
        // Phương thức để lấy nhiều sugar level theo ID
        Task<IEnumerable<SugarLevel>> GetSugarLevelsByIdsAsync(List<short> sugarLevelIds);
    }
}
