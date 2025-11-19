// Models/CartItem.cs (ĐÃ SỬA LỖI)
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace drinking_be.Models
{
    [Table("Cart_item")] // Khớp CSDL
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; } // bigint

        [Required]
        public long CartId { get; set; } // bigint
        public virtual Cart Cart { get; set; } = null!;

        [Required]
        public int ProductId { get; set; } // Sửa thành int
        public virtual Product Product { get; set; } = null!;

        [Required]
        public int Quantity { get; set; } // int

        // ⭐️ THÊM: Các trường giá bị thiếu
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal BasePrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal FinalPrice { get; set; }

        // ⭐️ SỬA: Tên cột và cho phép NULL
        [MaxLength(255)]
        [Column("note")] // Khớp tên cột 'note' trong CSDL
        public string? Notes { get; set; } // Cho phép NULL

        // ⭐️ THÊM: Logic Topping (ParentItem)
        public long? ParentItemId { get; set; } // Cho phép NULL
        public virtual CartItem? ParentItem { get; set; }
        public virtual ICollection<CartItem> InverseParentItem { get; set; } = new List<CartItem>();

        // ⭐️ SỬA: Tùy chọn phải là NULLABLE
        public short? SizeId { get; set; } // Cho phép NULL
        public virtual Size? Size { get; set; }

        public short? SugarLevelId { get; set; } // Cho phép NULL
        public virtual SugarLevel? SugarLevel { get; set; }

        public short? IceLevelId { get; set; } // Cho phép NULL
        public virtual IceLevel? IceLevel { get; set; }

        // ⭐️ XÓA: CSDL không có 'CreatedAt'
        // public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}