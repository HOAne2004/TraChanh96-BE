using drinking_be.Interfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Repositories
{
    public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(DBDrinkContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(int userId)
        {
            return await _context.Reservations
                .Include(r => r.Store)
                .Include(r => r.AssignedTable)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.ReservationDatetime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByStoreIdAsync(int storeId, DateTime? dateFilter = null)
        {
            var query = _context.Reservations
                .Include(r => r.AssignedTable) // Include bàn để xem bàn nào đã được gán
                .Where(r => r.StoreId == storeId);

            if (dateFilter.HasValue)
            {
                // Lọc theo ngày cụ thể (bỏ qua phần giờ phút)
                query = query.Where(r => r.ReservationDatetime.Date == dateFilter.Value.Date);
            }

            return await query
                .OrderBy(r => r.ReservationDatetime)
                .ToListAsync();
        }

        public async Task<Reservation?> GetByCodeAsync(string code)
        {
            return await _context.Reservations
                .Include(r => r.Store)
                .Include(r => r.AssignedTable)
                .FirstOrDefaultAsync(r => r.ReservationCode == code);
        }

        // Override GetByIdAsync để Include thông tin chi tiết
        public new async Task<Reservation?> GetByIdAsync(int id) // Lưu ý: Id của Reservation là BIGINT (long) nhưng GenericRepo dùng int, bạn có thể cần điều chỉnh GenericRepo hoặc ép kiểu. 
        // Nếu GenericRepo của bạn dùng int cho ID, bạn cần cẩn thận vì Reservation Id là bigint.
        // Tốt nhất là sửa GenericRepository<T> thành GenericRepository<T, TKey> hoặc dùng object id.
        // Dưới đây giả định GenericRepo hỗ trợ hoặc ta viết method riêng.
        {
            // Do Id của bảng Reservation là BIGINT, mà GenericRepo thường định nghĩa int id.
            // Ta sẽ dùng FirstOrDefault thay vì FindAsync của base nếu base chỉ nhận int.
            return await _context.Reservations
               .Include(r => r.Store)
               .Include(r => r.AssignedTable)
               .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}