// Dtos/StoreDtos/StoreReadDto.cs
using System;

namespace drinking_be.Dtos.StoreDtos
{
    public class StoreReadDto
    {
        public long Id { get; set; }
        public Guid PublicId { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        public bool IsActive { get; set; }

        // Thêm thông tin Brand (nếu cần Eager Loading)
        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
    }
}