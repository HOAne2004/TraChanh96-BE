using drinking_be.Interfaces.ProductInterfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            // Chỉ lấy thông tin cơ bản để tính tiền, không Include nặng
            return await _context.Products
                                 .Where(p => productIds.Contains(p.Id))
                                 .ToListAsync();
        }

        public async Task<Product?> GetProductWithDependencies(long id)
        {
            // Eager Loading: Lấy sâu vào bảng Size/Level để lấy Label/Price
            return await _context.Products
                .Include(p => p.Category) // Lấy luôn Category nếu cần
                .Include(p => p.ProductSizes)
                    .ThenInclude(ps => ps.Size) // ⭐️ Quan trọng: Lấy chi tiết Size
                .Include(p => p.ProductIceLevels)
                    .ThenInclude(pi => pi.IceLevel)
                .Include(p => p.ProductSugarLevels)
                    .ThenInclude(ps => ps.SugarLevel)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public void UpdateProductOptions(Product product,
                                         int[] newSizeIds,
                                         int[] newIceLevelIds,
                                         int[] newSugarLevelIds)
        {
            // 1. Xử lý Size
            // Lưu ý: Phải đảm bảo product.ProductSizes đã được load (qua Include) trước khi gọi hàm này
            if (product.ProductSizes != null)
            {
                // Xóa các liên kết cũ
                _context.ProductSizes.RemoveRange(product.ProductSizes);
            }

            if (newSizeIds != null)
            {
                foreach (var sizeId in newSizeIds)
                {
                    product.ProductSizes.Add(new ProductSize
                    {
                        ProductId = product.Id,
                        SizeId = (short)sizeId 
                    });
                }
            }

            // 2. Xử lý Ice Level
            if (product.ProductIceLevels != null)
            {
                _context.ProductIceLevels.RemoveRange(product.ProductIceLevels);
            }

            if (newIceLevelIds != null)
            {
                foreach (var iceId in newIceLevelIds)
                {
                    product.ProductIceLevels.Add(new ProductIceLevel
                    {
                        ProductId = product.Id,
                        IceLevelId = (short)iceId
                    });
                }
            }

            // 3. Xử lý Sugar Level
            if (product.ProductSugarLevels != null)
            {
                _context.ProductSugarLevels.RemoveRange(product.ProductSugarLevels);
            }

            if (newSugarLevelIds != null)
            {
                foreach (var sugarId in newSugarLevelIds)
                {
                    product.ProductSugarLevels.Add(new ProductSugarLevel
                    {
                        ProductId = product.Id,
                        SugarLevelId = (short)sugarId
                    });
                }
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync(string? productType)
        {
            var query = _context.Products.AsQueryable();

            // Include Category để hiển thị tên danh mục
            query = query.Include(p => p.Category);

            // Nếu muốn hiển thị Options ngay ở danh sách, mở comment dưới:
            // query = query.Include(p => p.ProductSizes).ThenInclude(ps => ps.Size);

            if (!string.IsNullOrEmpty(productType))
            {
                // So sánh không phân biệt hoa thường (Postgres có thể phân biệt, nên dùng ToLower hoặc EF.Functions.ILike)
                var typeLower = productType.ToLower();
                query = query.Where(p => p.ProductType.ToLower() == typeLower);
            }

            return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public async Task<int> CountProductsInCategoryAsync(int categoryId)
        {
            return await _context.Products.CountAsync(p => p.CategoryId == categoryId);
        }
    }
}