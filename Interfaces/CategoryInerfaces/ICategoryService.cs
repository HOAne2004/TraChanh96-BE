// Interfaces/CategoryInterfaces/ICategoryService.cs
using drinking_be.Dtos.CategoryDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drinking_be.Interfaces.CategoryInerfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync(string? searchQuery);
        Task<CategoryReadDto?> GetCategoryByIdAsync(int id);

        Task<CategoryReadDto> CreateCategoryAsync(CategoryCreateDto categoryDto);
        Task<CategoryReadDto?> UpdateCategoryAsync(int id, CategoryUpdateDto categoryDto);
        Task<bool> DeleteCategoryAsync(int id);

        Task<IEnumerable<CategoryReadDto>> GetCategoryTreeAsync();
        Task<int> CountProductsInCategoryAsync(int id);
    }
}