using drinking_be.Models;

namespace drinking_be.Interfaces.ProductInterfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllAsync(string? productType);
        Task<Product?> GetProductWithDependencies(long id);
        Task<IEnumerable<Product>> GetProductsByIdsAsync(List<int> productIds);

        // Phương thức mới để xử lý các bảng liên kết M:N
        void UpdateProductOptions(Product product,
                                  int[] newSizeIds,
                                  int[] newIceLevelIds,
                                  int[] newSugarLevelIds);
    }
}