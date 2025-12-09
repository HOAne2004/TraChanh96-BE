using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.StoreDtos
{
    public class StoreUpdateDto
    {
        [Required(ErrorMessage = "Tên cửa hàng không được để trống")]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn thương hiệu")]
        public int BrandId { get; set; }

        [MaxLength(200)]
        // Cho phép sửa Slug (URL), nếu null thì giữ nguyên
        public string? Slug { get; set; }

        [MaxLength(255)]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        // Tọa độ bản đồ
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        // Ngày khai trương (Nhận format chuỗi "yyyy-MM-dd" từ JSON)
        public DateTime? OpenDate { get; set; }

        // Giờ mở cửa (Nhận format chuỗi "HH:mm:ss" từ JSON)
        // Ví dụ: "08:00:00"
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }

        public decimal? ShippingFee { get; set; }

        public bool IsActive { get; set; } = true;

        public byte SortOrder { get; set; } = 0;

        public bool MapVerified { get; set; } = false;
    }
}