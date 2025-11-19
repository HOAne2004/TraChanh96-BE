using drinking_be.Dtos.CategoryDtos;

namespace drinking_be.Dtos.ProductDtos
{
    public class ProductReadDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
        public string? Desciption { get; set; }       
        public string? Ingredient { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public DateTime LaunchDateTime { get; set; }
        public double? TotalRating { get; set; }
        public int? TotalSold { get; set; }

        // Thêm các danh sách ID tùy chọn HỢP LỆ (cho frontend hiển thị)
        public List<int> AllowedSizeIds { get; set; } = new List<int>();
        public List<int> AllowedIceLevelIds { get; set; } = new List<int>();
        public List<int> AllowedSugarLevelIds { get; set; } = new List<int>();

        // Thông tin Danh mục (có thể dùng CategoryReadDto)
        public int CategoryId { get; set; }
    }
}