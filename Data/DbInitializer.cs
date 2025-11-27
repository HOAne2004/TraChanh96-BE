// Data/DbInitializer.cs
using drinking_be.Models;
using drinking_be.Utils; // Cần PasswordHasher
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace drinking_be.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(DBDrinkContext context)
        {
            // 1. Kiểm tra và Khởi tạo Membership Levels (Rất quan trọng!)
            if (!await context.MembershipLevels.AnyAsync())
            {
                context.MembershipLevels.AddRange(
  new MembershipLevel
  {
      Name = "Đồng",
      MinSpendRequired = 0m,
      DurationDays = 30,
      CreatedAt = DateTime.UtcNow,
      Benefits = "{}"
  },
    new MembershipLevel
    {
        Name = "Bạc",
        MinSpendRequired = 280000m,
        DurationDays = 30,
        CreatedAt = DateTime.UtcNow,
        Benefits = "{}"
    },
    new MembershipLevel
    {
        Name = "Vàng",
        MinSpendRequired = 600000m,
        DurationDays = 30,
        CreatedAt = DateTime.UtcNow,
        Benefits = "{}"
    },
    new MembershipLevel
    {
        Name = "Kim Cương",
        MinSpendRequired = 1000000m,
        DurationDays = 30,
        CreatedAt = DateTime.UtcNow,
        Benefits = "{}"
    }
);
                await context.SaveChangesAsync();
            }

            // 2. Kiểm tra và tạo Admin User
            if (await context.Users.AnyAsync(u => u.Email == "admin@drink.vn"))
            {
                return; // Đã tồn tại
            }

            var baseLevel = await context.MembershipLevels.FirstOrDefaultAsync(l => l.Name == "Đồng");

            var adminUser = new User
            {
                PublicId = Guid.NewGuid(),
                Username = "Admin",
                Email = "admin@abc.com",
                PasswordHash = PasswordHasher.HashPassword("Admin@123"), // Mật khẩu mặc định: Admin@123
                RoleId = 2, // 2: Admin
                Status = 1, // Active
                CreatedAt = DateTime.UtcNow,
            };

            context.Users.Add(adminUser);
            await context.SaveChangesAsync();

            // 3. Tạo Membership cho Admin User
            if (baseLevel != null)
            {
                context.Memberships.Add(new Membership
                {
                    UserId = adminUser.Id,
                    LevelId = baseLevel.Id,
                    CardCode = $"ADM-{adminUser.Id}-{DateTime.UtcNow.Ticks}",
                    LevelStartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    LevelEndDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(baseLevel.DurationDays)
                });
                await context.SaveChangesAsync();
            }
        }
    }
}