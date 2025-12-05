using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.CategoryDtos
{
    public class CategoryUpdateDto
    {
        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [MaxLength(100, ErrorMessage = "Tên danh mục không được quá 100 ký tự")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        // Cho phép sửa Slug (nếu muốn custom SEO), nếu để null hệ thống sẽ giữ nguyên hoặc tự tạo lại từ Name
        public string? Slug { get; set; }

        // Cho phép di chuyển danh mục (đổi cha)
        public int? ParentId { get; set; }

        public bool IsActive { get; set; } = true;

        public byte SortOrder { get; set; } = 0;
    }
}