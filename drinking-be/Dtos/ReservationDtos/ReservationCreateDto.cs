using System.ComponentModel.DataAnnotations;

namespace drinking_be.Dtos.ReservationDtos
{
    public class ReservationCreateDto
    {
        public int? UserId { get; set; } // Có thể null nếu khách vãng lai

        [Required(ErrorMessage = "Vui lòng chọn cửa hàng")]
        public int StoreId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn thời gian")]
        public DateTime ReservationDatetime { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Số lượng khách phải từ 1 người trở lên")]
        public byte NumberOfGuests { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên người đặt")]
        [MaxLength(100)]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [MaxLength(20)]
        [Phone]
        public string CustomerPhone { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }
    }
}
