namespace drinking_be.Dtos.ReservationDtos
{
    public class ReservationReadDto
    {
        public long Id { get; set; }
        public string? ReservationCode { get; set; }

        public int? UserId { get; set; }
        public int StoreId { get; set; }
        public string? StoreName { get; set; } // Hiển thị tên quán

        public DateTime ReservationDatetime { get; set; }
        public byte NumberOfGuests { get; set; }

        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? Note { get; set; }

        public byte Status { get; set; }
        public string? StatusLabel { get; set; } // Hiển thị text trạng thái (Pending, Confirmed...)

        public int? AssignedTableId { get; set; }
        public string? AssignedTableName { get; set; } // Hiển thị tên bàn nếu đã xếp

        public DateTime CreatedAt { get; set; }

        public decimal DepositAmount { get; set; }
        public bool IsDepositPaid { get; set; }
    }
}
