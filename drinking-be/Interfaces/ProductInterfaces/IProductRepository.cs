// Interfaces/ProductInterfaces/IProductRepository.cs
using drinking_be.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces.ProductInterfaces
{
    // Kế thừa các hàm CRUD cơ bản từ IGenericRepository
    public interface IProductRepository : IGenericRepository<Product>
    {
        // 1. Tải toàn bộ (có lọc productType và eager loading M:N)
        Task<IEnumerable<Product>> GetAllAsync(string? productType);

        // 2. Tải theo ID (có Eager Loading các Dependencies)
        Task<Product?> GetProductWithDependencies(long id);

        // 3. Lấy theo danh sách ID (Dùng cho logic Cart/Order)
        Task<IEnumerable<Product>> GetProductsByIdsAsync(List<int> productIds);

        // 4. Phương thức chuyên biệt: Cập nhật các bảng liên kết M:N
        void UpdateProductOptions(Product product,
                                  int[] newSizeIds,
                                  int[] newIceLevelIds,
                                  int[] newSugarLevelIds);

        // 5. Phương thức chuyên biệt: Đếm sản phẩm thuộc Category (Cho logic xóa Category)
        Task<int> CountProductsInCategoryAsync(int categoryId);
    }
}