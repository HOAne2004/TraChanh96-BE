using drinking_be.Interfaces.ProductInterfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly new DBDrinkContext _context;
        public ProductRepository(DBDrinkContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetProductsByIdsAsync(List<int> productIds)
        {
            // Đảm bảo không tải Dependencies để giữ tốc độ cho việc tính giá hàng loạt
            return await _context.Products
                                 .Where(p => productIds.Contains(p.Id))
                                 .ToListAsync();
        }
        public async Task<Product?> GetProductWithDependencies(long id)
        {
            // Eager Loading: Tải các quan hệ M:N cần thiết cho logic tùy chọn
            return await _context.Products
                .Include(p => p.ProductSizes) 
                .Include(p => p.ProductIceLevels)
                .Include(p => p.ProductSugarLevels)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public void UpdateProductOptions(Product product,
                                         int[] newSizeIds,
                                         int[] newIceLevelIds,
                                         int[] newSugarLevelIds)
        {
            // Đây là logic phức tạp, xóa cũ và thêm mới quan hệ M:N
            // (Hiện tại chỉ là skeleton, logic chi tiết sẽ được viết trong phần sau)

            // Ví dụ cho Size: Xóa hết các Size cũ và thêm Size mới
            product.ProductSizes.Clear();
            foreach (var sizeId in newSizeIds)
            {
                // LƯU Ý: Đảm bảo ProductSize đã được định nghĩa
                product.ProductSizes.Add(new ProductSize { ProductId = product.Id, SizeId = (short)sizeId });
            }

            // Bổ sung cho Ice Level
            product.ProductIceLevels.Clear();
            foreach (var iceId in newIceLevelIds)
            {
                product.ProductIceLevels.Add(new ProductIceLevel { ProductId = product.Id, IceLevelId = (short)iceId });
            }

            // Bổ sung cho Sugar Level
            product.ProductSugarLevels.Clear();
            foreach (var sugarId in newSugarLevelIds)
            {
                product.ProductSugarLevels.Add(new ProductSugarLevel { ProductId = product.Id, SugarLevelId = (short)sugarId });
            }

            // Áp dụng logic tương tự cho các tùy chọn khác...
        }

        public async Task<IEnumerable<Product>> GetAllAsync(string? productType)
        {
            var query = _context.Products
                // ⭐️ QUAN TRỌNG: Phải Include để Service có dữ liệu map sang DTO
                //.Include(p => p.ProductSizes)
                //.Include(p => p.ProductIceLevels)
                //.Include(p => p.ProductSugarLevels)
                .AsQueryable();

            if (!string.IsNullOrEmpty(productType))
            {
                // So sánh không phân biệt hoa thường
                query = query.Where(p => p.ProductType == productType);
            }

            // Sắp xếp sản phẩm mới nhất lên đầu
            return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public async Task<int> CountProductsInCategoryAsync(int categoryId)
        {
            return await _context.Products.CountAsync(p => p.CategoryId == categoryId);
        }
    }
}