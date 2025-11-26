using drinking_be.Dtos.ReservationDtos;
using drinking_be.Enums;


namespace drinking_be.Interfaces.ReservationInterfaces;

public interface IReservationService
{
    Task<ReservationReadDto> CreateReservationAsync(ReservationCreateDto createDto);
    Task<ReservationReadDto?> GetReservationByIdAsync(long id);
    Task<ReservationReadDto?> GetReservationByCodeAsync(string code);
    Task<IEnumerable<ReservationReadDto>> GetHistoryByUserIdAsync(int userId);
    Task<IEnumerable<ReservationReadDto>> GetReservationsByStoreAsync(int storeId, DateTime? date);

    // Cập nhật trạng thái hoặc gán bàn
    Task<bool> UpdateReservationAsync(long id, ReservationUpdateDto updateDto);

    // Hủy đặt bàn (User tự hủy)
    Task<bool> CancelReservationAsync(long id, int? userId);
    Task<bool> ConfirmDepositAsync(long id);
   
      
 
}