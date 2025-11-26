namespace drinking_be.Enums
{
    public enum OrderStatusEnum : short
    {
        New = 1,
        Confirmed = 2,
        Preparing = 3,
        Ready = 4,
        Delivering = 5,
        Completed = 6,
        Cancelled = 7 // Thêm trạng thái này nếu cần
    }
}