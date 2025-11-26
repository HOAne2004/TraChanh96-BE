using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Models;

public partial class DBDrinkContext : DbContext
{
    public DBDrinkContext()
    {
    }

    public DBDrinkContext(DbContextOptions<DBDrinkContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<IceLevel> IceLevels { get; set; }

    public virtual DbSet<Membership> Memberships { get; set; }

    public virtual DbSet<MembershipLevel> MembershipLevels { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Policy> Policies { get; set; }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductIceLevel> ProductIceLevels { get; set; }
    public virtual DbSet<ProductSize> ProductSizes { get; set; }
    public virtual DbSet<ProductSugarLevel> ProductSugarLevels { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<ShopTable> ShopTables { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<SugarLevel> SugarLevels { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAddress> UserAddresses { get; set; }

    public virtual DbSet<UserVoucher> UserVouchers { get; set; }

    public virtual DbSet<VoucherTemplate> VoucherTemplates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-2BE7M5SS\\SQLEXPRESS;Database=DBDrink;User Id=hoan;Password=HOAn2004;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brand__3213E83F3D8FA9ED");

            entity.ToTable("Brand");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(100)
                .HasColumnName("company_name");
            entity.Property(e => e.CopyrightText)
                .HasMaxLength(255)
                .HasColumnName("copyright_text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.EmailSupport)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email_support");
            entity.Property(e => e.Hotline)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("hotline");
            entity.Property(e => e.LogoUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("logo_url");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Slogan)
                .HasMaxLength(255)
                .HasColumnName("slogan");
            entity.Property(e => e.TaxCode)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("tax_code");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cart__3213E83FED0FE127");

            entity.ToTable("Cart");

            entity.HasIndex(e => e.UserId, "UQ__Cart__B9BE370E2DCD722C").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.Cart)
                .HasForeignKey<Cart>(d => d.UserId)
                .HasConstraintName("FK__Cart__user_id__37703C52");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cart_ite__3213E83F486E79A8");

            entity.ToTable("Cart_item");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BasePrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("base_price");
            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.FinalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("final_price");
            entity.Property(e => e.IceLevelId).HasColumnName("ice_level_id");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
            entity.Property(e => e.ParentItemId).HasColumnName("parent_item_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SizeId).HasColumnName("size_id");
            entity.Property(e => e.SugarLevelId).HasColumnName("sugar_level_id");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart_item__cart___3B40CD36");

            entity.HasOne(d => d.IceLevel).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.IceLevelId)
                .HasConstraintName("FK__Cart_item__ice_l__40058253");

            entity.HasOne(d => d.ParentItem).WithMany(p => p.InverseParentItem)
                .HasForeignKey(d => d.ParentItemId)
                .HasConstraintName("FK__Cart_item__paren__3D2915A8");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart_item__produ__3C34F16F");

            entity.HasOne(d => d.Size).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.SizeId)
                .HasConstraintName("FK__Cart_item__size___3E1D39E1");

            entity.HasOne(d => d.SugarLevel).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.SugarLevelId)
                .HasConstraintName("FK__Cart_item__sugar__3F115E1A");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3213E83F5A39BDEA");

            entity.ToTable("Category");

            entity.HasIndex(e => e.Slug, "UQ__Category__32DD1E4C00F59B8C").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.Slug)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("slug");
            entity.Property(e => e.SortOrder)
                .HasDefaultValue((byte)0)
                .HasColumnName("sort_order");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__Category__parent__3C69FB99");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comment__3213E83FE8B13CD2");

            entity.ToTable("Comment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content)
                .HasMaxLength(500)
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.NewsId).HasColumnName("news_id");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending")
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.News).WithMany(p => p.Comments)
                .HasForeignKey(d => d.NewsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__news_id__06CD04F7");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__Comment__parent___04E4BC85");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__user_id__05D8E0BE");
        });

        modelBuilder.Entity<IceLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ice_Leve__3213E83FD11018A5");

            entity.ToTable("Ice_Level");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Label)
                .HasMaxLength(20)
                .HasColumnName("label");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<Membership>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Membersh__3213E83FF483D767");

            entity.ToTable("Membership");

            entity.HasIndex(e => e.CardCode, "UQ__Membersh__81703D727EAE3828").IsUnique();

            entity.HasIndex(e => e.UserId, "UQ__Membersh__B9BE370EFC41B82F").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CardCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("card_code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.LastLevelSpentReset).HasColumnName("last_level_spent_reset");
            entity.Property(e => e.LevelEndDate).HasColumnName("level_end_date");
            entity.Property(e => e.LevelId).HasColumnName("level_id");
            entity.Property(e => e.LevelStartDate)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("level_start_date");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");
            entity.Property(e => e.TotalSpent)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("total_spent");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Level).WithMany(p => p.Memberships)
                .HasForeignKey(d => d.LevelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Membershi__level__51300E55");

            entity.HasOne(d => d.User).WithOne(p => p.Membership)
                .HasForeignKey<Membership>(d => d.UserId)
                .HasConstraintName("FK__Membershi__user___503BEA1C");
        });

        modelBuilder.Entity<MembershipLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Membersh__3213E83F3901FC68");

            entity.ToTable("Membership_Level");

            entity.HasIndex(e => e.Name, "UQ__Membersh__72E12F1BBCF20D34").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Benefits).HasColumnName("benefits");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DurationDays).HasColumnName("duration_days");
            entity.Property(e => e.MinSpendRequired)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("min_spend_required");
            entity.Property(e => e.Name)
                .HasMaxLength(35)
                .HasColumnName("name");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__News__3213E83F6A9F9780");

            entity.HasIndex(e => e.Slug, "UQ__News__32DD1E4C5A174899").IsUnique();

            entity.HasIndex(e => e.PublicId, "UQ__News__5699A53062598161").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IsFeatured)
                .HasDefaultValue(false)
                .HasColumnName("is_featured");
            entity.Property(e => e.PublicId)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("public_id");
            entity.Property(e => e.PublishedDate)
                .HasColumnType("datetime")
                .HasColumnName("published_date");
            entity.Property(e => e.SeoDescription)
                .HasMaxLength(255)
                .HasColumnName("seo_description");
            entity.Property(e => e.Slug)
                .HasMaxLength(200)
                .HasColumnName("slug");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Draft")
                .HasColumnName("status");
            entity.Property(e => e.ThumbnailUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("thumbnail_url");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.News)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__News__user_id__73BA3083");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3213E83FDE5887F1");

            entity.ToTable("Order");

            entity.HasIndex(e => e.OrderCode, "UQ__Order__99D12D3FD8ECB07F").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CoinsEarned)
                .HasDefaultValue(0)
                .HasColumnName("coins_earned");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(100)
                .HasColumnName("customer_name");
            entity.Property(e => e.CustomerPhone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("customer_phone");
            entity.Property(e => e.DeliveryAddress)
                .HasMaxLength(500)
                .HasColumnName("delivery_address");
            entity.Property(e => e.DeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("delivery_date");
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("discount_amount");
            entity.Property(e => e.GrandTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("grand_total");
            entity.Property(e => e.OrderCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("order_code");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("order_date");
            entity.Property(e => e.PaymentMethodId).HasColumnName("payment_method_id");
            entity.Property(e => e.ShippingFee)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("shipping_fee");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");
            entity.Property(e => e.StoreId).HasColumnName("store_id");
            entity.Property(e => e.StoreName)
                .HasMaxLength(200)
                .HasColumnName("store_name");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_amount");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UserNotes)
                .HasMaxLength(500)
                .HasColumnName("user_notes");
            entity.Property(e => e.VoucherCodeUsed)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("voucher_code_used");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentMethodId)
                .HasConstraintName("FK__Order__payment_m__29221CFB");

            entity.HasOne(d => d.Store).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__store_id__282DF8C2");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Order__user_id__2739D489");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order_it__3213E83F36FD5A45");

            entity.ToTable("Order_item");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BasePrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("base_price");
            entity.Property(e => e.FinalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("final_price");
            entity.Property(e => e.IceLevelId).HasColumnName("ice_level_id");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ParentItemId).HasColumnName("parent_item_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SizeId).HasColumnName("size_id");
            entity.Property(e => e.SugarLevelId).HasColumnName("sugar_level_id");

            entity.HasOne(d => d.IceLevel).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.IceLevelId)
                .HasConstraintName("FK__Order_ite__ice_l__31B762FC");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order_ite__order__2CF2ADDF");

            entity.HasOne(d => d.ParentItem).WithMany(p => p.InverseParentItem)
                .HasForeignKey(d => d.ParentItemId)
                .HasConstraintName("FK__Order_ite__paren__2EDAF651");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order_ite__produ__2DE6D218");

            entity.HasOne(d => d.Size).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.SizeId)
                .HasConstraintName("FK__Order_ite__size___2FCF1A8A");

            entity.HasOne(d => d.SugarLevel).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.SugarLevelId)
                .HasConstraintName("FK__Order_ite__sugar__30C33EC3");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment___3213E83F7896F946");

            entity.ToTable("Payment_Method");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("image_url");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.ProcessingFee)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("processing_fee");
            entity.Property(e => e.SortOrder)
                .HasDefaultValue((byte)0)
                .HasColumnName("sort_order");
        });

        modelBuilder.Entity<Policy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Policy__3213E83FB49E512D");

            entity.ToTable("Policy");

            entity.HasIndex(e => e.Slug, "UQ__Policy__32DD1E4C980885E3").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Slug)
                .HasMaxLength(100)
                .HasColumnName("slug");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Brand).WithMany(p => p.Policies)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Policy__brand_id__5BE2A6F2");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3213E83F0F2383EA");

            entity.ToTable("Product");

            entity.HasIndex(e => e.Slug, "UQ__Product__32DD1E4CAB867B39").IsUnique();

            entity.HasIndex(e => e.PublicId, "UQ__Product__5699A530AE3E7BDD").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BasePrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("base_price");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("image_url");
            entity.Property(e => e.Ingredient).HasColumnName("ingredient");
            entity.Property(e => e.LaunchDateTime)
                .HasColumnType("datetime")
                .HasColumnName("launch_date_time");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.ProductType)
                .HasMaxLength(20)
                .HasColumnName("product_type");
            entity.Property(e => e.PublicId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("public_id");
            entity.Property(e => e.SearchVector).HasColumnName("search_vector");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.TotalRating)
                .HasDefaultValue(0.0)
                .HasColumnName("total_rating");
            entity.Property(e => e.TotalSold)
                .HasDefaultValue(0)
                .HasColumnName("total_sold");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__updated__440B1D61");
        });

        // 1. ProductIceLevel: Khóa chính là (ProductId, IceLevelId)
        modelBuilder.Entity<ProductIceLevel>()
            .HasKey(p => new { p.ProductId, p.IceLevelId });

        // 2. ProductSize: Khóa chính là (ProductId, SizeId)
        modelBuilder.Entity<ProductSize>()
            .HasKey(p => new { p.ProductId, p.SizeId });

        // 3. ProductSugarLevel: Khóa chính là (ProductId, SugarLevelId)
        modelBuilder.Entity<ProductSugarLevel>()
            .HasKey(p => new { p.ProductId, p.SugarLevelId });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reservat__3213E83F5F2BFAAF");

            entity.ToTable("Reservation");

            entity.HasIndex(e => e.ReservationCode, "UQ__Reservat__FA8FADE431DEB25F").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AssignedTableId).HasColumnName("assigned_table_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(100)
                .HasColumnName("customer_name");
            entity.Property(e => e.CustomerPhone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("customer_phone");
            entity.Property(e => e.Note)
                .HasMaxLength(500)
                .HasColumnName("note");
            entity.Property(e => e.NumberOfGuests).HasColumnName("number_of_guests");
            entity.Property(e => e.ReservationCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("reservation_code");
            entity.Property(e => e.ReservationDatetime)
                .HasColumnType("datetime")
                .HasColumnName("reservation_datetime");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");
            entity.Property(e => e.StoreId).HasColumnName("store_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.AssignedTable).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.AssignedTableId)
                .HasConstraintName("FK__Reservati__assig__17C286CF");

            entity.HasOne(d => d.Store).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reservati__store__16CE6296");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Reservati__user___15DA3E5D");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Review__3213E83F5E4BD5F0");

            entity.ToTable("Review");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdminResponse).HasColumnName("admin_response");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.MediaUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("media_url");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending")
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Review__product___7F2BE32F");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Review__user_id__00200768");
        });

        modelBuilder.Entity<ShopTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Table__3213E83FCBCE513F");

            entity.ToTable("Shop_Table");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CanBeMerged)
                .HasDefaultValue(true)
                .HasColumnName("can_be_merged");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.MergedWithTableId).HasColumnName("merged_with_table_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.StoreId).HasColumnName("store_id");

            entity.HasOne(d => d.MergedWithTable).WithMany(p => p.InverseMergedWithTable)
                .HasForeignKey(d => d.MergedWithTableId)
                .HasConstraintName("FK__Table__merged_wi__0D44F85C");

            entity.HasOne(d => d.Store).WithMany(p => p.ShopTables)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Table__store_id__0C50D423");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Size__3213E83F985D1A04");

            entity.ToTable("Size");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Label)
                .HasMaxLength(20)
                .HasColumnName("label");
            entity.Property(e => e.PriceModifier)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("price_modifier");
        });

         
        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Store__3213E83FCF8EAB0E");

            entity.ToTable("Store");

            entity.HasIndex(e => e.Slug, "UQ__Store__32DD1E4C92725A74").IsUnique();

            entity.HasIndex(e => e.PublicId, "UQ__Store__5699A53072383001").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasColumnName("address");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.CloseTime).HasColumnName("close_time");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image_url");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.MapVerified)
                .HasDefaultValue(false)
                .HasColumnName("map_verified");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.OpenTime).HasColumnName("open_time");
            entity.Property(e => e.PublicId)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("public_id");
            entity.Property(e => e.Slug)
                .HasMaxLength(200)
                .HasColumnName("slug");
            entity.Property(e => e.SortOrder)
                .HasDefaultValue((byte)0)
                .HasColumnName("sort_order");

            entity.HasOne(d => d.Brand).WithMany(p => p.Stores)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Store__brand_id__5535A963");
        });

        modelBuilder.Entity<SugarLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sugar_Le__3213E83F86537186");

            entity.ToTable("Sugar_Level");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Label)
                .HasMaxLength(20)
                .HasColumnName("label");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83F0D985B4B");

            entity.ToTable("User");

            entity.HasIndex(e => e.PublicId, "UQ__User__5699A530AE2F5693").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__User__AB6E616418E7B215").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CurrentCoins)
                .HasDefaultValue(0)
                .HasColumnName("current_coins");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.EmailVerified)
                .HasDefaultValue(false)
                .HasColumnName("email_verified");
            entity.Property(e => e.LastLogin)
                .HasColumnType("datetime")
                .HasColumnName("last_login");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.PublicId)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("public_id");
            entity.Property(e => e.RoleId)
                .HasDefaultValue((byte)1)
                .HasColumnName("role_id");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");
            entity.Property(e => e.ThumbnailUrl)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("thumbnail_url");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User_Add__3213E83FE51862F8");

            entity.ToTable("User_Address");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.FullAddress)
                .HasMaxLength(500)
                .HasColumnName("full_address");
            entity.Property(e => e.IsDefault)
                .HasDefaultValue(false)
                .HasColumnName("is_default");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserAddresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__User_Addr__user___787EE5A0");
        });

        modelBuilder.Entity<UserVoucher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User_Vou__3213E83F950C18C0");

            entity.ToTable("User_Voucher");

            entity.HasIndex(e => e.VoucherCode, "UQ__User_Vou__21731069F90ADEF0").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ExpiryDate)
                .HasColumnType("datetime")
                .HasColumnName("expiry_date");
            entity.Property(e => e.IssuedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("issued_date");
            entity.Property(e => e.OrderIdUsed).HasColumnName("order_id_used");
            entity.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasColumnName("status");
            entity.Property(e => e.UsedDate)
                .HasColumnType("datetime")
                .HasColumnName("used_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.VoucherCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("voucher_code");
            entity.Property(e => e.VoucherTemplateId).HasColumnName("voucher_template_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserVouchers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User_Vouc__order__65370702");

            entity.HasOne(d => d.VoucherTemplate).WithMany(p => p.UserVouchers)
                .HasForeignKey(d => d.VoucherTemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User_Vouc__vouch__662B2B3B");
        });

        modelBuilder.Entity<VoucherTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Voucher___3213E83F0298D293");

            entity.ToTable("Voucher_Template");

            entity.HasIndex(e => e.CouponCode, "UQ__Voucher___ADE5CBB794CB76DB").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CouponCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("coupon_code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DiscountType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("discount_type");
            entity.Property(e => e.DiscountValue)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("discount_value");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LevelId).HasColumnName("level_id");
            entity.Property(e => e.MaxDiscountAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("max_discount_amount");
            entity.Property(e => e.MinOrderValue)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("min_order_value");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.QuantityPerLevel).HasColumnName("quantity_per_level");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
            entity.Property(e => e.UsageLimit).HasColumnName("usage_limit");
            entity.Property(e => e.UsageLimitPerUser).HasColumnName("usage_limit_per_user");
            entity.Property(e => e.UsedCount)
                .HasDefaultValue(0)
                .HasColumnName("used_count");

            entity.HasOne(d => d.Level).WithMany(p => p.VoucherTemplates)
                .HasForeignKey(d => d.LevelId)
                .HasConstraintName("FK__Voucher_T__level__5E8A0973");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
