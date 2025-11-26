// Dtos/StoreDtos/StoreCreateDto.cs
using System.ComponentModel.DataAnnotations;
using System;

namespace drinking_be.Dtos.StoreDtos
{
    public class StoreCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        [Required]
        public int BrandId { get; set; } // Liên kết đến Brand

        public string? ImageUrl { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        [Required]
        public TimeSpan OpenTime { get; set; } // Sử dụng TimeSpan cho giờ mở cửa

        [Required]
        public TimeSpan CloseTime { get; set; } // Sử dụng TimeSpan cho giờ đóng cửa

        public bool IsActive { get; set; } = true; // Mặc định là Active
    }
}