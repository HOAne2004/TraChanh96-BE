namespace drinking_be.Dtos.ShopTableDtos
{
    public class ShopTableReadDto
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; } = string.Empty; // Thêm tên quán để tiện hiển thị
        public string Name { get; set; } = string.Empty;
        public byte Capacity { get; set; }
        public bool CanBeMerged { get; set; }
        public int? MergedWithTableId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
