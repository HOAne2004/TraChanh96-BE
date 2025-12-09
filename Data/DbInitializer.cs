using drinking_be.Models;
using drinking_be.Utils;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(DBDrinkContext context)
        {
            // --- 1. Membership Levels ---
            if (!await context.MembershipLevels.AnyAsync())
            {
                context.MembershipLevels.AddRange(
                    new MembershipLevel { Name = "Đồng", MinSpendRequired = 0m, DurationDays = 30, CreatedAt = DateTime.UtcNow, Benefits = "{}" },
                    new MembershipLevel { Name = "Bạc", MinSpendRequired = 280000m, DurationDays = 30, CreatedAt = DateTime.UtcNow, Benefits = "{}" },
                    new MembershipLevel { Name = "Vàng", MinSpendRequired = 600000m, DurationDays = 30, CreatedAt = DateTime.UtcNow, Benefits = "{}" },
                    new MembershipLevel { Name = "Kim Cương", MinSpendRequired = 1000000m, DurationDays = 30, CreatedAt = DateTime.UtcNow, Benefits = "{}" }
                );
                await context.SaveChangesAsync();
            }

            // --- 2. Admin User ---
            if (!await context.Users.AnyAsync(u => u.Email == "admin@drink.vn"))
            {
                var baseLevel = await context.MembershipLevels.FirstOrDefaultAsync(l => l.Name == "Đồng");
                var adminUser = new User
                {
                    PublicId = Guid.NewGuid(),
                    Username = "Admin",
                    Email = "admin@drink.vn", // Sửa lại email cho khớp check
                    PasswordHash = PasswordHasher.HashPassword("Admin@123"),
                    RoleId = 2,
                    Status = 1,
                    CreatedAt = DateTime.UtcNow,
                };
                context.Users.Add(adminUser);
                await context.SaveChangesAsync();

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

            // --- 3. Options (Size, Sugar, Ice) ---
            if (!await context.Sizes.AnyAsync())
            {
                context.Sizes.AddRange(
                    new Size { Label = "Nhỏ", PriceModifier = 0, IsActive = true },
                    new Size { Label = "Vừa", PriceModifier = 5000, IsActive = true },
                    new Size { Label = "Lớn", PriceModifier = 10000, IsActive = true }
                );
                context.SugarLevels.AddRange(
                    new SugarLevel { Label = "0%", Value = 0, IsActive = true },
                    new SugarLevel { Label = "50%", Value = 50, IsActive = true },
                    new SugarLevel { Label = "100%", Value = 100, IsActive = true }
                );
                context.IceLevels.AddRange(
                    new IceLevel { Label = "Không đá", Value = 0, IsActive = true },
                    new IceLevel { Label = "50%", Value = 50, IsActive = true },
                    new IceLevel { Label = "100%", Value = 100, IsActive = true }
                );
                await context.SaveChangesAsync();
            }

            // --- 4. Categories ---
            if (!await context.Categories.AnyAsync())
            {
                context.Categories.AddRange(
                    new Category { Name = "Trà Trái Cây", Slug = "tra-trai-cay", IsActive = true },
                    new Category { Name = "Trà Sữa", Slug = "tra-sua", IsActive = true },
                    new Category { Name = "Cà Phê", Slug = "ca-phe", IsActive = true },
                    new Category { Name = "Topping", Slug = "topping", IsActive = true }
                );
                await context.SaveChangesAsync();
            }

            // --- 5. Products ---
            if (!await context.Products.AnyAsync())
            {
                var cateTraSua = await context.Categories.FirstOrDefaultAsync(c => c.Slug == "tra-sua");
                var cateTraicay = await context.Categories.FirstOrDefaultAsync(c => c.Slug == "tra-trai-cay");

                if (cateTraSua != null && cateTraicay != null)
                {
                    // Lấy ID options
                    var sizeIds = await context.Sizes.Select(s => s.Id).ToListAsync();
                    var sugarIds = await context.SugarLevels.Select(s => s.Id).ToListAsync();
                    var iceIds = await context.IceLevels.Select(s => s.Id).ToListAsync();

                    var products = new List<Product>
                    {
                        new Product
                        {
                            PublicId = Guid.NewGuid().ToString(), // Thêm PublicId
                            Name = "Trà Sữa Trân Châu Đường Đen",
                            Slug = "ts-tran-chau-duong-den",
                            BasePrice = 35000,
                            CategoryId = cateTraSua.Id,
                            ProductType = "Beverage",
                            // Dùng ảnh online để demo luôn hiển thị được
                            ImageUrl = "https://images.unsplash.com/photo-1558160074-4d7d8bdf4256?auto=format&fit=crop&w=500&q=60", 
                            Description = "Hương vị đường đen thơm lừng kết hợp sữa tươi.",
                            Status = "Active",
                            TotalSold = 150,
                            TotalRating = 4.8,
                            LaunchDateTime = DateTime.UtcNow
                        },
                        new Product
                        {
                            PublicId = Guid.NewGuid().ToString(),
                            Name = "Trà Chanh Dây Kim Quất",
                            Slug = "tra-chanh-day-kim-quat",
                            BasePrice = 30000,
                            CategoryId = cateTraicay.Id,
                            ProductType = "Beverage",
                            ImageUrl = "https://images.unsplash.com/photo-1513558161293-cdaf765ed2fd?auto=format&fit=crop&w=500&q=60",
                            Description = "Giải nhiệt mùa hè cực đã.",
                            Status = "Active",
                            TotalSold = 89,
                            TotalRating = 4.5,
                            LaunchDateTime = DateTime.UtcNow
                        }
                    };

                    context.Products.AddRange(products);
                    await context.SaveChangesAsync();

                    // Gán Option (Product_Size...)
                    foreach (var p in products)
                    {
                        foreach (var sId in sizeIds) context.ProductSizes.Add(new ProductSize { ProductId = p.Id, SizeId = sId });
                        foreach (var suId in sugarIds) context.ProductSugarLevels.Add(new ProductSugarLevel { ProductId = p.Id, SugarLevelId = suId });
                        foreach (var iId in iceIds) context.ProductIceLevels.Add(new ProductIceLevel { ProductId = p.Id, IceLevelId = iId });
                    }
                    await context.SaveChangesAsync();
                }
            }

            // --- 6. News ---
            if (!await context.News.AnyAsync())
            {
                var admin = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin@drink.vn");
                if (admin != null)
                {
                    context.News.AddRange(
                        new News { 
                            Title = "Khai trương chi nhánh mới tại Cầu Giấy", 
                            Slug = "khai-truong-cau-giay", // Thêm Slug
                            Content = "Nội dung chi tiết...", 
                            Type = "Tin tức",
                            Status = "Published",
                            UserId = admin.Id,
                            PublishedDate = DateTime.UtcNow,
                            ThumbnailUrl = "https://images.unsplash.com/photo-1559305616-3a9922835b84?auto=format&fit=crop&w=500&q=60"
                        },
                        new News { 
                            Title = "Ưu đãi mua 1 tặng 1 cuối tuần", 
                            Slug = "uu-dai-mua-1-tang-1",
                            Content = "Nội dung chi tiết...", 
                            Type = "Khuyến mãi",
                            Status = "Published",
                            UserId = admin.Id,
                            PublishedDate = DateTime.UtcNow.AddDays(-2),
                            ThumbnailUrl = "https://images.unsplash.com/photo-1623944894372-23023e32df02?auto=format&fit=crop&w=500&q=60"
                        }
                    );
                    await context.SaveChangesAsync();
                }
            }
            
            // --- 7. Stores ---
             if (!await context.Stores.AnyAsync())
            {
                // 1. Logic tạo/tìm Brand chính
                if (!await context.Brands.AnyAsync())
                {
                    context.Brands.Add(new Brand { Name = "Trà Chanh 96" });
                    await context.SaveChangesAsync();
                }

                // 2. ⭐️ CHỈ CẦN LẤY LẠI THÔNG TIN: KHÔNG DÙNG "VAR" LẦN NỮA
                var brand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "Trà Chanh 96");
                if (brand != null) {
                     context.Stores.AddRange(
                        new Store { Name = "Trà Chanh 96 - Cầu Giấy", Slug="tc-cau-giay", Address = "123 Cầu Giấy, Hà Nội", BrandId = brand.Id, OpenDate = new DateTime(2019, 11, 19), OpenTime = new TimeSpan(8,0,0), CloseTime = new TimeSpan(22,0,0), ImageUrl="https://images.unsplash.com/photo-1554118811-1e0d58224f24?auto=format&fit=crop&w=500&q=60", ShippingFee=10000 },
                        new Store { Name = "Trà Chanh 96 - Đống Đa", Slug="tc-dong-da", Address = "456 Xã Đàn, Hà Nội", BrandId = brand.Id, OpenDate = new DateTime(2021, 06,07), OpenTime = new TimeSpan(8,0,0), CloseTime = new TimeSpan(23,0,0), ImageUrl="https://images.unsplash.com/photo-1559925393-8be0ec4767c8?auto=format&fit=crop&w=500&q=60", ShippingFee = 15000 }
                     );
                     await context.SaveChangesAsync();
                 }
            }


        }
    }
}