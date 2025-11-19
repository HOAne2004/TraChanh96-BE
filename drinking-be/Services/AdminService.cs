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
    }
}