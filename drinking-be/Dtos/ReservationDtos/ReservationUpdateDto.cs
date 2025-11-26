using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.ReservationDtos
{
    public class ReservationUpdateDto
    {
        // Dùng để Admin cập nhật trạng thái hoặc gán bàn
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }

        [Required]
        public byte Status { get; set; } // Sử dụng Enum value

        public int? AssignedTableId { get; set; } // Có thể null nếu chưa xếp bàn hoặc hủy xếp

        public string? Note { get; set; } // Admin có thể ghi chú thêm
        public decimal DepositAmount { get; set; }
    }
}
