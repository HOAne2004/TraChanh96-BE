using AutoMapper;
using drinking_be.Dtos.ProductDtos;
using drinking_be.Interfaces.ProductInterfaces;
using drinking_be.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace drinking_be.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        // --- Hàm hỗ trợ ánh xạ phức tạp (quan trọng nhất) ---
        private ProductReadDto MapProductToReadDto(Product product)
        {
            var dto = _mapper.Map<ProductReadDto>(product);

            dto.AllowedSizeIds = product.ProductSizes?
                 .Select(ps => (int)ps.SizeId).ToList() ?? new List<int>();

            dto.AllowedIceLevelIds = product.ProductIceLevels?
                .Select(pi => (int)pi.IceLevelId).ToList() ?? new List<int>();

            dto.AllowedSugarLevelIds = product.ProductSugarLevels?
                .Select(ps => (int)ps.SugarLevelId).ToList() ?? new List<int>();

            return dto;
        }

        // --- Implement các phương thức CRUD ---

        public async Task<IEnumerable<ProductReadDto>> GetAllProducts()
        {
            // LỖI: GetAll
            var products = await _productRepository.GetAllAsync(); // Đổi thành GetAllAsync
            return products.Select(p => MapProductToReadDto(p)).ToList();
        }

        public async Task<ProductReadDto?> GetProductById(int id)
        {
            // Sử dụng GetProductWithDependencies để tải cả các tùy chọn M:N
            var product = await _productRepository.GetProductWithDependencies(id);
            if (product == null)
            {
                return null;
            }

            return MapProductToReadDto(product);
        }

        public async Task<ProductReadDto> CreateProduct(ProductCreateDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            // Thêm Product vào DB để lấy Product.Id (cần thiết cho các bảng liên kết)
            await _productRepository.AddAsync(product);

            // Cập nhật các bảng liên kết M:N
            _productRepository.UpdateProductOptions(
                product,
                productDto.SizeIds,
                productDto.IceLevelIds,
                productDto.SugarLevelIds
            );

            await _productRepository.SaveChangesAsync();

            // Tải lại Product với các Dependencies để trả về DTO đầy đủ
            var createdProduct = await _productRepository.GetProductWithDependencies(product.Id);
            return MapProductToReadDto(createdProduct!);
        }

        public async Task<ProductReadDto?> UpdateProduct(int id, ProductUpdateDto productDto)
        {
            // Lấy Product cũ cùng với các Dependencies (để Repository có thể xử lý việc xóa/thêm liên kết)
            var existingProduct = await _productRepository.GetProductWithDependencies(id);
            if (existingProduct == null)
            {
                return null;
            }

            // Cập nhật các trường cơ bản (tên, giá, category)
            _mapper.Map(productDto, existingProduct);

            // Cập nhật các bảng liên kết M:N
            _productRepository.UpdateProductOptions(
                existingProduct,
                productDto.SizeIds,
                productDto.IceLevelIds,
                productDto.SugarLevelIds
            );

            // Lưu thay đổi
            _productRepository.Update(existingProduct);
            await _productRepository.SaveChangesAsync();

            // Trả về DTO sau khi cập nhật
            return MapProductToReadDto(existingProduct);
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _productRepository.GetProductWithDependencies(id);
            if (product == null)
            {
                return false;
            }

            // Lưu ý: Tùy thuộc vào cấu hình Cascade Delete của CSDL, 
            // việc xóa Product có thể tự động xóa các mục trong bảng liên kết.
            // Nếu không, cần xóa thủ công các mục trong Product_Size, Product_Topping, v.v.

            _productRepository.Delete(product);
            await _productRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProductReadDto>> GetAllProductsAsync(string? productType)
        {
            // ⭐️ GỌI: Hàm repo mới (sẽ tạo ở bước sau)
            var products = await _productRepository.GetAllAsync(productType);
            return products.Select(p => MapProductToReadDto(p)).ToList();
        }
    }
}