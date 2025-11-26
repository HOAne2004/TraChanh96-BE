using AutoMapper;
using drinking_be.Dtos.BrandDtos;
using drinking_be.Dtos.CartDtos;
using drinking_be.Dtos.CategoryDtos;
using drinking_be.Dtos.CommentDtos;
using drinking_be.Dtos.MembershipDtos;
using drinking_be.Dtos.MembershipLevelDtos;
using drinking_be.Dtos.NewsDtos;
using drinking_be.Dtos.OptionDtos;
using drinking_be.Dtos.OrderDtos;
using drinking_be.Dtos.PaymentMethodDtos;
using drinking_be.Dtos.ProductDtos;
using drinking_be.Dtos.ReservationDtos;
using drinking_be.Dtos.ReviewDtos;
using drinking_be.Dtos.ShopTableDtos;
using drinking_be.Dtos.StoreDtos;
using drinking_be.Dtos.UserDtos;
using drinking_be.Dtos.VoucherDtos;
using drinking_be.Enums;
using drinking_be.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // --- Mapping từ Model (CSDL) sang DTO (Trả về) ---
        CreateMap<Category, CategoryReadDto>();

        // --- Mapping từ DTO (Input) sang Model (CSDL) ---
        // AutoMapper sẽ tự tạo ra Slug và các trường khác
        CreateMap<CategoryCreateDto, Category>();

        // --- Product Mapping ---
        // Mapping Input DTO sang Model (CSDL)
        CreateMap<ProductCreateDto, Product>()
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Name.ToLower().Replace(" ", "-")));

        // Mapping Model sang Read DTO
        CreateMap<Product, ProductReadDto>();

        // Entity OrderItem (Topping) -> OrderToppingReadDto
        CreateMap<OrderItem, OrderToppingReadDto>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.ProductName, opt => opt.Ignore()) // Sẽ ánh xạ thủ công ở Service
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.BasePrice));

        // Entity OrderItem (Món chính) -> OrderItemReadDto
        CreateMap<OrderItem, OrderItemReadDto>()
            // Bỏ qua các trường cần tra cứu (Name/Label)
            .ForMember(dest => dest.ProductName, opt => opt.Ignore())
            .ForMember(dest => dest.SizeLabel, opt => opt.Ignore())
            .ForMember(dest => dest.SugarLabel, opt => opt.Ignore())
            .ForMember(dest => dest.IceLabel, opt => opt.Ignore())
            // Bỏ qua Toppings vì chúng ta sẽ ánh xạ thủ công (hoặc sử dụng cấu trúc ProjectTo)
            .ForMember(dest => dest.Toppings, opt => opt.Ignore());

        // Entity Order -> OrderReadDto
        CreateMap<Order, OrderReadDto>()
            // Ánh xạ Status từ short sang chuỗi (Enum)
            .ForMember(dest => dest.Status,
                       opt => opt.MapFrom(src => ((OrderStatusEnum)src.Status).ToString()))
            // Bỏ qua Items vì chúng ta sẽ tự điền Items đã được ánh xạ ở Service
            .ForMember(dest => dest.Items, opt => opt.Ignore());

        // Entity News -> NewsReadDto
        CreateMap<News, NewsReadDto>()
             .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        // DTO NewsCreateDto -> Entity News
        CreateMap<NewsCreateDto, News>()
             .ForMember(dest => dest.Slug, opt => opt.MapFrom(src =>
                 src.Title.ToLower().Replace(" ", "-") + "-" + DateTime.UtcNow.Ticks)) // Tạo slug đơn giản
             .ForMember(dest => dest.PublishedDate, opt => opt.Condition(src => src.Status == "Published")); // Chỉ đặt nếu trạng thái là Published

        // ----------------------------------------------------------------
        // --- Store Mappings ---
        // ----------------------------------------------------------------

        // Entity Store -> StoreReadDto
        CreateMap<Store, StoreReadDto>()
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name)); // Cần Include Brand

        // DTO StoreCreateDto -> Entity Store
        CreateMap<StoreCreateDto, Store>()
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src =>
                 src.Name.ToLower().Replace(" ", "-") + "-" + DateTime.UtcNow.Ticks)); // Tạo slug đơn giản

        // ----------------------------------------------------------------
        // --- Brand Mappings ---
        // ----------------------------------------------------------------

        // Entity Brand -> BrandReadDto
        CreateMap<Brand, BrandReadDto>();

        // DTO BrandCreateDto -> Entity Brand
        CreateMap<BrandCreateDto, Brand>();

        // Trong Constructor của MappingProfile:

        CreateMap<SocialMedia, SocialMediaDto>();

        CreateMap<Brand, BrandReadDto>()
            .ForMember(dest => dest.SocialMedia, opt => opt.MapFrom(src => src.SocialMedia));

        // ----------------------------------------------------------------
        // --- Payment Method Mappings ---
        // ----------------------------------------------------------------

        // Entity PaymentMethod -> PaymentMethodReadDto
        CreateMap<PaymentMethod, PaymentMethodReadDto>();

        // DTO PaymentMethodCreateDto -> Entity PaymentMethod
        CreateMap<PaymentMethodCreateDto, PaymentMethod>();

        // ----------------------------------------------------------------
        // --- Membership Level Mappings ---
        // ----------------------------------------------------------------

        // Entity MembershipLevel -> MembershipLevelReadDto
        CreateMap<MembershipLevel, MembershipLevelReadDto>();

        // DTO MembershipLevelCreateDto -> Entity MembershipLevel
        CreateMap<MembershipLevelCreateDto, MembershipLevel>();

        // ----------------------------------------------------------------
        // --- Voucher Template Mappings ---
        // ----------------------------------------------------------------

        // Entity VoucherTemplate -> VoucherTemplateReadDto
        CreateMap<VoucherTemplate, VoucherTemplateReadDto>()
            // Ánh xạ LevelName từ Navigation Property, xử lý null
            .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src =>
                src.LevelId.HasValue ? src.Level.Name : "Tất cả"));

        // DTO VoucherTemplateCreateDto -> Entity VoucherTemplate
        CreateMap<VoucherTemplateCreateDto, VoucherTemplate>();

        // ----------------------------------------------------------------
        // --- User Voucher Mappings ---
        // ----------------------------------------------------------------

        // Entity UserVoucher -> UserVoucherReadDto
        CreateMap<UserVoucher, UserVoucherReadDto>()
            // Ánh xạ chi tiết từ Template lồng nhau
            .ForMember(dest => dest.TemplateName, opt => opt.MapFrom(src => src.VoucherTemplate.Name))
            .ForMember(dest => dest.DiscountValue, opt => opt.MapFrom(src => src.VoucherTemplate.DiscountValue))
            .ForMember(dest => dest.DiscountType, opt => opt.MapFrom(src => src.VoucherTemplate.DiscountType))
            .ForMember(dest => dest.MinOrderValue, opt => opt.MapFrom(src => src.VoucherTemplate.MinOrderValue))
            .ForMember(dest => dest.MaxDiscountAmount, opt => opt.MapFrom(src => src.VoucherTemplate.MaxDiscountAmount));

        // ----------------------------------------------------------------
        // --- Membership Mappings ---
        // ----------------------------------------------------------------

        // Entity Membership -> MembershipReadDto
        CreateMap<Membership, MembershipReadDto>()
            .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src => src.Level.Name))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username));

        // ----------------------------------------------------------------
        // --- Review Mappings ---
        // ----------------------------------------------------------------

        // Entity Review -> ReviewReadDto
        CreateMap<Review, ReviewReadDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.UserThumbnailUrl, opt => opt.MapFrom(src => src.User.ThumbnailUrl)); // Cần Include User

        // DTO ReviewCreateDto -> Entity Review
        CreateMap<ReviewCreateDto, Review>()
            // UserId sẽ được gán thủ công trong Service
            .ForMember(dest => dest.UserId, opt => opt.Ignore());

        // ----------------------------------------------------------------
        // --- Comment Mappings ---
        // ----------------------------------------------------------------

        // Entity Comment -> CommentReadDto
        CreateMap<Comment, CommentReadDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.UserThumbnailUrl, opt => opt.MapFrom(src => src.User.ThumbnailUrl)); // Cần Include User

        // DTO CommentCreateDto -> Entity Comment
        CreateMap<CommentCreateDto, Comment>()
            // UserId sẽ được gán thủ công trong Service
            .ForMember(dest => dest.UserId, opt => opt.Ignore());

        // ----------------------------------------------------------------
        // --- Cart Mappings ---
        // ----------------------------------------------------------------

        // Entity CartItem (Món chính) -> CartItemReadDto
        CreateMap<CartItem, CartItemReadDto>()
            // Ánh xạ các trường cần Join (Giả định Repository đã Include)
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl))
            .ForMember(dest => dest.SizeLabel, opt => opt.MapFrom(src => src.Size != null ? src.Size.Label : ""))
            .ForMember(dest => dest.SugarLabel, opt => opt.MapFrom(src => src.SugarLevel != null ? src.SugarLevel.Label : ""))
            .ForMember(dest => dest.IceLabel, opt => opt.MapFrom(src => src.IceLevel != null ? src.IceLevel.Label : ""))
            .ForMember(dest => dest.Toppings, opt => opt.Ignore()); // Sẽ map thủ công trong Service

        // Entity CartItem (Topping) -> CartToppingReadDto
        CreateMap<CartItem, CartToppingReadDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

        // Entity Cart -> CartReadDto
        CreateMap<Cart, CartReadDto>()
            .ForMember(dest => dest.Items, opt => opt.Ignore()) // Sẽ map thủ công
            .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()); // Sẽ tính toán thủ công

        // ----------------------------------------------------------------
        // --- Option Mappings (Size, Sugar, Ice) ---
        // ----------------------------------------------------------------

        CreateMap<Size, SizeDto>();
        CreateMap<SugarLevel, SugarLevelDto>();
        CreateMap<IceLevel, IceLevelDto>();

        // 1. Map cho Size
        CreateMap<SizeCreateDto, Size>();

        // 2. Map cho IceLevel
        CreateMap<IceLevelCreateDto, IceLevel>();

        // 3. Map cho SugarLevel
        CreateMap<SugarLevelCreateDto, SugarLevel>();

        // ----------------------------------------------------------------
        // --- User Mappings ---
        // ----------------------------------------------------------------

        // 1. Ánh xạ DTO (Input) sang Model (CSDL)
        CreateMap<UserRegisterDto, User>()
            // Bỏ qua Password, vì chúng ta hash nó thủ công trong Service
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        // 2. Ánh xạ Model (CSDL) sang DTO (Trả về)
        CreateMap<User, UserReadDto>()
            // Ánh xạ RoleId (byte/tinyint) sang Role (string)
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src =>
                src.RoleId == 2 ? "Admin" : (src.RoleId == 3 ? "Manager" : "User")
            ));

        // ---ShopTable Mapping-- -
        // Map từ Entity sang ReadDto
        CreateMap<ShopTable, ShopTableReadDto>()
            .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store != null ? src.Store.Name : null));

        // Map từ CreateDto sang Entity
        CreateMap<ShopTableCreateDto, ShopTable>();

        // Map từ UpdateDto sang Entity
        CreateMap<ShopTableUpdateDto, ShopTable>();

        // --- Reservation Mapping ---
        CreateMap<Reservation, ReservationReadDto>()
            .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store != null ? src.Store.Name : "N/A"))
            .ForMember(dest => dest.AssignedTableName, opt => opt.MapFrom(src => src.AssignedTable != null ? src.AssignedTable.Name : null))
            .ForMember(dest => dest.StatusLabel, opt => opt.MapFrom(src => ((ReservationStatusEnum)src.Status).ToString()));

        CreateMap<ReservationCreateDto, Reservation>();

        // UpdateDto map thủ công trong Service vì logic phức tạp, hoặc map fields đơn giản
        CreateMap<ReservationUpdateDto, Reservation>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}