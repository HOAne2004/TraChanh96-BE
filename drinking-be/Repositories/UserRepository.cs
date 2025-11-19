// Repositories/UserRepository.cs
using drinking_be.Interfaces.UserInterfaces;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace drinking_be.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly DBDrinkContext _context;

        public UserRepository(DBDrinkContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            // Phải đảm bảo so sánh không phân biệt chữ hoa/thường (tùy thuộc vào CSDL)
            return await _context.Users // Giả định DbSet là Users
                                 .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            // Kiểm tra xem Email đã tồn tại chưa
            return await _context.Users
                                 .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }
        public async Task UpdateLastLoginAsync(User user)
        {
            // Attach user vào context nếu nó chưa được theo dõi
            if (_context.Entry(user).State == EntityState.Detached)
            {
                _context.Users.Attach(user);
            }

            // Chỉ đánh dấu cột LastLogin là đã sửa đổi
            _context.Entry(user).Property(u => u.LastLogin).IsModified = true;

            // Lưu thay đổi
            await _context.SaveChangesAsync();
        }
    }
}