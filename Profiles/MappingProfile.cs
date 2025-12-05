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
using drinking_be.Dtos.PolicyDtos;
using drinking_be.Dtos.ProductDtos;
using drinking_be.Dtos.ReservationDtos;
using drinking_be.Dtos.ReviewDtos;
using drinking_be.Dtos.ShopTableDtos;
using drinking_be.Dtos.StoreDtos;
using drinking_be.Dtos.UserDtos;
using drinking_be.Dtos.VoucherDtos;
using drinking_be.Enums;
using drinking_be.Models;
using System;
public class MappingProfile : Profile
{
    public MappingProfile()
    {

        // ==================================================
        // 1. PRODUCT & CATEGORY & OPTIONS
        // ==================================================

        // --- Category ---
        CreateMap<Category, CategoryReadDto>();
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryUpdateDto, Category>()
            .ForMember(dest => dest.Slug, opt => opt.Condition(src => src.Slug != null));

        // --- Product ---
        CreateMap<ProductCreateDto, Product>()
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Name.ToLower().Replace(" ", "-")));
        CreateMap<Product, ProductReadDto>();
        CreateMap<ProductUpdateDto, Product>()
            .ForMember(dest => dest.Slug, opt => opt.Condition(src => src.Slug != null))
            .ForMember(dest => dest.ProductSizes, opt => opt.Ignore())
            .ForMember(dest => dest.ProductIceLevels, opt => opt.Ignore())
            .ForMember(dest => dest.ProductSugarLevels, opt => opt.Ignore());
        
        // --- Options (Size, Sugar, Ice) ---

        // Read DTOs
        CreateMap<Size, SizeReadDto>();
        CreateMap<SugarLevel, SugarLevelReadDto>();
        CreateMap<IceLevel, IceLevelDto>();

        // Create DTOs -> Entity
        CreateMap<SizeCreateDto, Size>();
        CreateMap<IceLevelCreateDto, IceLevel>();
        CreateMap<SugarLevelCreateDto, SugarLevel>();

        // ==================================================
        // 2. ORDER & CART
        // ==================================================

        // --- Order ---
        CreateMap<Order, OrderReadDto>()
            .ForMember(dest => dest.Status,
                       opt => opt.MapFrom(src => ((OrderStatusEnum)src.Status).ToString()))
            .ForMember(dest => dest.Items, opt => opt.Ignore());

        CreateMap<OrderItem, OrderItemReadDto>()
            .ForMember(dest => dest.ProductName, opt => opt.Ignore())
            .ForMember(dest => dest.SizeLabel, opt => opt.Ignore())
            .ForMember(dest => dest.SugarLabel, opt => opt.Ignore())
            .ForMember(dest => dest.IceLabel, opt => opt.Ignore())
            .ForMember(dest => dest.Toppings, opt => opt.Ignore());

        CreateMap<OrderItem, OrderToppingReadDto>()
          .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
          .ForMember(dest => dest.ProductName, opt => opt.Ignore()) // Sẽ ánh xạ thủ công ở Service
          .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.BasePrice));

        // --- Cart ---
        CreateMap<Cart, CartReadDto>()
            .ForMember(dest => dest.Items, opt => opt.Ignore())
            .ForMember(dest => dest.TotalAmount, opt => opt.Ignore());

        CreateMap<CartItem, CartItemReadDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl))
            .ForMember(dest => dest.SizeLabel, opt => opt.MapFrom(src => src.Size != null ? src.Size.Label : ""))
            .ForMember(dest => dest.SugarLabel, opt => opt.MapFrom(src => src.SugarLevel != null ? src.SugarLevel.Label : ""))
            .ForMember(dest => dest.IceLabel, opt => opt.MapFrom(src => src.IceLevel != null ? src.IceLevel.Label : ""))
            .ForMember(dest => dest.Toppings, opt => opt.Ignore());

        CreateMap<CartItem, CartToppingReadDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

        // ==================================================
        // 3. USER & MEMBERSHIP
        // ==================================================

        // --- User ---
        CreateMap<User, UserReadDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src =>
            src.RoleId == 2 ? "Admin" : (src.RoleId == 3 ? "Manager" : "User")
            ));
        CreateMap<UserRegisterDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        // --- Membership ---
        CreateMap<Membership, MembershipReadDto>()
            .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src => src.Level.Name))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username));
        CreateMap<MembershipLevel, MembershipLevelReadDto>();
        CreateMap<MembershipLevelCreateDto, MembershipLevel>();

        // ==================================================
        // 4. STORE & BRAND & TABLE
        // ==================================================

        // --- Brand ---
        CreateMap<Brand, BrandReadDto>()
           .ForMember(dest => dest.SocialMedia, opt => opt.MapFrom(src => src.SocialMedia));
        CreateMap<BrandCreateDto, Brand>();
        CreateMap<BrandUpdateDto, Brand>();

        CreateMap<SocialMedia, SocialMediaDto>();

        // --- Store ---
        CreateMap<Store, StoreReadDto>()
           .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name)); // Cần Include Brand
        CreateMap<StoreCreateDto, Store>()
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src =>
                 src.Name.ToLower().Replace(" ", "-") + "-" + DateTime.UtcNow.Ticks)); // Tạo slug đơn giản
        CreateMap<StoreUpdateDto, Store>()
            .ForMember(dest => dest.Slug, opt => opt.Condition(src => src.Slug != null));
        
        // --- ShopTable ---
        CreateMap<ShopTable, ShopTableReadDto>()
            .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store != null ? src.Store.Name : null));
        CreateMap<ShopTableCreateDto, ShopTable>();
        CreateMap<ShopTableUpdateDto, ShopTable>();

        // ==================================================
        // 5. RESERVATION
        // ==================================================
        CreateMap<Reservation, ReservationReadDto>()
            .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store != null ? src.Store.Name : "N/A"))
            .ForMember(dest => dest.AssignedTableName, opt => opt.MapFrom(src => src.AssignedTable != null ? src.AssignedTable.Name : null))
            .ForMember(dest => dest.StatusLabel, opt => opt.MapFrom(src => ((ReservationStatusEnum)src.Status).ToString()));

        CreateMap<ReservationCreateDto, Reservation>();
        CreateMap<ReservationUpdateDto, Reservation>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // ==================================================
        // 6. NEWS & INTERACTION (Review/Comment)
        // ==================================================

        // --- News ---
        CreateMap<News, NewsReadDto>()
             .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<NewsCreateDto, News>()
             .ForMember(dest => dest.Slug, opt => opt.MapFrom(src =>
                 src.Title.ToLower().Replace(" ", "-") + "-" + DateTime.UtcNow.Ticks)) // Tạo slug đơn giản
             .ForMember(dest => dest.PublishedDate, opt => opt.Condition(src => src.Status == "Published")); // Chỉ đặt nếu trạng thái là Published
        CreateMap<NewsUpdateDto, News>();

        // --- Review ---
        CreateMap<Review, ReviewReadDto>()
           .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
           .ForMember(dest => dest.UserThumbnailUrl, opt => opt.MapFrom(src => src.User.ThumbnailUrl)); // Cần Include User

        CreateMap<ReviewCreateDto, Review>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore());

        // --- Comment ---
        CreateMap<Comment, CommentReadDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.UserThumbnailUrl, opt => opt.MapFrom(src => src.User.ThumbnailUrl)); // Cần Include User

        CreateMap<CommentCreateDto, Comment>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore());

        // ==================================================
        // 7. VOUCHER & PAYMENT & POLICY
        // ==================================================

        // --- Voucher ---
        CreateMap<VoucherTemplate, VoucherTemplateReadDto>()
                .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src => src.LevelId.HasValue ? src.Level.Name : "Tất cả"));
        CreateMap<VoucherTemplateCreateDto, VoucherTemplate>();

        CreateMap<UserVoucher, UserVoucherReadDto>()
            .ForMember(dest => dest.TemplateName, opt => opt.MapFrom(src => src.VoucherTemplate.Name))
            .ForMember(dest => dest.DiscountValue, opt => opt.MapFrom(src => src.VoucherTemplate.DiscountValue))
            .ForMember(dest => dest.DiscountType, opt => opt.MapFrom(src => src.VoucherTemplate.DiscountType))
            .ForMember(dest => dest.MinOrderValue, opt => opt.MapFrom(src => src.VoucherTemplate.MinOrderValue))
            .ForMember(dest => dest.MaxDiscountAmount, opt => opt.MapFrom(src => src.VoucherTemplate.MaxDiscountAmount));

        // --- Payment ---
        CreateMap<PaymentMethod, PaymentMethodReadDto>();
        CreateMap<PaymentMethodCreateDto, PaymentMethod>();

        // --- Policy ---
        CreateMap<Policy, PolicyReadDto>();
        CreateMap<PolicyCreateDto, Policy>()
             .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Title.ToLower().Replace(" ", "-")));
        // ----------------------------------------------------------------
        }
}