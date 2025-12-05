using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.ProductDtos
{
    public class ProductUpdateDto
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá cơ bản không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá tiền phải lớn hơn hoặc bằng 0")]
        public decimal BasePrice { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public int CategoryId { get; set; }

        // Tùy chọn: Cho phép sửa Slug (nếu muốn SEO tốt hơn), nếu null thì giữ nguyên hoặc tự tạo lại
        public string? Slug { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public string? Description { get; set; }

        public string? Ingredient { get; set; }

        // Trạng thái: 'Active', 'Draft', 'Archived', 'SoldOut'
        [Required]
        public string Status { get; set; } = "Active";

        [Required]
        public string ProductType { get; set; } = "Beverage";

        public DateTime? LaunchDateTime { get; set; }

        // --- CẬP NHẬT OPTIONS (QUAN TRỌNG) ---
        // Khi Update, ta gửi danh sách ID mới lên. 
        // Backend sẽ xóa các liên kết cũ và tạo liên kết mới theo danh sách này.
        public int[]? SizeIds { get; set; }
        public int[]? IceLevelIds { get; set; }
        public int[]? SugarLevelIds { get; set; }
    }
}