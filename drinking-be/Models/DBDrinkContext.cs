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

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<IceLevel> IceLevels { get; set; }

    public virtual DbSet<Membership> Memberships { get; set; }

    public virtual DbSet<MembershipLevel> MembershipLevels { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<NewsCategory> NewsCategories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Policy> Policies { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<SocialMedia> SocialMedia { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<SugarLevel> SugarLevels { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserVoucher> UserVouchers { get; set; }

    public virtual DbSet<VoucherTemplate> VoucherTemplates { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }
    public virtual DbSet<ProductSize> ProductSizes { get; set; }
    public virtual DbSet<ProductIceLevel> ProductIceLevels { get; set; }
    public virtual DbSet<ProductSugarLevel> ProductSugarLevels { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brand__3213E83FD9DC15DE");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cart__3213E83F7E08CD61");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithOne(p => p.Cart).HasConstraintName("FK__Cart__user_id__2BFE89A6");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3213E83FF989C6D4");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK__Category__parent__3A81B327");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comment__3213E83F881ADB21");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.News).WithMany(p => p.Comments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__news_id__00200768");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK__Comment__parent___7E37BEF6");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__user_id__7F2BE32F");
        });

        modelBuilder.Entity<IceLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ice_Leve__3213E83FDC213417");
        });

        modelBuilder.Entity<Membership>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Membersh__3213E83F2F8F16D5");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.LevelStartDate).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.TotalSpent).HasDefaultValue(0m);

            entity.HasOne(d => d.Level).WithMany(p => p.Memberships)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Membershi__level__3C34F16F");

            entity.HasOne(d => d.User).WithOne(p => p.Membership).HasConstraintName("FK__Membershi__user___3B40CD36");
        });

        modelBuilder.Entity<MembershipLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Membersh__3213E83F66AD24BB");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__News__3213E83FADAEAFC7");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Status).HasDefaultValue("Draft");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Category).WithMany(p => p.News)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__News__category_i__71D1E811");

            entity.HasOne(d => d.User).WithMany(p => p.News)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__News__user_id__72C60C4A");
        });

        modelBuilder.Entity<NewsCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__News_Cat__3213E83F9852A4C8");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3213E83F7DFC6018");

            entity.Property(e => e.CoinsEarned).HasDefaultValue(0);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DiscountAmount).HasDefaultValue(0m);
            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ShippingFee).HasDefaultValue(0m);
            entity.Property(e => e.Status).HasDefaultValue((byte)1);

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Orders).HasConstraintName("FK__Order__payment_m__1DB06A4F");

            entity.HasOne(d => d.Store).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__store_id__1CBC4616");

            entity.HasOne(d => d.User).WithMany(p => p.Orders).HasConstraintName("FK__Order__user_id__1BC821DD");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order_it__3213E83F69C50F05");

            entity.HasOne(d => d.IceLevel).WithMany(p => p.OrderItems).HasConstraintName("FK__Order_ite__ice_l__2645B050");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order_ite__order__2180FB33");

            entity.HasOne(d => d.ParentItem).WithMany(p => p.InverseParentItem).HasConstraintName("FK__Order_ite__paren__236943A5");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order_ite__produ__22751F6C");

            entity.HasOne(d => d.Size).WithMany(p => p.OrderItems).HasConstraintName("FK__Order_ite__size___245D67DE");

            entity.HasOne(d => d.SugarLevel).WithMany(p => p.OrderItems).HasConstraintName("FK__Order_ite__sugar__25518C17");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment___3213E83FFE9D522A");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Policy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Policy__3213E83FA127D925");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Brand).WithMany(p => p.Policies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Policy__brand_id__571DF1D5");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3213E83F9A4C34A2");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TotalRating).HasDefaultValue(0.0);
            entity.Property(e => e.TotalSold).HasDefaultValue(0);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Category");

        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Review__3213E83FA04511AF");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Pending");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Review__product___797309D9");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Review__user_id__7A672E12");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Size__3213E83F32A074F0");

            entity.Property(e => e.PriceModifier).HasDefaultValue(0m);
        });

        modelBuilder.Entity<SocialMedia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Social_m__3213E83F8FC45427");

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Brand).WithMany(p => p.SocialMedia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Social_me__brand__48CFD27E");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Store__3213E83F07635B9C");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.Brand).WithMany(p => p.Stores)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Store__brand_id__5070F446");
        });

        modelBuilder.Entity<SugarLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sugar_Le__3213E83F25540675");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83FA72D6F13");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CurrentCoins).HasDefaultValue(0);
            entity.Property(e => e.EmailVerified).HasDefaultValue(false);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.RoleId).HasDefaultValue((byte)1);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<UserVoucher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User_Vou__3213E83F404FEF45");

            entity.Property(e => e.IssuedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue((byte)1);

            entity.HasOne(d => d.User).WithMany(p => p.UserVouchers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User_Vouc__user___4F47C5E3");

            entity.HasOne(d => d.VoucherTemplate).WithMany(p => p.UserVouchers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User_Vouc__vouch__503BEA1C");
        });

        modelBuilder.Entity<VoucherTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Voucher___3213E83F0E8C0410");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MinOrderValue).HasDefaultValue(0m);
            entity.Property(e => e.UsedCount).HasDefaultValue(0);

            entity.HasOne(d => d.Level).WithMany(p => p.VoucherTemplates).HasConstraintName("FK__Voucher_T__level__489AC854");
        });

        // ⭐️ SỬA LỖI: Ánh xạ đúng tên bảng Product_Size
        modelBuilder.Entity<ProductSize>(entity =>
        {
            entity.ToTable("Product_Size"); // <-- Quan trọng: Tên bảng trong SQL
            entity.HasKey(e => new { e.ProductId, e.SizeId }); // Khóa chính kép

            entity.HasOne(d => d.Product)
                .WithMany(p => p.ProductSizes)
                .HasForeignKey(d => d.ProductId);

            entity.HasOne(d => d.Size)
                .WithMany()
                .HasForeignKey(d => d.SizeId);
        });

        // ⭐️ SỬA LỖI: Ánh xạ đúng tên bảng Product_Ice_Level
        modelBuilder.Entity<ProductIceLevel>(entity =>
        {
            entity.ToTable("Product_Ice_Level"); // <-- Quan trọng
            entity.HasKey(e => new { e.ProductId, e.IceLevelId });

            entity.HasOne(d => d.Product)
                .WithMany(p => p.ProductIceLevels)
                .HasForeignKey(d => d.ProductId);

            entity.HasOne(d => d.IceLevel)
                .WithMany()
                .HasForeignKey(d => d.IceLevelId);
        });

        // ⭐️ SỬA LỖI: Ánh xạ đúng tên bảng Product_Sugar_Level
        modelBuilder.Entity<ProductSugarLevel>(entity =>
        {
            entity.ToTable("Product_Sugar_Level"); // <-- Quan trọng
            entity.HasKey(e => new { e.ProductId, e.SugarLevelId });

            entity.HasOne(d => d.Product)
                .WithMany(p => p.ProductSugarLevels)
                .HasForeignKey(d => d.ProductId);

            entity.HasOne(d => d.SugarLevel)
                .WithMany()
                .HasForeignKey(d => d.SugarLevelId);
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cart_Item__3213E83F69C50F06");

            entity.ToTable("Cart_Item"); // Specify table name if different

            // Properties configuration
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(e => e.Quantity)
                .IsRequired()
                .HasColumnName("quantity");

            entity.Property(e => e.Notes)
                .HasMaxLength(255)
                .HasColumnName("notes");

            // Relationships
            entity.HasOne(d => d.Cart)
                .WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.Cascade) // Xóa cart item khi cart bị xóa
                .HasConstraintName("FK__Cart_Item__cart_id__CASCADE");

            entity.HasOne(d => d.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart_Item__product_id__PRODUCT");

            entity.HasOne(d => d.Size)
                .WithMany(p => p.CartItems)
                .HasForeignKey(d => d.SizeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart_Item__size_id__SIZE");

            entity.HasOne(d => d.SugarLevel)
                .WithMany(p => p.CartItems)
                .HasForeignKey(d => d.SugarLevelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart_Item__sugar_level_id__SUGAR");

            entity.HasOne(d => d.IceLevel)
                .WithMany(p => p.CartItems)
                .HasForeignKey(d => d.IceLevelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart_Item__ice_level_id__ICE");

            // Indexes for performance
            entity.HasIndex(e => e.CartId, "IX_Cart_Item_CartId");
            entity.HasIndex(e => e.ProductId, "IX_Cart_Item_ProductId");

            // ----- BỔ SUNG CÁC CỘT DECIMAL (GIÁ) -----
            entity.Property(e => e.BasePrice)
                  .IsRequired()
                  .HasColumnType("decimal(10, 2)") // Khớp CSDL
                  .HasColumnName("base_price");

            entity.Property(e => e.FinalPrice)
                  .IsRequired()
                  .HasColumnType("decimal(10, 2)") // Khớp CSDL
                  .HasColumnName("final_price");

            // ----- BỔ SUNG CỘT PARENT_ITEM_ID -----
            entity.Property(e => e.ParentItemId)
                  .HasColumnName("parent_item_id"); // Khớp CSDL


            // ----- BỔ SUNG CẤU HÌNH QUAN HỆ TỰ THAM CHIẾU (TOPPING) -----
            entity.HasOne(d => d.ParentItem) // Một CartItem (Topping) có một ParentItem (Món chính)
                  .WithMany(p => p.InverseParentItem) // Một Món chính có nhiều Topping
                  .HasForeignKey(d => d.ParentItemId) // Khóa ngoại là ParentItemId
                  .OnDelete(DeleteBehavior.ClientSetNull) // Quan trọng: Khi xóa món chính, không tự động xóa Topping (ta sẽ xử lý ở Repository)
                  .HasConstraintName("FK__Cart_Item__parent_item_id__CART_ITEM");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
