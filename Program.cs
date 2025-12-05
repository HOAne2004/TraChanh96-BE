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

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var builder = WebApplication.CreateBuilder(args);
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// 1. CẤU HÌNH PORT (OK)
// Quan trọng: Railway sẽ inject PORT vào biến môi trường
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

// 2. KẾT NỐI DATABASE (OK)
//builder.Services.AddDbContext<DBDrinkContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 👇 SỬA ĐOẠN NÀY: Tách biến ra và in Log
var dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// In ra Console để xem trên Railway nó đang đọc được cái quái gì
// Dấu '' giúp bạn nhìn thấy nếu có khoảng trắng thừa ở đầu/cuối
Console.WriteLine($"👉 [DEBUG CHECK] Connection String: '{dbConnectionString}'"); 

builder.Services.AddDbContext<DBDrinkContext>(options =>
{
    // Nếu biến môi trường bị null hoặc sai, ta thử Hardcode (Dán cứng) luôn để test
    // options.UseNpgsql("Host=shortline.proxy.rlwy.net;Port=39042;Database=railway;Username=postgres;Password=NHVSywfdYybBiGbJQZyoyLVkvaWIJSkx;");
    
    // Dùng biến lấy từ cấu hình
    options.UseNpgsql(dbConnectionString);
});

// 3. CẤU HÌNH ROUTING CHỮ THƯỜNG (SỬA LỖI)
// ⚠️ Lỗi cũ: Bạn đặt dòng này SAU khi app.Build(). Phải đặt ở đây.
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// 4. AUTO MAPPER & SERVICES
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// --- ĐĂNG KÝ REPOSITORIES & SERVICES ---
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Product & Options
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISizeRepository, SizeRepository>();
builder.Services.AddScoped<ISizeService, SizeService>();
builder.Services.AddScoped<ISugarLevelRepository, SugarLevelRepository>();
builder.Services.AddScoped<ISugarLevelService, SugarLevelService>();
builder.Services.AddScoped<IIceLevelRepository, IceLevelRepository>();
builder.Services.AddScoped<IIceLevelService, IceLevelService>();

// Order & Cart
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();

// User & Auth
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddScoped<IAdminService, AdminService>();

// Others (News, Store, Policy, Brand, Voucher, Review, Comment...)
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();
builder.Services.AddScoped<IPolicyService, PolicyService>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddScoped<IMembershipLevelRepository, MembershipLevelRepository>();
builder.Services.AddScoped<IMembershipLevelService, MembershipLevelService>();
builder.Services.AddScoped<IVoucherTemplateRepository, VoucherTemplateRepository>();
builder.Services.AddScoped<IVoucherService, VoucherService>();
builder.Services.AddScoped<IUserVoucherRepository, UserVoucherRepository>();
builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();
builder.Services.AddScoped<IMembershipService, MembershipService>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IShopTableRepository, ShopTableRepository>();
builder.Services.AddScoped<IShopTableService, ShopTableService>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IReservationService, ReservationService>();

// 5. JSON OPTIONS
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// 6. SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 7. CẤU HÌNH JWT
var config = builder.Configuration;
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
        ValidateIssuer = true,
        ValidIssuer = config["JwtSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = config["JwtSettings:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// 8. CẤU HÌNH CORS (CHO PHÉP TẤT CẢ ĐỂ FIX LỖI)
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            // ⭐️ Quan trọng: Dùng SetIsOriginAllowed(origin => true) để mở hoàn toàn
            // Thay vì liệt kê cứng link Vercel, giúp tránh lỗi sai http/https hoặc www
            policy.SetIsOriginAllowed(origin => true) 
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
});

// =========================================================
// BUILD APP
// =========================================================
var app = builder.Build();

// 9. CẤU HÌNH PIPELINE (MIDDLEWARE)

// ⭐️ SỬA LỖI SWAGGER: Đưa ra ngoài if (IsDevelopment)
// Để Swagger chạy được trên Railway (Production)
app.UseSwagger();
app.UseSwaggerUI();

// ⭐️ LƯU Ý: Trên Railway (Docker/Linux), đôi khi UseHttpsRedirection gây lỗi vòng lặp
// Nếu app chạy lỗi, hãy thử comment dòng này lại. Hiện tại cứ để.
// app.UseHttpsRedirection(); 

app.UseStaticFiles();

// ⭐️ CORS phải đặt giữa StaticFiles và Auth
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

// 10. DATABASE SEEDER
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DBDrinkContext>();
        // Tự động migrate nếu chưa có DB (quan trọng cho Postgres)
        await context.Database.MigrateAsync(); 
        await DbInitializer.SeedData(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.MapControllers();

app.Run();