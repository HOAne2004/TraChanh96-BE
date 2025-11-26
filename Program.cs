using DBDrink.Repositories;
using drinking_be.Data;
using drinking_be.Interfaces;
using drinking_be.Interfaces.CategoryInerfaces;
using drinking_be.Interfaces.MembershipInterfaces;
using drinking_be.Interfaces.NewsInterfaces;
using drinking_be.Interfaces.OptionInterfaces;
using drinking_be.Interfaces.OrderInterfaces;
using drinking_be.Interfaces.PolicyInterfaces;
using drinking_be.Interfaces.ProductInterfaces;
using drinking_be.Interfaces.ReservationInterfaces;
using drinking_be.Interfaces.ShopTableInterfaces;
using drinking_be.Interfaces.StoreInterfaces;
using drinking_be.Interfaces.UserInterfaces;
using drinking_be.Models;            // Thư mục chứa DBDrinkContext của bạn
using drinking_be.Repositories;
using drinking_be.Services;
using drinking_be.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore; // Cần thiết cho AddDbContext
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls($"http://*:{Environment.GetEnvironmentVariable("PORT") ?? "5000"}");

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
var config = builder.Configuration; // Lấy Configuration

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DBDrinkContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Sau khi đăng ký DBContext
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Tự động chuyển đổi tên thuộc tính JSON (VD: "email") 
        // thành tên thuộc tính C# (VD: "Email") và ngược lại
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/// Đăng ký Generic Repository. Dòng này sẽ bao hàm cả ICategoryRepository 
// và tất cả các Repository cụ thể khác (như Product, Order) sử dụng IGenericRepository.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Dòng đăng ký riêng cho CategoryRepository vẫn cần, vì nó kế thừa thêm Interface riêng.
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// ----------------------------------------------------------------
// THÊM ĐĂNG KÝ CHO ORDER
// ----------------------------------------------------------------
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISizeRepository, SizeRepository>();

builder.Services.AddScoped<ISugarLevelRepository, SugarLevelRepository>();
builder.Services.AddScoped<IIceLevelRepository, IceLevelRepository>();

// Đăng ký Service Layer
builder.Services.AddScoped<ICategoryService, CategoryService>();

// ----------------------------------------------------------------
// THÊM ĐĂNG KÝ CHO NEWS
// ----------------------------------------------------------------

// Repositories
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<IGenericRepository<User>, GenericRepository<User>>(); // Nếu chưa có

// Service
builder.Services.AddScoped<INewsService, NewsService>();

// ----------------------------------------------------------------
// THÊM ĐĂNG KÝ CHO STORES
// ----------------------------------------------------------------
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IStoreService, StoreService>();

// ----------------------------------------------------------------
// ĐĂNG KÝ CÁC THÀNH PHẦN USER VÀ AUTH
// ----------------------------------------------------------------
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
// CẦN THIẾT CHO JWT
// ----------------------------------------------------------------
// CẤU HÌNH AUTHENTICATION VÀ JWT BEARER
// ----------------------------------------------------------------
// CẤU HÌNH JWT
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,

        // ⭐️ 2. Bật kiểm tra Issuer và Audience để bảo mật hơn
        ValidateIssuer = true,
        ValidIssuer = config["JwtSettings:Issuer"], // Phải khớp appsettings

        ValidateAudience = true,
        ValidAudience = config["JwtSettings:Audience"], // Phải khớp appsettings

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});
// ----------------------------------------------------------------


// ----------------------------------------------------------------
// THÊM ĐĂNG KÝ CHO POLICIES
// ----------------------------------------------------------------
builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();
builder.Services.AddScoped<IPolicyService, PolicyService>();

// ----------------------------------------------------------------
// THÊM ĐĂNG KÝ CHO BRAND
// ----------------------------------------------------------------
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IBrandService, BrandService>();

// THÊM ĐĂNG KÝ CHO PAYMENT METHODS
// ----------------------------------------------------------------
builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();

// ----------------------------------------------------------------
// THÊM ĐĂNG KÝ CHO MEMBERSHIP LEVEL
// ----------------------------------------------------------------
builder.Services.AddScoped<IMembershipLevelRepository, MembershipLevelRepository>();
builder.Services.AddScoped<IMembershipLevelService, MembershipLevelService>();

// ----------------------------------------------------------------
// THÊM ĐĂNG KÝ CHO VOUCHER
// ----------------------------------------------------------------
builder.Services.AddScoped<IVoucherTemplateRepository, VoucherTemplateRepository>();
builder.Services.AddScoped<IVoucherService, VoucherService>();

// ----------------------------------------------------------------
// THÊM ĐĂNG KÝ CHO USER VOUCHER
// ----------------------------------------------------------------
builder.Services.AddScoped<IUserVoucherRepository, UserVoucherRepository>();

// ----------------------------------------------------------------
// THÊM ĐĂNG KÝ CHO MEMBERSHIP
// ----------------------------------------------------------------
builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();
builder.Services.AddScoped<IMembershipService, MembershipService>();

// ----------------------------------------------------------------
// THÊM ĐĂNG KÝ CHO REVIEW
// ----------------------------------------------------------------
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewService, ReviewService>();

// THÊM ĐĂNG KÝ CHO COMMENT
// ----------------------------------------------------------------
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();

// ----------------------------------------------------------------
// THÊM ĐĂNG KÝ CHO CART
// ----------------------------------------------------------------
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();
// Lưu ý: CartService cũng cần IProductRepository, ISizeRepository... (đã đăng ký)

// Đăng ký Service Layer (Thêm 3 dòng mới)
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISizeService, SizeService>(); // ⭐️ THÊM MỚI
builder.Services.AddScoped<ISugarLevelService, SugarLevelService>(); // ⭐️ THÊM MỚI
builder.Services.AddScoped<IIceLevelService, IceLevelService>(); // ⭐️ THÊM MỚI

// ----------------------------------------------------------------
// THÊM ĐĂNG KÝ CHO ADMIN SERVICE
// ----------------------------------------------------------------
builder.Services.AddScoped<IAdminService, AdminService>();

// ⭐️ THÊM CẤU HÌNH CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // ⭐️ THAY ĐỔI: Sửa cổng 5173 thành cổng Vue FE của bạn
                          // (Nếu dùng Vite, thường là 5173. Nếu dùng Vue CLI, thường là 8080)
                          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

                      });
});

builder.Services.AddScoped<IUploadService, UploadService>();

// Đăng ký Repository
builder.Services.AddScoped<IShopTableRepository, ShopTableRepository>();

// Đăng ký Service
builder.Services.AddScoped<IShopTableService, ShopTableService>();

// Đăng ký Repository
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

// Đăng ký Service
builder.Services.AddScoped<IReservationService, ReservationService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}




// Cho phép phục vụ các file trong thư mục wwwroot
app.UseStaticFiles();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication(); // Xác thực trước
app.UseAuthorization(); // Rồi mới phân quyền


// ⭐️ GỌI INITIALIZER (SEEDER) ⭐️
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Lấy DbContext và chạy Seeder
        var context = services.GetRequiredService<DBDrinkContext>();
        await DbInitializer.SeedData(context); // ⭐️ Gọi Seeder
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.MapControllers();


app.Run();
