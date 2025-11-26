using drinking_be.Models;

namespace drinking_be.Interfaces
{
    public interface IReservationRepository : IGenericRepository<Reservation>
    {
        // Lấy lịch sử đặt bàn của một User
        Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(int userId);

        // Lấy danh sách đặt bàn của một Cửa hàng (có thể lọc theo ngày)
        Task<IEnumerable<Reservation>> GetReservationsByStoreIdAsync(int storeId, DateTime? dateFilter = null);

        // Tìm theo mã Code (để tra cứu nhanh)
        Task<Reservation?> GetByCodeAsync(string code);
    }
}