using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.BrandDtos
{
    public class BrandUpdateDto
    {
        [Required(ErrorMessage = "Tên thương hiệu không được để trống")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? LogoUrl { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        [MaxLength(20)]
        [Phone(ErrorMessage = "Số điện thoại hotline không đúng định dạng")]
        public string? Hotline { get; set; }

        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Email hỗ trợ không đúng định dạng")]
        public string? EmailSupport { get; set; }

        [MaxLength(30)]
        public string? TaxCode { get; set; }

        [MaxLength(100)]
        public string? CompanyName { get; set; }

        [MaxLength(255)]
        public string? Slogan { get; set; }

        [MaxLength(255)]
        public string? CopyrightText { get; set; }

        // Lưu ý: SocialMedia là danh sách liên kết 1-N. 
        // Thường ta sẽ cập nhật nó qua API riêng hoặc xử lý logic thay thế toàn bộ trong Service.
        // Ở DTO cơ bản này ta chỉ tập trung vào thông tin chính của Brand.
    }
}