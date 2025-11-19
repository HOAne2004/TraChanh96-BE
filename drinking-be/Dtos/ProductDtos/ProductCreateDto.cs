using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.ProductDtos
{
    public class ProductCreateDto
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal BasePrice { get; set; }

        [Required]
        public int CategoryId { get; set; }

        // THÊM các fields optional
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; } = "Available";
        public string ProductType { get; set; } = "Drink";
        public string? Ingredient { get; set; }
        public DateTime? LaunchDateTime { get; set; }

        // Mảng ID tùy chọn sẽ được gửi từ Frontend để tạo liên kết M:N
        public int[] SizeIds { get; set; } = Array.Empty<int>();
        public int[] IceLevelIds { get; set; } = Array.Empty<int>();
        public int[] SugarLevelIds { get; set; } = Array.Empty<int>();
    }
}