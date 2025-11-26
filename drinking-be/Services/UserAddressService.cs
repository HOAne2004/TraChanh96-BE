// Services/UserAddressService.cs
using AutoMapper;
using drinking_be.Dtos.UserAddressDtos;
using drinking_be.Interfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Services
{
    public class UserAddressService : IUserAddressService
    {
        private const int MAX_ADDRESSES_PER_USER = 5; // ⭐️ GIỚI HẠN BUSINESS RULE
        private readonly IUserAddressRepository _userAddressRepo;
        private readonly IMapper _mapper;

        public UserAddressService(IUserAddressRepository userAddressRepo, IMapper mapper)
        {
            _userAddressRepo = userAddressRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserAddressReadDto>> GetAllMyAddressesAsync(int userId)
        {
            var addresses = await _userAddressRepo.GetAddressesByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<UserAddressReadDto>>(addresses);
        }

        public async Task<UserAddressReadDto> CreateAddressAsync(int userId, UserAddressCreateDto dto)
        {
            // 1. Kiểm tra giới hạn số lượng (ENFORCEMENT)
            int addressCount = await _userAddressRepo.CountAddressesByUserIdAsync(userId);
            if (addressCount >= MAX_ADDRESSES_PER_USER)
            {
                throw new Exception($"Đã đạt giới hạn {MAX_ADDRESSES_PER_USER} địa chỉ. Vui lòng xóa địa chỉ cũ.");
            }

            // 2. Xử lý địa chỉ mặc định (BUSINESS LOGIC)
            if (dto.IsDefault)
            {
                var currentDefault = await _userAddressRepo.FindDefaultAddressAsync(userId);
                if (currentDefault != null)
                {
                    currentDefault.IsDefault = false; // Gỡ cờ mặc định của địa chỉ cũ
                    _userAddressRepo.Update(currentDefault);
                    // Không cần SaveChanges ở đây; transaction sẽ xử lý
                }
            }

            // 3. Map và lưu địa chỉ mới
            var newAddress = _mapper.Map<UserAddress>(dto);
            newAddress.UserId = userId;
            newAddress.CreatedAt = DateTime.UtcNow;

            // Nếu đây là địa chỉ đầu tiên, buộc nó là mặc định
            if (addressCount == 0)
            {
                newAddress.IsDefault = true;
            }

            await _userAddressRepo.AddAsync(newAddress);
            await _userAddressRepo.SaveChangesAsync();

            return _mapper.Map<UserAddressReadDto>(newAddress);
        }

        public async Task<UserAddressReadDto?> UpdateAddressAsync(int addressId, int userId, UserAddressCreateDto dto)
        {
            var existingAddress = await _userAddressRepo.GetByIdAsync(addressId);

            // 1. Kiểm tra quyền sở hữu
            if (existingAddress == null || existingAddress.UserId != userId)
            {
                return null; // Trả về null, Controller sẽ trả về 404/403
            }

            // 2. Xử lý địa chỉ mặc định (BUSINESS LOGIC)
            if (dto.IsDefault)
            {
                var currentDefault = await _userAddressRepo.FindDefaultAddressAsync(userId);
                if (currentDefault != null && currentDefault.Id != addressId)
                {
                    currentDefault.IsDefault = false; // Gỡ cờ mặc định của địa chỉ cũ
                    _userAddressRepo.Update(currentDefault);
                }
            }
            else if (!dto.IsDefault && (existingAddress.IsDefault ?? false))
            {
                // Logic phức tạp hơn: Nếu người dùng cố ý bỏ chọn địa chỉ mặc định,
                // chúng ta có thể ngăn chặn hoặc chuyển mặc định sang địa chỉ khác.
                // Ở đây, ta chỉ cho phép bỏ chọn, nhưng không tự chọn cái mới.
            }

            // 3. Map và lưu
            _mapper.Map(dto, existingAddress);
            existingAddress.UpdatedAt = DateTime.UtcNow;

            _userAddressRepo.Update(existingAddress);
            await _userAddressRepo.SaveChangesAsync();

            return _mapper.Map<UserAddressReadDto>(existingAddress);
        }

        public async Task<bool> DeleteAddressAsync(int addressId, int userId)
        {
            var address = await _userAddressRepo.GetByIdAsync(addressId);

            // 1. Kiểm tra quyền sở hữu
            if (address == null || address.UserId != userId)
            {
                return false;
            }

            // 2. Xử lý logic xóa địa chỉ mặc định
            if (address.IsDefault ?? false)
            {
                // Không cho phép xóa địa chỉ mặc định nếu đây không phải là địa chỉ cuối cùng
                int count = await _userAddressRepo.CountAddressesByUserIdAsync(userId);
                if (count > 1)
                {
                    throw new Exception("Không thể xóa địa chỉ mặc định. Vui lòng chọn địa chỉ khác làm mặc định trước.");
                }
            }

            _userAddressRepo.Delete(address);
            await _userAddressRepo.SaveChangesAsync();
            return true;
        }
    }
}