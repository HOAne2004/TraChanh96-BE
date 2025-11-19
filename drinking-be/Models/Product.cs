using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace drinking_be.Models;

[Table("Product")]
[Index("Slug", Name = "UQ__Product__32DD1E4C39E40483", IsUnique = true)]
[Index("PublicId", Name = "UQ__Product__5699A5309C7C0C8D", IsUnique = true)]
public partial class Product
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("public_id")]
    [StringLength(36)]
    [Unicode(false)]
    public string PublicId { get; set; } = null!;

    [Column("category_id")]
    public int CategoryId { get; set; }

    [Column("slug")]
    [StringLength(255)]
    public string Slug { get; set; } = null!;

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("product_type")]
    [StringLength(20)]
    public string ProductType { get; set; } = null!;

    [Column("base_price", TypeName = "decimal(10, 2)")]
    public decimal BasePrice { get; set; }

    [Column("image_url")]
    [StringLength(500)]
    [Unicode(false)]
    public string? ImageUrl { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("ingredient")]
    public string? Ingredient { get; set; }

    [Column("status")]
    [StringLength(20)]
    public string Status { get; set; } = null!;

    [Column("total_rating")]
    public double? TotalRating { get; set; }

    [Column("total_sold")]
    public int? TotalSold { get; set; }

    [Column("launch_date_time", TypeName = "datetime")]
    public DateTime? LaunchDateTime { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();


    [InverseProperty("Product")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [InverseProperty("Product")]
    public ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
    [InverseProperty("Product")]
    public ICollection<ProductIceLevel> ProductIceLevels { get; set; } = new List<ProductIceLevel>();
    [InverseProperty("Product")]
    public ICollection<ProductSugarLevel> ProductSugarLevels { get; set; } = new List<ProductSugarLevel>();
}
