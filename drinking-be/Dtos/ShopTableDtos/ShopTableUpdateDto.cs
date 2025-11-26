using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.ShopTableDtos
{
    public class ShopTableUpdateDto
    {
        [Required(ErrorMessage = "Tên bàn không được để trống")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Range(1, 50, ErrorMessage = "Sức chứa phải từ 1 đến 50 người")]
        public byte Capacity { get; set; }

        public bool CanBeMerged { get; set; }

        public int? MergedWithTableId { get; set; } // Cho phép cập nhật ghép bàn ở đây nếu cần

        public bool IsActive { get; set; }
    }
}
