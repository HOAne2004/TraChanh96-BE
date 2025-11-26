namespace drinking_be.Dtos.CategoryDtos
{
    public class CategoryReadDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // ⭐️ Cho phép hiển thị cấu trúc cây (Nested Categories)
        public List<CategoryReadDto> Children { get; set; } = new List<CategoryReadDto>();
    }
}