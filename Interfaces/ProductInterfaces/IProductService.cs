using drinking_be.Dtos.ProductDtos;
using drinking_be.Models;

namespace drinking_be.Interfaces.ProductInterfaces
{
    public interface IProductService
    {
        // CRUD cơ bản
        Task<IEnumerable<ProductReadDto>> GetAllProducts();
        Task<IEnumerable<ProductReadDto>> GetAllProductsAsync(string? productType);
        Task<ProductReadDto?> GetProductById(int id);
        Task<ProductReadDto> CreateProduct(ProductCreateDto productDto);
        Task<ProductReadDto?> UpdateProduct(int id, ProductUpdateDto productDto);
        Task<bool> DeleteProduct(int id);
    }
}