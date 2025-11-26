// Services/UserService.cs
using AutoMapper;
using drinking_be.Dtos.UserDtos;
using drinking_be.Interfaces;
using drinking_be.Interfaces.MembershipInterfaces;
using drinking_be.Interfaces.UserInterfaces;
using drinking_be.Models;
using drinking_be.Utils; // Dùng PasswordHasher
using System.Linq;
using System.Threading.Tasks;

namespace drinking_be.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IMapper _mapper;

        private readonly IMembershipRepository _membershipRepo;
        private readonly IMembershipLevelRepository _levelRepo;

        public UserService(IUserRepository userRepo,
                           IJwtGenerator jwtGenerator,
                           IMapper mapper,
                           IMembershipRepository membershipRepo,
                           IMembershipLevelRepository levelRepo)
        {
            _userRepo = userRepo;
            _jwtGenerator = jwtGenerator;
            _mapper = mapper;

            _membershipRepo = membershipRepo;
            _levelRepo = levelRepo;
        }

        public async Task<UserReadDto> RegisterAsync(UserRegisterDto registerDto)
        {
            // 1. Kiểm tra Email đã tồn tại
            if (await _userRepo.IsEmailTakenAsync(registerDto.Email))
            {
                throw new Exception("Email đã được sử dụng.");
            }

            // 2. Tạo Entity User
            var user = _mapper.Map<User>(registerDto);

            // 3. Hash mật khẩu
            user.PasswordHash = PasswordHasher.HashPassword(registerDto.Password);
            user.EmailVerified = false; // Mặc định chưa xác minh
            user.RoleId = 1; // Mặc định là User thường (Xem CSDL: 1:User, 2:Admin, 3:Other)

            // 4. Lưu vào DB
            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();


            // ⭐️ 5. TẠO MEMBERSHIP MỚI CHO USER ⭐️

            // Lấy cấp độ cơ bản (giả định là cấp độ có MinSpendRequired = 0)
            var baseLevel = (await _levelRepo.GetAllSortedAsync()).FirstOrDefault();
            if (baseLevel == null)
            {
                // Nếu hệ thống chưa cấu hình Level, việc đăng ký sẽ thất bại
                throw new Exception("Hệ thống chưa cấu hình cấp độ thành viên cơ bản.");
            }

            var newMembership = new Membership
            {
                UserId = user.Id, // Gán ID của User vừa tạo
                LevelId = baseLevel.Id, // Gán cấp độ cơ bản
                CardCode = $"DRK-{user.Id}-{DateTime.UtcNow.Ticks}", // Tạo mã thẻ duy nhất
                TotalSpent = 0,
                LevelStartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                LevelEndDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(baseLevel.DurationDays)
            };

            await _membershipRepo.AddAsync(newMembership);
            await _membershipRepo.SaveChangesAsync(); // Lưu Membership

            return _mapper.Map<UserReadDto>(user);
        }

        public async Task<string> LoginAsync(UserLoginDto loginDto)
        {
            // 1. Tìm kiếm User bằng Username hoặc Email
            var user = await _userRepo.GetByEmailAsync(loginDto.Email);

            if (user == null)
            {
                throw new Exception("Email đăng nhập hoặc mật khẩu không chính xác.");
            }

            // 2. Kiểm tra mật khẩu
            if (!PasswordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                throw new Exception("Email đăng nhập hoặc mật khẩu không chính xác.");
            }

            // 3. Cập nhật thời gian đăng nhập cuối
            user.LastLogin = DateTime.UtcNow;
            await _userRepo.SaveChangesAsync();

            // 4. Tạo và trả về Token JWT
            return _jwtGenerator.CreateToken(user);
        }

        // Phương thức khác: Lấy User bằng PublicId (dùng cho Controller)
        public async Task<UserReadDto?> GetUserByPublicIdAsync(Guid publicId)
        {
            var user = await _userRepo.FindAsync(u => u.PublicId == publicId);

            return _mapper.Map<UserReadDto>(user.FirstOrDefault());
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