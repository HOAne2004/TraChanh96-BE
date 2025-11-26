using AutoMapper;
using drinking_be.Dtos.ReservationDtos;
using drinking_be.Enums;
using drinking_be.Interfaces;
using drinking_be.Interfaces.ShopTableInterfaces;
using drinking_be.Interfaces.ReservationInterfaces;
using drinking_be.Models;

namespace drinking_be.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repository;
        private readonly IShopTableRepository _tableRepository; // Để kiểm tra bàn khi gán
        private readonly IMapper _mapper;

        public ReservationService(IReservationRepository repository, IShopTableRepository tableRepository, IMapper mapper)
        {
            _repository = repository;
            _tableRepository = tableRepository;
            _mapper = mapper;
        }

        public async Task<ReservationReadDto> CreateReservationAsync(ReservationCreateDto createDto)
        {
            // 1. Map DTO sang Entity
            var reservation = _mapper.Map<Reservation>(createDto);

            // 2. Sinh mã đặt chỗ (Ví dụ: RES-YYYYMMDD-XXXX)
            string datePart = DateTime.Now.ToString("yyyyMMdd");
            string randomPart = new Random().Next(1000, 9999).ToString();
            reservation.ReservationCode = $"RES-{datePart}-{randomPart}";

            // 3. Set giá trị mặc định
            reservation.Status = (byte)ReservationStatusEnum.Pending;
            reservation.CreatedAt = DateTime.Now;
            reservation.UpdatedAt = DateTime.Now;

            // 4. Lưu vào DB
            await _repository.AddAsync(reservation);
            await _repository.SaveChangesAsync();

            // 5. Trả về DTO (Load lại kèm tên quán nếu cần, hoặc map ngược lại luôn)
            // Ở đây ta map ngược lại object vừa lưu, lưu ý StoreName sẽ null nếu không load lại từ DB
            // Để đơn giản ta trả về thông tin cơ bản
            // --- LOGIC TÍNH TIỀN CỌC ---
            // Quy định: 10.000 VNĐ / người
            decimal depositPerPerson = 10000;
            reservation.DepositAmount = createDto.NumberOfGuests * depositPerPerson;

            // Mặc định vừa đặt xong thì chưa thanh toán cọc
            reservation.IsDepositPaid = false;

            // ... các thiết lập khác ...

            await _repository.AddAsync(reservation);
            await _repository.SaveChangesAsync();

            return _mapper.Map<ReservationReadDto>(reservation);
        }

        public async Task<ReservationReadDto?> GetReservationByIdAsync(long id)
        {
            // Ép kiểu int nếu GenericRepo yêu cầu, hoặc sửa GenericRepo
            var reservation = await _repository.GetByIdAsync((int)id);
            if (reservation == null) return null;
            return _mapper.Map<ReservationReadDto>(reservation);
        }

        public async Task<ReservationReadDto?> GetReservationByCodeAsync(string code)
        {
            var reservation = await _repository.GetByCodeAsync(code);
            if (reservation == null) return null;
            return _mapper.Map<ReservationReadDto>(reservation);
        }

        public async Task<IEnumerable<ReservationReadDto>> GetHistoryByUserIdAsync(int userId)
        {
            var list = await _repository.GetReservationsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<ReservationReadDto>>(list);
        }

        public async Task<IEnumerable<ReservationReadDto>> GetReservationsByStoreAsync(int storeId, DateTime? date)
        {
            var list = await _repository.GetReservationsByStoreIdAsync(storeId, date);
            return _mapper.Map<IEnumerable<ReservationReadDto>>(list);
        }

        public async Task<bool> UpdateReservationAsync(long id, ReservationUpdateDto updateDto)
        {
            var reservation = await _repository.GetByIdAsync((int)id);
            if (reservation == null) return false;

            // Kiểm tra bàn nếu có gán bàn
            if (updateDto.AssignedTableId.HasValue)
            {
                var table = await _tableRepository.GetByIdAsync(updateDto.AssignedTableId.Value);
                if (table == null || table.StoreId != reservation.StoreId)
                {
                    throw new Exception("Bàn không hợp lệ hoặc không thuộc cửa hàng này.");
                }
                reservation.AssignedTableId = updateDto.AssignedTableId;
            }

            reservation.Status = updateDto.Status;

            if (!string.IsNullOrEmpty(updateDto.Note))
            {
                // Có thể nối thêm note hoặc ghi đè
                reservation.Note = updateDto.Note;
            }

            reservation.UpdatedAt = DateTime.Now;

            _repository.Update(reservation);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelReservationAsync(long id, int? userId)
        {
            var reservation = await _repository.GetByIdAsync((int)id);
            if (reservation == null) return false;

            // Nếu có userId, kiểm tra xem người hủy có phải chủ đơn không
            if (userId.HasValue && reservation.UserId != userId)
            {
                return false; // Không có quyền hủy đơn của người khác
            }

            // Chỉ cho phép hủy khi trạng thái là Pending hoặc Confirmed
            if (reservation.Status != (byte)ReservationStatusEnum.Pending &&
                reservation.Status != (byte)ReservationStatusEnum.Confirmed)
            {
                throw new Exception("Không thể hủy đặt bàn ở trạng thái này.");
            }

            reservation.Status = (byte)ReservationStatusEnum.Cancelled;
            reservation.UpdatedAt = DateTime.Now;

            _repository.Update(reservation);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ConfirmDepositAsync(long id)
        {
            var reservation = await _repository.GetByIdAsync((int)id); // Lưu ý vụ ép kiểu int/long
            if (reservation == null) return false;

            reservation.IsDepositPaid = true;
            // Thường khi đóng cọc xong thì trạng thái đơn cũng nên chuyển sang Confirmed
            reservation.Status = (byte)ReservationStatusEnum.Confirmed;
            reservation.UpdatedAt = DateTime.Now;

            _repository.Update(reservation);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}