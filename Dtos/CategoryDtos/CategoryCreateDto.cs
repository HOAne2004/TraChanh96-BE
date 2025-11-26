using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.CategoryDtos
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Tên danh mục là bắt buộc.")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        // ParentId có thể rỗng nếu là danh mục cha
        public int? ParentId { get; set; }

        // Slug sẽ được tạo tự động, nhưng để trong DTO cho phép admin tùy chỉnh
        public string? Slug { get; set; }
    }
}