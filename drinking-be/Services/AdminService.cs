// Services/AdminService.cs
using AutoMapper;
using drinking_be.Dtos.UserDtos;
using drinking_be.Interfaces;
using drinking_be.Interfaces.UserInterfaces;
using drinking_be.Models;

namespace drinking_be.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public AdminService(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }


        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync()
        {
            // Lấy tất cả user (chúng ta có thể thêm logic lọc sau)
            var users = await _userRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<UserReadDto>>(users);
        }

        // ⭐️ THÊM: Triển khai Cập nhật người dùng
        public async Task<UserReadDto?> UpdateUserByPublicIdAsync(Guid publicId, UserUpdateDto updateDto)
        {
            // 1. Tìm người dùng cũ (dùng FindAsync của GenericRepo)
            var existingUser = (await _userRepo.FindAsync(u => u.PublicId == publicId)).FirstOrDefault();

            if (existingUser == null)
            {
                return null;
            }

            // 2. Ánh xạ các trường đã thay đổi từ DTO sang Entity
            // AutoMapper sẽ chỉ ánh xạ các trường không null/default trong DTO
            _mapper.Map(updateDto, existingUser);

            // 3. Cập nhật UpdatedAt và lưu
            existingUser.UpdatedAt = DateTime.UtcNow;
            _userRepo.Update(existingUser);
            await _userRepo.SaveChangesAsync();

            return _mapper.Map<UserReadDto>(existingUser);
        }

        // ⭐️ THÊM: Triển khai Xóa người dùng
        public async Task<bool> DeleteUserByPublicIdAsync(Guid publicId)
        {
            // 1. Tìm người dùng
            var user = (await _userRepo.FindAsync(u => u.PublicId == publicId)).FirstOrDefault();

            if (user == null)
            {
                return false;
            }

            // 2. Xóa
            _userRepo.Delete(user);
            await _userRepo.SaveChangesAsync();
            return true;
        }
    }
}