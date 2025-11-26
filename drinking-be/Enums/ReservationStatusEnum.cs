namespace drinking_be.Enums
{
    public enum ReservationStatusEnum : byte
    {
        Pending = 1,    // Chờ xác nhận
        Confirmed = 2,  // Đã xác nhận
        Arrived = 3,    // Khách đã đến
        NoShow = 4,     // Khách không đến
        Cancelled = 5,  // Đã hủy
        Completed = 6   // Hoàn thành
    }
}
